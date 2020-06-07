using System;
using System.Collections.Generic;

namespace WebaoDynamic.TP3Fluent
{
    public class Info
    {
        public Type returnType { get; set; }
        public string url { get; set; }
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
        public List<InfoMethod> list = new List<InfoMethod>();

        public Info(Type type)
        {
            this.returnType = type;
        }

        public Delegate GetMethodDelegate(string name)
        {
            InfoMethod method = list.Find(search => search.name.Equals(name));
            return method.Del;
        }
    }

    public class InfoMethod
    {
        public string name { get; set; }
        public string query { get; set; }
        public Type methodReturnType { get; set; }
        public Delegate Del { get; set; }

        public InfoMethod(string name)
        {
            this.name = name;
        }

        public int GetNumberParameters()
        {
            return query.Split('{').Length - 1;
        }
    }
}
