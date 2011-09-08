using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Xml;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder
{
    [Obsolete("will be replaced by file format conversion")]
    internal static class PackageManager
    {
        #region internal methods
        internal static void UnpackResources(string file)
        {
            lock (thisLock)
            {
                string packName = string.Empty; // InformationProvider.FindFreePath(App.DataDir, Path.GetFileNameWithoutExtension(file), false);
                if (!Directory.Exists(packName))
                    Directory.CreateDirectory(packName);

                using (Package zip = Package.Open(file, FileMode.Open, FileAccess.Read))
                {
                    foreach (PackageRelationship relation in zip.GetRelationships())
                    {
                        string type = relation.RelationshipType;
                        if (type != "ChameleonCoder://Package.Resource" && type != "ChameleonCoder://Package.Resource.Map"
                            && type != "ChameleonCoder://Package.Resource.ResolvedResource" && type != "ChameleonCoder://Package.Resource.FSComponent")
                            continue;

                        Uri source = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                        PackagePart part = zip.GetPart(source);

                        Uri targetPath = new Uri(new Uri(packName + "\\", UriKind.Absolute),
                            new Uri(part.Uri.ToString().TrimStart('/'), UriKind.Relative));

                        using (FileStream fileStream = new FileStream(targetPath.LocalPath, FileMode.Create))
                            CopyStream(part.GetStream(), fileStream);
                    }
                }
                //App.ParseDir(packName);
            }
        }

        internal static void PackageResources(IEnumerable<IResource> resources)
        {
            lock (thisLock)
            {
                currentMap = new XmlDocument();
                currentMap.LoadXml("<cc-resource-map/>");

                string packPath = Path.GetTempFileName();
                File.Delete(packPath);

                using (zip = Package.Open(packPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    foreach (IResource resource in resources)
                    {
                        string path = GetPath(resource);
                        PackagePart part = GetPackagePart(path, GetPartUri(resource), "ChameleonCoder/Resource");
                        if (part != null)
                        {
                            zip.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource");
                            (currentMap.DocumentElement.AppendChild(currentMap.CreateElement("file"))).InnerText = GetPartUri(resource).OriginalString.TrimStart('/');
                        }
                        File.Delete(path);

                        AddFSComponent(resource as IFSComponent, part, part);
                        AddResolved(resource as IResolvable, part, part);
                        foreach (IResource child in resource.children)
                            AddChild(child, part, part);
                    }
                    string mapPath = Path.GetTempFileName();
                    currentMap.Save(mapPath);

                    PackagePart mapPart = GetPackagePart(mapPath, GetPartUri("package.ccm"), "ChameleonCoder/ResourceMap");
                    zip.CreateRelationship(mapPart.Uri, TargetMode.Internal, "ChameleonCoder://Package.ResourceMap");

                    currentMap = null;
                    File.Delete(mapPath);
                }
                File.Move(packPath, InformationProvider.FindFreePath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource pack.ccp", true));
            }
        }
        #endregion

        #region URI & Path
        private static Uri GetPartUri(IResource resource)
        {
            return PackUriHelper.CreatePartUri(new Uri(resource.Name + "." + resource.GUID.ToString("N") + ".ccr", UriKind.Relative));
        }

        private static Uri GetPartUri(string fsPath)
        {
            return PackUriHelper.CreatePartUri(new Uri(Path.GetFileName(fsPath), UriKind.Relative));
        }

        private static string GetPath(IResource resource)
        {
            string path = Path.GetTempFileName();
            if (resource.Parent == null)
                File.Copy(resource.GetResourceFile().FilePath, path, true);
            else
                File.WriteAllText(path, resource.Xml.OuterXml);
            return path;
        }
        #endregion

        #region Add~
        private static void AddFSComponent(IFSComponent resource, PackagePart parent, PackagePart ancestor)
        {
            if (resource != null)
            {
                PackagePart fs = GetPackagePart(resource.GetFSPath(), GetPartUri(resource.GetFSPath()), "ChameleonCoder.FSComponent");

                parent.CreateRelationship(fs.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.FSComponent");
                fs.CreateRelationship(parent.Uri, TargetMode.Internal, "ChameleonCoder://Package.FSComponent.Resource");

                ancestor.CreateRelationship(fs.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Descendant");
                fs.CreateRelationship(ancestor.Uri, TargetMode.Internal, "ChameleonCoder://Package.FSComponent.Ancestor");
            }
        }

        private static void AddResolved(IResolvable resource, PackagePart parent, PackagePart ancestor)
        {
            if (resource != null)
            {
                IResource target = resource.Resolve();

                PackagePart res = GetPackagePart(GetPath(target), GetPartUri(target), "ChameleonCoder/Resource");

                parent.CreateRelationship(res.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Resolved");
                res.CreateRelationship(parent.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Origin");

                ancestor.CreateRelationship(res.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Descendant");
                res.CreateRelationship(ancestor.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Ancestor");

                (currentMap.DocumentElement.AppendChild(currentMap.CreateElement("file"))).InnerText = GetPartUri(target).OriginalString.TrimStart('/');

                AddFSComponent(target as IFSComponent, res, ancestor);
                AddResolved(target as IResolvable, res, ancestor);
                foreach (IResource child in target.children)
                    AddChild(child, res, ancestor);
            }
        }

        private static void AddChild(IResource child, PackagePart parent, PackagePart ancestor)
        {
            AddFSComponent(child as IFSComponent, parent, ancestor);
            AddResolved(child as IResolvable, parent, ancestor);
            foreach (IResource grandChild in child.children)
                AddChild(grandChild, parent, ancestor);
        }
        #endregion

        #region helper methods
        private static PackagePart GetPackagePart(string path, Uri target, string contentType)
        {
            if (zip.PartExists(target))
                return zip.GetPart(target);

            if (File.Exists(path))
            {
                PackagePart part = zip.CreatePart(target, contentType, CompressionOption.Maximum);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                    CopyStream(stream, part.GetStream());

                return part;
            }
            return null;
        }

        private static void CopyStream(Stream source, Stream target)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buffer, 0, 4096)) > 0)
                target.Write(buffer, 0, bytesRead);
        }
        #endregion

        #region fields & properties
        static object thisLock = new object();
        static XmlDocument currentMap;
        static bool? _includeFS = null;
        static bool? _includeTarget = null;

        static bool includeFS
        {
            get
            {
                /*if (Properties.Settings.Default.UseDefaultPackageSettings)
                    return Properties.Settings.Default.Package_IncludeFSComponent;*/

                if (_includeFS == null)
                    _includeFS = MessageBox.Show(Properties.Resources.Pack_IncludeFS,
                                                Properties.Resources.Status_Pack,
                                                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                return _includeFS == true;
            }
        }

        static bool includeTarget
        {
            get
            {
                /*if (Properties.Settings.Default.UseDefaultPackageSettings)
                    return Properties.Settings.Default.Package_IncludeTarget;*/

                if (_includeTarget == null)
                    _includeTarget = MessageBox.Show(Properties.Resources.Pack_IncludeResolve,
                                                    Properties.Resources.Status_Pack,
                                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                return _includeTarget == true;
            }
        }

        static Package zip;
        #endregion
    }
}
