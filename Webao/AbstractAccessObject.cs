using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Webao.Attributes;
using Webao.Base;

namespace Webao
{
    public abstract class AbstractAccessObject
    {
        private readonly IRequest req;
        private readonly char[] separator = new char[] { '.' };
        private readonly Regex rgx = new Regex(@"\{[a-zA-Z0-9]*\}");

        protected AbstractAccessObject(IRequest req)
        {
            this.req = req;
        }

        public object Request(params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            MethodInfo callSite = (MethodInfo)stackTrace.GetFrame(1).GetMethod();
            /*
             * The callsite is the caller method.
             *
             * 
             * When not implemented, use this
             * throw new NotImplementedException();
             */

            TypeInformation typeInfo = TypeInfoCache.Get(callSite.DeclaringType);
            /*
             * Obtain parameters of Get and Mapping
             */
            //GetAttribute get = (GetAttribute)Attribute.GetCustomAttribute(callSite, typeof(GetAttribute));
            GetAttribute get = (GetAttribute)typeInfo[callSite.Name + typeof(GetAttribute).FullName][0];
            string path = get.path;
            if (args.Length != 0)
            {
                foreach (object arg in args)
                {
                    path = rgx.Replace(path, arg.ToString());
                }
            }

            //MappingAttribute map = (MappingAttribute)Attribute.GetCustomAttribute(callSite, typeof(MappingAttribute));
            MappingAttribute map = (MappingAttribute)typeInfo[callSite.Name + typeof(MappingAttribute).FullName][0];
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            
            object obj = req.Get(path, map.destType);
            
            /*
             * Get object from properties from response
             */
            Type type = obj.GetType();
            PropertyInfo prop;

            /*
             * We use iteration and then trash objects as needed
             * as we dive deeper into the domain.
             * We rely on GC to clean lost references.
             */
            object newObj = new object();
            foreach (string domain in domains)
            {
                prop = type.GetProperty(domain);
                newObj = prop.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }

            return obj;
        }
    }
}
