using System;

namespace Webao.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MappingAttribute : Attribute
    {
        public string path;
        public readonly Type destType;

        public MappingAttribute(Type dest, string path)
        {
            this.path = path;
            this.destType = dest;
        }

        public MappingAttribute(Type dest)
        {
            this.destType = dest;
        }

        public string With { set { this.path = value; } }
    }
}
