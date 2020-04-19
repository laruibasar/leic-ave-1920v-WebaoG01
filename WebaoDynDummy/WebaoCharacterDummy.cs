using System;
using System.Collections.Generic;
using System.Reflection;
using Webao;
using Webao.Attributes;
using Webao.Base;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    [AddParameter("format", "json")]
    public class WebaoCharacterDummy : WebaoDynCharacter
    {
        private readonly IRequest req;
        private readonly Dictionary<string, string> requestParameters;
        private readonly char[] separator = new char[] { '.' };


        public WebaoCharacterDummy(IRequest req)
        {
            this.req = req;

            req.BaseUrl(WebaoOps.GetUrl(typeof(WebaoDynCharacter)));
            requestParameters = WebaoOps.GetParameters(typeof(WebaoCharacterDummy));

            foreach (KeyValuePair<string, string> pair in requestParameters)
            {
                req.AddParameter(pair.Key, pair.Value);
            }
        }

        public Character GetCharacter(int id)
        {
            string path = WebaoOps.GetQuery(typeof(WebaoDynCharacter), "GetCharacter");
            path = path.Replace("{id}", id.ToString());

            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynCharacter), "GetCharacter");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            Type type = obj.GetType();
            PropertyInfo propInfo;

            object newObj = new object();
            foreach (string domain in domains)
            {
                propInfo = type.GetProperty(domain);
                newObj = propInfo.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            } 
            return (Character)obj;
        }

        public List<Character> GetList()
        {                       
            string path = WebaoOps.GetQuery(typeof(WebaoDynCharacter), "GetList");

            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynCharacter), "GetList");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            Type type = obj.GetType();
            PropertyInfo propInfo;

            object newObj = new object();
            foreach (string domain in domains)
            {
                propInfo = type.GetProperty(domain);
                newObj = propInfo.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }
            return (List<Character>)obj;
        }
    }
}
