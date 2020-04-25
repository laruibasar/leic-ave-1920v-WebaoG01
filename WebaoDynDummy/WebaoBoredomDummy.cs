using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    public class WebaoBoredomDummy : WebaoDyn, WebaoDynBoredom
    {
        public WebaoBoredomDummy(IRequest req) : base(req)
        {
            base.SetUrl("https://www.boredapi.com/api/");
            base.SetParameter("format", "json");
        }

        public Boredom GetActivityByKey(int key)
        {
            string path = "activity?key={key}";
            path = path.Replace("{key}", key.ToString());

            Boredom boredom = (Boredom)base.GetRequest(path, typeof(Boredom));

            return boredom;
        }

        public Boredom GetActivity(int participants, float price)
        {
            string path = "activity?participants={participants}&price={price}";
            path = path.Replace("{participants}", participants.ToString());
            path = path.Replace("{price}", price.ToString());

            Type type = typeof(Boredom);
            Boredom boredom = (Boredom)base.GetRequest(path, type);

            return boredom;
        }
    }
}