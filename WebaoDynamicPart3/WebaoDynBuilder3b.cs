using System;
using Webao.Attributes;
using Webao.Base;

namespace WebaoDynamicPart3
{
    public class WebaoDynBuilder3b
    {
        public static Context context;
        public static Type type;

        public static Context For<T>(string baseUrl)
        {
            return new Context(typeof(T), baseUrl);
        }
    }
}
