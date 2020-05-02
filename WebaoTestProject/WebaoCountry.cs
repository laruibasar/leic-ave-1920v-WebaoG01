using System;
using System.Collections.Generic;
using Webao;
using Webao.Attributes;
using Webao.Dto;

namespace WebaoTestProject
{
    [BaseUrl("https://api.nationalize.io")]
    [AddParameter("format", "json")]
    public class WebaoCountry : AbstractAccessObject
    {
        public WebaoCountry(IRequest req) : base(req) { }

        [Get("?name={name}")]
        [Mapping(typeof(DtoCountrySearch), ".Country")]
        public List<Country> GetNationality(string name) => (List<Country>)Request(name);
    }
}
