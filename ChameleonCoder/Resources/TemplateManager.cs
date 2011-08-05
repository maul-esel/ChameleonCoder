using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources
{
    internal static class TemplateManager
    {
        static ConcurrentDictionary<Guid, ITemplate> templates = new ConcurrentDictionary<Guid, ITemplate>();

        public static void Add(Type type)
        {
            ITemplate template = Activator.CreateInstance(type) as ITemplate;

            if (template != null)
            {
                templates.TryAdd(template.Identifier, template);
            }
        }
    }
}
