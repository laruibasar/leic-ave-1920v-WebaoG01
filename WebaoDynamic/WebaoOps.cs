using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;

namespace WebaoDynamic
{
    public static class WebaoOps
    {
        public const BindingFlags ALL_INSTANCE =
            BindingFlags.Instance |
            BindingFlags.FlattenHierarchy |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        public static string GetUrl(Type type)
		{
            BaseUrlAttribute url = (BaseUrlAttribute)Attribute.GetCustomAttribute(type, typeof(BaseUrlAttribute));
            return url.host;
		}

        public static Dictionary<string, string> GetParameters(Type type)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            AddParameterAttribute[] parametersAttributes = (AddParameterAttribute[])Attribute
                    .GetCustomAttributes(type, typeof(AddParameterAttribute));
            foreach (AddParameterAttribute p in parametersAttributes)
            {
                parameters.Add(p.name, p.val);
            }

            return parameters;
        }

        public static string GetQuery(Type type, string method)
        {
            MethodInfo methodInfo = type.GetMethod(method);
            GetAttribute get = (GetAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(GetAttribute));
            return get.path;
        }

        public static string GetMappingDomain(Type type, string method)
        {
            MethodInfo methodInfo = type.GetMethod(method);
            MappingAttribute map = (MappingAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MappingAttribute));
            return map.path;
        }

        public static Type GetMappingType(Type type, string method)
        {
            MethodInfo methodInfo = type.GetMethod(method);
            MappingAttribute map = (MappingAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MappingAttribute));
            return map.destType;
        }

        public static MethodInfo[] GetMethods(Type type)
        {
            return type.GetMethods();
        }
    }
}
