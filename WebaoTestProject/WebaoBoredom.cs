using System;
using Webao;
using Webao.Attributes;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [BaseUrl("https://www.boredapi.com/api/")]
    [AddParameter("format", "json")]
    public class WebaoBoredom : AbstractAccessObject
    {
        public WebaoBoredom(IRequest req) : base(req) { }

        [Get("activity?key={key}")]
        [Mapping(typeof(Boredom), ".")]
        public Boredom GetActivityByKey(int key) => (Boredom)Request(key);

        [Get("activity?participants={participants}&price={price}")]
        [Mapping(typeof(Boredom), ".")]
        public Boredom GetActivity(int participants, float price) => (Boredom)Request(participants, price);
    }
}


