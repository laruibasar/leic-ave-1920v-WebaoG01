using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;
using Webao.Base;

namespace Webao
{
    public class WebaoBuilder
    {
        public static AbstractAccessObject Build(Type webao, IRequest req) {
            /*
             * Load attributes for HTTP request setup
             *
             * We cached the type
             */
            TypeInformation typeInfo = TypeInfoCache.Get(webao);

            //BaseUrlAttribute url = webao.GetCustomAttribute<BaseUrlAttribute>(false);
            BaseUrlAttribute url = (BaseUrlAttribute)typeInfo[typeof(BaseUrlAttribute).FullName][0];
            req.BaseUrl(url.host);

            //AddParameterAttribute[] parameters = (AddParameterAttribute[])Attribute
            //        .GetCustomAttributes(webao, typeof(AddParameterAttribute));
            List<Attribute> parameters = typeInfo[typeof(AddParameterAttribute).FullName];
            foreach (AddParameterAttribute p in parameters)
            {
                req.AddParameter(p.name, p.val);
            }

            return (AbstractAccessObject) Activator.CreateInstance(webao, req);
        }
    }
}
