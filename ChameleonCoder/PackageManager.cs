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
    internal static class PackageManager
    {
        static object thislock = new object();
        static XmlDocument currentMap;
        static bool? _includeFS = null;
        static bool? _includeTarget = null;

        static bool includeFS
        {
            get
            {
                if (Properties.Settings.Default.UseDefaultPackageSettings)
                    return Properties.Settings.Default.Package_IncludeFSComponent;

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
                if (Properties.Settings.Default.UseDefaultPackageSettings)
                    return Properties.Settings.Default.Package_IncludeTarget;

                if (_includeTarget == null)
                    _includeTarget = MessageBox.Show(Properties.Resources.Pack_IncludeResolve,
                                                    Properties.Resources.Status_Pack,
                                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                return _includeTarget == true;
            }
        }

        internal static void UnpackResources(string file)
        {
            lock (thislock)
            {
                string packName = InformationProvider.FindFreePath(App.DataDir, Path.GetFileNameWithoutExtension(file), false);
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
                App.ParseDir(packName);
            }
        }

        internal static void PackageResources(IList<IResource> resources)
        {
            lock (thislock)
            {
                if (!Directory.Exists(App.AppDir + "\\Temp"))
                    Directory.CreateDirectory(App.AppDir + "\\Temp");

                string tempzip = InformationProvider.FindFreePath(App.AppDir + "\\Temp", "pack.tmp", true);

                currentMap = new XmlDocument();
                currentMap.LoadXml("<cc-project-map/>");

                using (Package zip = Package.Open(tempzip, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    foreach (IResource resource in resources)
                        AddPackagePart(zip, resource, "ChameleonCoder://Package.Resource");

                    string mapPath = InformationProvider.FindFreePath(App.AppDir + "\\Temp", "package.ccm", true);
                    currentMap.Save(mapPath);

                    PackagePart mapPart = GetPackagePart(zip, mapPath, System.Net.Mime.MediaTypeNames.Text.Xml);
                    zip.CreateRelationship(mapPart.Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.Map");

                    currentMap = null;
                    File.Delete(mapPath);
                }

                File.Move(tempzip, InformationProvider.FindFreePath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource pack.ccp", true));
                MessageBox.Show(Properties.Resources.Pack_Finished);
            }
        }

        private static void AddPackagePart(Package zip, IResource resource, string relation)
        {
            #region 'path'
            string path;
            if (resource.Parent == null)
            {
                path = InformationProvider.FindFreePath(App.AppDir + "\\Temp",
                    Path.GetFileNameWithoutExtension(resource.GetResourceFile()) + "." + resource.GUID.ToString("n") + Path.GetExtension(resource.GetResourceFile()), true);
                File.Copy(resource.GetResourceFile(), path);
            }
            else
            {
                path = InformationProvider.FindFreePath(App.AppDir, resource.Name + "." + resource.GUID.ToString("n") + ".ccr", true);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resource.Xml.OuterXml);
                doc.Save(path);
            }
            #endregion

            PackagePart newPart = GetPackagePart(zip, path, System.Net.Mime.MediaTypeNames.Text.Xml);
            if (newPart != null)
            {
                zip.CreateRelationship(newPart.Uri, TargetMode.Internal, relation);
                currentMap.DocumentElement.InnerXml += "<file>" + Path.GetFileName(path) + "</file>";
            }

            File.Delete(path);

            GetRequiredParts(zip, resource);
        }

        private static PackagePart GetPackagePart(Package zip, string path, string contentType)
        {
            Uri target = PackUriHelper.CreatePartUri(
                            new Uri(Path.GetFileName(path), UriKind.Relative));

            if (!zip.PartExists(target))
            {
                PackagePart part = zip.CreatePart(target, contentType);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    CopyStream(stream, part.GetStream());
                }
                return part;
            }
            return null;
        }

        private static void GetRequiredParts(Package zip, IResource parent)
        {
            if (parent is IFSComponent && includeFS)
                zip.CreateRelationship(GetPackagePart(zip, (parent as IFSComponent).GetFSPath(), "unknown").Uri, TargetMode.Internal, "ChameleonCoder://Package.Resource.FSComponent");
            if (parent is IResolvable && includeTarget)
                AddPackagePart(zip, (parent as IResolvable).Resolve(), "ChameleonCoder://Package.Resource.ResolvedResource");

            foreach (IResource child in parent.children)
                GetRequiredParts(zip, child);
        }

        private static void CopyStream(Stream source, Stream target)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buffer, 0, 4096)) > 0)
                target.Write(buffer, 0, bytesRead);
        }

        
    }
}
