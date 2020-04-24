using System.Collections.Generic;
using Webao.Attributes;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("https://api.nationalize.io")]
    [AddParameter("format", "json")]
    public interface WebaoDynCountry
    {
        [Get("?name={name}")]
        [Mapping(typeof(DtoCountrySearch), ".Country")]
        List<Country> GetNationality(string name);
    }
}
