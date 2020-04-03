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

        [Get("activity")]
        [Mapping(typeof(Boredom), ".")]
        public Boredom GetRandomActivity() => (Boredom)Request();

        [Get("activity?key={key}")]
        [Mapping(typeof(Boredom), ".")]
        public Boredom GetActivity(int key) => (Boredom)Request(key);

    }
}


