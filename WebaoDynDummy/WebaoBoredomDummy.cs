using System;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    public class WebaoBoredomDummy : WebaoDynBoredom
    {
        private readonly IRequest req;
        public WebaoBoredomDummy(IRequest req)
        {
            this.req = req;
            req.BaseUrl("https://www.boredapi.com/api/");
            req.AddParameter("format", "json");
        }

        public Boredom GetActivityByKey(int key)
        {
            string path = "activity?key={key}";
            path = path.Replace("{key}", key.ToString());

            Type type = typeof(Boredom);
            Boredom boredom = (Boredom)req.Get(path, type);

            return boredom;
        }

        public Boredom GetActivity(int participants, float price)
        {
            string path = "activity?participants={participants}&price={price}";
            path = path.Replace("{participants}", participants.ToString());
            path = path.Replace("{price}", price.ToString());

            Type type = typeof(Boredom);
            Boredom boredom = (Boredom)req.Get(path, type);

            return boredom;
        }
    }
}