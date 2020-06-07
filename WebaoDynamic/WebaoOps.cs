using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;
using Webao.Base;
using WebaoDynamic.TP3Fluent;

namespace WebaoDynamic
{
    public static class WebaoOps
    {
        /* To answer part 2 of TP3 we rearrange this class
         * to allow to set a current Context to use if and allow for 
         * only small changes on the overall solution
         */
        private static Context currentContext;

        public static void SetContext(Context context)
        {
            currentContext = context;
        }

        public static Delegate GetDelegate(string method)
        {
            InfoMethod im = currentContext.info.list.Find(infoMethod => infoMethod.name.Equals(method));
            return im.Del;
        }

        public static bool IsContextSet()
        {
            return currentContext != null;
        }

        public const BindingFlags ALL_INSTANCE =
            BindingFlags.Instance |
            BindingFlags.FlattenHierarchy |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        public static string GetUrl(Type type)
		{
            if (currentContext != null)
            {
                return currentContext.info.url;
            }
            else
            {
                TypeInformation typeInfo = TypeInfoCache.Get(type);
                BaseUrlAttribute url = (BaseUrlAttribute)typeInfo[typeof(BaseUrlAttribute).FullName][0];
                return url.host;
            }
		}

        public static Dictionary<string, string> GetParameters(Type type)
        {
            if (currentContext != null)
            {
                return currentContext.info.parameters;
            }
            else
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                TypeInformation typeInfo = TypeInfoCache.Get(type);

                List<Attribute> parametersAttributes = typeInfo[typeof(AddParameterAttribute).FullName];
                foreach (AddParameterAttribute p in parametersAttributes)
                {
                    parameters.Add(p.name, p.val);
                }
                return parameters;
            }
        }

        public static string GetQuery(Type type, string method)
        {
            if (currentContext != null)
            {
                InfoMethod im = currentContext.info.list.Find(infoMethod => infoMethod.name.Equals(method));
                return im.query;
            }
            else
            {
                TypeInformation typeInfo = TypeInfoCache.Get(type);
                GetAttribute get = (GetAttribute)typeInfo[method + typeof(GetAttribute).FullName][0];
                return get.path;
            }
        }

        public static string GetMappingDomain(Type type, string method)
        {
                MappingAttribute map = GetMapping(type, method);
                return map.path;
        }

        public static Type GetMappingType(Type type, string method)
        {
            if (currentContext != null)
            {
                InfoMethod im = currentContext.info.list.Find(infoMethod => infoMethod.name.Equals(method));
                return im.methodReturnType;
            }
            else
            {
                MappingAttribute map = GetMapping(type, method);
                return map.destType;
            }
        }

        public static string GetMappingWith(Type type, string method)
        {
            MappingAttribute map = GetMapping(type, method);
            return map.With;
        }

        private static MappingAttribute GetMapping(Type type, string method)
        {
            TypeInformation typeInfo = TypeInfoCache.Get(type);
            return (MappingAttribute)typeInfo[method + typeof(MappingAttribute).FullName][0];
        }

        public static MethodInfo[] GetMethods(Type type)
        {
            return type.GetMethods();
        }
    }
}
