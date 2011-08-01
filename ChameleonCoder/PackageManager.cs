using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using ChameleonCoder.Resources.Interfaces;
using System.Windows;
using System.IO.Packaging;

namespace ChameleonCoder
{
    internal static class PackageManager
    {
        static object lock_ = new object();
        static XmlDocument currentMap;
        static bool includeFS;
        static bool includeTarget;

        internal static void UnpackResources(string file)
        {
            lock (lock_)
            {
                string packName = FindFreePath(App.AppDir + "\\Data\\", Path.GetFileNameWithoutExtension(file), false);

                using (Package zip = Package.Open(file, FileMode.Open, FileAccess.Read))
                {
                    foreach (PackageRelationship relation in zip.GetRelationshipsByType("ChameleonCoder.Package.Resource").Concat(zip.GetRelationshipsByType("ChameleonCoder.Package.ResourceMap")))
                    {
                        Uri source = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                        PackagePart part = zip.GetPart(source);
                        Uri targetPath = new Uri(new Uri(App.AppDir + "\\Data\\" + packName, UriKind.Absolute),
                            new Uri(part.Uri.ToString().TrimStart('/'), UriKind.Relative));
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath.LocalPath));
                        using (FileStream fileStream = new FileStream(targetPath.LocalPath, FileMode.Create))
                        {
                            CopyStream(part.GetStream(), fileStream);
                        }
                    }
                }
            }
        }

        

        internal static void PackageResources(IList<IResource> resources)
        {
            lock (lock_)
            {
                if (!Directory.Exists(App.AppDir + "\\Temp"))
                    Directory.CreateDirectory(App.AppDir + "\\Temp");

                string tempzip = FindFreePath(App.AppDir + "\\Temp", "temp_zip", true);

                if (ChameleonCoder.Properties.Settings.Default.UseDefaultPackageSettings)
                {
                    includeFS = ChameleonCoder.Properties.Settings.Default.Package_IncludeFSComponent;
                    includeTarget = ChameleonCoder.Properties.Settings.Default.Package_IncludeTarget;
                }
                else
                {
                    includeFS =
                            MessageBox.Show("Do you want to include the file system equivalents for the packaged resources?",
                                "CC - packaging...",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                    includeTarget =
                            MessageBox.Show("Do you want to include the target resources for the packaged resources?",
                                "CC- packaging...",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                }

                List<PackagePart> parts = new List<PackagePart>();
                currentMap = new XmlDocument();
                currentMap.LoadXml("<cc-project-map/>");

                using (Package zip = Package.Open(tempzip, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    foreach (IResource resource in resources)
                        AddPackagePart(zip, resource, parts);
                    foreach (PackagePart part in parts)
                        zip.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder.Package.Resource");

                    string mapPath = FindFreePath(App.AppDir + "\\Temp", "package.ccm", true);
                    currentMap.Save(mapPath);

                    PackagePart mapPart = GetPackagePart(zip, mapPath);
                    zip.CreateRelationship(mapPart.Uri, TargetMode.Internal, "ChameleonCoder.Package.ResourceMap");

                    currentMap = null;
                    File.Delete(mapPath);
                }

                File.Move(tempzip, FindFreePath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource pack.ccp", true));
                MessageBox.Show("mission complete!");
            }
        }

        private static void AddPackagePart(Package zip, IResource resource, List<PackagePart> collection)
        {
            string path;
            if (resource.Parent == null)
            {
                path = FindFreePath(App.AppDir + "\\Temp",
                    Path.GetFileNameWithoutExtension(resource.GetResourceFile()) + "." + resource.GUID.ToString("n") + Path.GetExtension(resource.GetResourceFile()), true);
                File.Copy(resource.GetResourceFile(), path);
            }
            else
            {
                path = FindFreePath(App.AppDir, resource.Name + "." + resource.GUID.ToString("n") + ".ccr", true);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resource.Xml.OuterXml);
                doc.Save(path);
            }
            PackagePart newPart = GetPackagePart(zip, path);

            if (newPart != null)
                collection.Add(newPart);
            currentMap.DocumentElement.InnerXml += "<file>" + Path.GetFileName(path) + "</file>";

            File.Delete(path);

            GetRequiredParts(zip, resource, collection);
        }

        private static PackagePart GetPackagePart(Package zip, string path)
        {
            Uri target = PackUriHelper.CreatePartUri(
                            new Uri(Path.GetFileName(path), UriKind.Relative));

            if (!zip.PartExists(target))
            {
                PackagePart part = zip.CreatePart(target, System.Net.Mime.MediaTypeNames.Text.Xml);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(stream, part.GetStream());
                }
                return part;
            }
            return null;
        }

        private static void GetRequiredParts(Package zip, IResource parent, List<PackagePart> parts)
        {
            if (parent is IFSComponent && includeFS)
                parts.Add(GetPackagePart(zip, (parent as IFSComponent).GetFSPath()));
            if (parent is IResolvable && includeTarget)
                AddPackagePart(zip, (parent as IResolvable).Resolve(), parts);

            foreach (IResource child in parent.children)
                GetRequiredParts(zip, child, parts);
        }

        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }

        private static string FindFreePath(string directory, string baseName, bool isFile)
        {
            directory = directory[directory.Length - 1] == Path.DirectorySeparatorChar
                ? directory : directory + Path.DirectorySeparatorChar;

            baseName = baseName.TrimStart(Path.DirectorySeparatorChar);

            string fileName = isFile
                ? Path.GetFileNameWithoutExtension(baseName) : baseName;

            string Extension = isFile
                ? Path.GetExtension(baseName) : string.Empty;

            string path = directory + fileName + Extension;
            int i = 0;

            while ((isFile ? File.Exists(path) : Directory.Exists(path)))
            {
                path = directory + fileName + "_" + i + Extension;
                i++;
            }

            return path;
        }
    }
}
