using System;
using System.Runtime.InteropServices;
using ChameleonCoder.Files;

namespace ChameleonCoder.Resources
{
    [ComVisible(false), Guid("5DBC99B9-88E0-4F9E-B0F2-D8A24C015A22"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IResourceManagerEvents))]
    public sealed class ResourceManager : IResourceManager
    {
        internal ResourceManager(IChameleonCoderApp app)
        {
            App = app;
        }

        public IChameleonCoderApp App
        {
            get;
            private set;
        }

        /// <summary>
        /// contains all resources that don't have a direct parent (top-level resources)
        /// </summary>
        private ResourceCollection childrenCollection;

        /// <summary>
        /// contains a list of ALL resources
        /// </summary>
        private ResourceCollection allResources;

        /// <summary>
        /// gets the currently loaded resource
        /// </summary>
        public IResource ActiveResource
        {
            get;
            private set;
        }

        /// <summary>
        /// sets the collection instances used by this class.
        /// </summary>
        /// <param name="flat">the instance to use as flat list of all resources</param>
        /// <param name="hierarchy">the instance to use as list of top-level resources</param>
        /// <remarks>this is needed to make it possible to create the instances in XAML,
        /// from where they can be referenced in the UI.</remarks>
        internal void SetCollections(ResourceCollection flat, ResourceCollection hierarchy)
        {
            allResources = flat;
            childrenCollection = hierarchy;
        }

        /// <summary>
        /// adds a resource
        /// 1) to the list of ALL resources
        /// 2) to a given parent list OR the list of top-level resources
        /// It also assigns a method to the resource's 'PropertyChanged' event
        /// </summary>
        /// <param name="instance">the resource to add</param>
        /// <param name="parent">the parent to add the resource to.
        /// If this is null, it will be added to the list of top-level resources</param>
        public void Add(IResource instance, IResource parent)
        {
            allResources.Add(instance);
            if (parent == null)
            {
                childrenCollection.Add(instance);
            }
            else
            {
                parent.AddChild(instance);
            }
            instance.PropertyChanged += OnPropertyChanged;

            IRichContentResource richResource = instance as IRichContentResource;
            if (richResource != null) // if it is really a RichContentResource:
            {
                richResource.MakeRichContent(); // parse the RichContent
            }

            foreach (var refAttr in instance.File.ResourceParseReferences(instance))
            {
                instance.AddReference(new ResourceReference(refAttr, instance.File));
            }
        }

        public void AddRange(IResource[] resources, IResource parent)
        {
            foreach (IResource res in resources)
                Add(res, parent);
        }

        /// <summary>
        /// removes the resource
        /// 1) from the list of ALL resources
        /// 2) from its parent's Children list OR from the list of top-level resources.
        /// It also removes the handler from the 'PropertyChanged' event.
        /// </summary>
        /// <param name="instance">the instance to remove</param>
        public void Remove(IResource instance)
        {
            allResources.Remove(instance);

            if (instance.Parent == null)
            {
                childrenCollection.Remove(instance);
            }
            else
            {
                instance.Parent.RemoveChild(instance);
            }

            instance.PropertyChanged -= OnPropertyChanged;
        }

        public void Delete(IResource resource)
        {
            foreach (IResource child in resource.Children) // remove references to all child resources
            {
                if (ActiveResource == child)
                   Close(); // if a child is loaded: unload it
                Remove(child);
            }

            if (ActiveResource == resource)
                Close(); // unload the resource to delete
            Remove(resource);

            resource.File.ResourceDelete(resource);
            resource.File.Save(); // save changes
        }

        public IResource Move(IResource resource, IResource newParent)
        {
            if (resource.Parent == newParent)
                return resource;

            Guid id = resource.Identifier;

            CopyResource(resource, newParent, true);
            Delete(resource);

            return GetResource(id);
        }

        public IResource Copy(IResource resource, IResource newParent)
        {
            CopyResource(resource, newParent, false);
            return null; // Todo!
        }

        [Obsolete]
        private void CopyResource(IResource resource, IResource newParent, bool moveGUID) // TODO!!!!
        {
            var file = newParent == null ? resource.File : newParent.File;

            // ======================================================================

            // BAD:
            var doc = ((DataFile)file).Document; // HACK!
            var manager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("cc", DataFile.NamespaceUri);

            var element = (System.Xml.XmlElement)resource.Xml.CloneNode(true); // get a clone for the copy
            if (element.OwnerDocument != doc) //if we switch the document:
                element = (System.Xml.XmlElement)doc.ImportNode(element, true); // import the XmlElement

            if (newParent == null) // if no parent:
                doc.SelectSingleNode(DataFile.DocumentXPath.ResourceRoot, manager).AppendChild(element); // add element to resource list
            else // if parent:
                newParent.Xml.AppendChild(element); // add element to parent's Children

            // =====================================================================

            // GOOD:
            if (moveGUID) // if the copy should receive the original Identifier:
            {
                // resource.Xml.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b"));
                resource.Attributes["id"] = Guid.NewGuid().ToString("b"); // set the Identifier-attribute of the old instance
                resource.Update(resource.Attributes, resource.Parent, file); // update it to apply the changes
            }
            // =====================================================================

            // BAD:
            else // if the copy receives a new Identifier:
                element.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b")); // set the appropriate attribute

            ((DataFile)file).LoadResource(element, newParent); // let the DataFile class create an instance, add it to the lists, init it, ... // HACK!

            // ===============================================================================

            resource.File.Save(); // save the documents
            if (newParent != null)
                newParent.File.Save();
        }

        public IResource Create(Guid key, string name, System.Collections.Specialized.IObservableStringDictionary attributes, IResource parent, IDataFile file)
        {
            attributes["name"] = name; attributes["type"] = key.ToString("B");

            Type type = App.ResourceTypeMan.GetResourceType(key);
            IResource resource = App.ResourceTypeMan.GetFactory(type).CreateInstance(type, attributes, parent, file);
            if (resource != null)
            {
                App.ResourceMan.Add(resource, parent);
                file.ResourceInsert(resource, parent);
                file.ResourceSetCreatedDate(resource);
            }

            return resource;
        }

        /// <summary>
        /// gets the Children list
        /// </summary>
        public IResource[] ChildResources
        {
            get
            {
                return childrenCollection.Values;
            }
        }

        public IResource[] Resources
        {
            get { return allResources.Values; }
        }

        public IResource GetResource(Guid id)
        {
            return allResources.GetInstance(id);
        }

        /// <summary>
        /// opens a resource
        /// </summary>
        /// <param name="resource">the resource to open</param>
        public void Open(IResource resource)
        {
            if (ActiveResource != null)
                Close();

            OnResourceLoad(resource);

            ActiveResource = resource;

            ILanguageResource langRes;

            if ((langRes = resource as ILanguageResource) != null)
            {
                if (App.PluginMan.ActiveModule != null
                    && langRes.Language != App.PluginMan.ActiveModule.Identifier)
                {
                    App.PluginMan.UnloadModule();
                    if (App.PluginMan.IsModuleRegistered(langRes.Language))
                        App.PluginMan.LoadModule(App.PluginMan.GetModule(langRes.Language));
                }
            }
            OnResourceLoaded(resource);
        }

        /// <summary>
        /// closes the loaded resource
        /// </summary>
        public void Close()
        {
            if (ActiveResource != null)
            {
                OnResourceUnload(ActiveResource);

                ILanguageResource langRes = ActiveResource as ILanguageResource;
                if (langRes != null)
                {
                    if (App.PluginMan.ActiveModule != null && App.PluginMan.ActiveModule.Identifier == langRes.Language)
                        App.PluginMan.UnloadModule();
                }

                var item = ActiveResource;
                ActiveResource = null;

                OnResourceUnloaded(item);
            }
            else
                throw new InvalidOperationException("no resource can be closed.");
        }

        public void RemoveAll()
        {
            foreach (IResource resource in allResources)
            {
                Remove(resource); // use this so that event handlers are removed correctly etc.
            }
        }

        public void Shutdown()
        {
            Close();
            RemoveAll();

            App = null;
        }

        #region paths

        /// <summary>
        /// gets an array of GUIDs representing the path to the resource
        /// </summary>
        /// <param name="resource">the resource to get the path for</param>
        /// <returns>the path array, with the first item being the identifier of top-level ancestor and the last one the identifier of the passed resource</returns>
        public Guid[] GetIdPath(IResource resource)
        {
            var path = new System.Collections.Generic.List<Guid>();

            while (resource != null)
            {
                path.Add(resource.Identifier);
                resource = resource.Parent; // go up the resource tree
            }

            path.Reverse(); // make the GUID of the top-level ancestor be the first, the resource itself the last item
            return path.ToArray();
        }

        /// <summary>
        /// gets a resource from a given GUID path as returned by <see cref="GetIdPath"/>.
        /// </summary>
        /// <param name="path">the path array of GUIDs</param>
        /// <returns>the resource if it exists within this ResourceManager, null otherwise</returns>
        public IResource GetFromIdPath(Guid[] path)
        {
            IResource result = null;
            var collection = ChildResources;
            int currentIndex = 0;

            foreach (Guid currentId in path)
            {
                currentIndex++;
                foreach (IResource res in collection)
                {
                    if (res.Identifier.Equals(currentId))
                    {
                        if (path.Length > currentIndex)
                            collection = res.Children;
                        else if (path.Length == currentIndex)
                            result = res;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// determines whether a resource is an ancestor of another one
        /// </summary>
        /// <param name="descendant">the resource to test</param>
        /// <param name="ancestor">the resource that may be an ancestor of <paramref name="descendant"/>.</param>
        /// <returns>true if the <paramref name="ancestor"/> is really an ancestor, false otherwise.</returns>
        public bool IsAncestorOf(IResource descendant, IResource ancestor)
        {
            Guid[] pathAncestor = GetIdPath(ancestor);
            Guid[] pathDescendant = GetIdPath(descendant);
            int index = 0;

            foreach (Guid id in pathAncestor)
            {
                if (!id.Equals(pathDescendant[index]))
                    return false;
                index++;
            }

            return true;
        }

        /// <summary>
        /// determines whether a resource is a descendant of another one
        /// </summary>
        /// <param name="ancestor">the resource to test</param>
        /// <param name="descendant">the resource that may be a descendant of <paramref name="ancestor"/>.</param>
        /// <returns>true if the <paramref name="descendant"/> is really a descendant, false otherwise.</returns>
        public bool IsDescendantOf(IResource ancestor, IResource descendant)
        {
            return IsAncestorOf(descendant, ancestor);
        }

        /// <summary>
        /// gets a user-friendly path to the resource using the resource names
        /// </summary>
        /// <param name="resource">the resource to get the path for</param>
        /// <param name="separator">the string to separate the resource names</param>
        public string GetDisplayPath(IResource resource, string separator)
        {
            string path = string.Empty;

            while (resource != null)
            {
                path = separator + resource.Name + path;
                resource = resource.Parent;
            }

            return path;
        }

        [Obsolete]
        public IResource GetResourceFromDisplayPath(string path, string separator)
        {
            var start = ChameleonCoder.Properties.Resources.Item_Home + separator + ChameleonCoder.Properties.Resources.Item_List;
            if (path.StartsWith(start, StringComparison.Ordinal))
                path = path.Remove(0, start.Length);

            var collection = ChildResources;
            string[] segments = path.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            IResource result = null;
            int i = 0;
            foreach (string segment in segments)
            {
                i++;
                foreach (IResource res in collection)
                {
                    if (res.Name != segment)
                        continue;
                    if (segments.Length > i)
                        collection = res.Children;
                    else if (segments.Length == i)
                        result = res;
                    break;
                }
            }
            return result;
        }

        #endregion // "paths"

        #region events

        /// <summary>
        /// raised when a resource is going to be loaded
        /// </summary>
        public event ResourceEventHandler ResourceLoad;

        /// <summary>
        /// raised when a resource was loaded
        /// </summary>
        public event ResourceEventHandler ResourceLoaded;

        /// <summary>
        /// raised when a resource is going to be unloaded
        /// </summary>
        public event ResourceEventHandler ResourceUnload;

        /// <summary>
        /// raised when a resource was unloaded
        /// </summary>
        public event ResourceEventHandler ResourceUnloaded;

        #endregion

        #region event infrastructure

        /// <summary>
        /// raises the ResourceLoad event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal void OnResourceLoad(IResource resource)
        {
            ResourceEventHandler handler = ResourceLoad;
            if (handler != null)
                handler(this, new ResourceEventArgs(resource));
        }

        /// <summary>
        /// raises the ResourceLoaded event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal void OnResourceLoaded(IResource resource)
        {
            ResourceEventHandler handler = ResourceLoaded;
            if (handler != null)
                handler(this, new ResourceEventArgs(resource));
        }

        /// <summary>
        /// raises the ResourceUnload event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal void OnResourceUnload(IResource resource)
        {
            ResourceEventHandler handler = ResourceUnload;
            if (handler != null)
                handler(this, new ResourceEventArgs(resource));
        }

        /// <summary>
        /// raises the ResourceUnloaded event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal void OnResourceUnloaded(IResource resource)
        {
            ResourceEventHandler handler = ResourceUnloaded;
            if (handler != null)
                handler(this, new ResourceEventArgs(resource));
        }

        #endregion

        /// <summary>
        /// handles changes to a resource and updates the 'last-modified' timestamp
        /// </summary>
        /// <param name="sender">the resource that changed</param>
        /// <param name="args">additional data</param>
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            IResource resource = sender as IResource;
            if (resource != null)
                resource.File.ResourceUpdateLastModified(resource);
        }
    }
}
