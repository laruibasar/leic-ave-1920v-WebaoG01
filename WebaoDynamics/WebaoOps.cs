﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;
using Webao.Base;

namespace WebaoDynamics
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
            TypeInformation typeInfo = TypeInfoCache.Get(type);
            BaseUrlAttribute url = (BaseUrlAttribute)typeInfo[typeof(BaseUrlAttribute).FullName][0];
            return url.host;
		}

        public static Dictionary<string, string> GetParameters(Type type)
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

        public static string GetQuery(Type type, string method)
        {
            //MethodInfo methodInfo = type.GetMethod(method);
            TypeInformation typeInfo = TypeInfoCache.Get(type);
            GetAttribute get = (GetAttribute)typeInfo[method + typeof(GetAttribute).FullName][0];
            //GetAttribute get = (GetAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(GetAttribute));
            return get.path;
        }

        public static string GetMappingDomain(Type type, string method)
        {
            //MethodInfo methodInfo = type.GetMethod(method);
            TypeInformation typeInfo = TypeInfoCache.Get(type);
            MappingAttribute map = (MappingAttribute)typeInfo[method + typeof(MappingAttribute).FullName][0];
            //MappingAttribute map = (MappingAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MappingAttribute));
            return map.path;
        }

        public static Type GetMappingType(Type type, string method)
        {
            //MethodInfo methodInfo = type.GetMethod(method);
            TypeInformation typeInfo = TypeInfoCache.Get(type);
            MappingAttribute map = (MappingAttribute)typeInfo[method + typeof(MappingAttribute).FullName][0];
            //MappingAttribute map = (MappingAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MappingAttribute));
            return map.destType;
        }

        public static MethodInfo[] GetMethods(Type type)
        {
            return type.GetMethods();
        }
    }
}
