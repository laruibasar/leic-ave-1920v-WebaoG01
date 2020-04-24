using System;
using System.Reflection;
using Webao;
using Webao.Attributes;

namespace WebaoDynamic
{
    public class WebaoDyn
    {
        public readonly IRequest req;

        public WebaoDyn(IRequest req)
        {
            this.req = req;
        }
    }
}
