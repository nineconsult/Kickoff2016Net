using System.Collections.Generic;
using System.Reflection;

namespace Abp.EntityFramework.Utils
{
    internal class EntityDateTimePropertiesInfo
    {
        public EntityDateTimePropertiesInfo()
        {
            DateTimePropertyInfos = new List<PropertyInfo>();
            ComplexTypePropertyPaths = new List<string>();
        }

        public List<PropertyInfo> DateTimePropertyInfos { get; set; }

        public List<string> ComplexTypePropertyPaths { get; set; }
    }
}