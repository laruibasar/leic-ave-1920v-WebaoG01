using System;
using Webao;
using WebaoDynDummy;

namespace WebaoDynamic
{
    public class WebaoDynBuilder
    {
        public static object Build(Type type, IRequest req)
        {
            return new WebaoBoredomDummy(req);
        }
    }
}
