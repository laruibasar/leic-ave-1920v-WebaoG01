using Webao;
using Webao.Dto;
using WebaoDynamics.Interfaces;

namespace WebaoDynamics.Dummies
{
    public class WebaoBoredomDummy : WebaoDyn, IWebaoBoredom
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

            Boredom boredom = (Boredom)base.GetRequest(path, typeof(Boredom));

            return boredom;
        }
    }
}