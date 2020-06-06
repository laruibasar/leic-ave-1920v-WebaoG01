using System.Collections.Generic;
using Webao.Attributes;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("https://anapioficeandfire.com/api/")]
    [AddParameter("format", "json")]
    public interface IWebaoCharacter3A
    {
        [Get("characters/{id}")]
        [Mapping(typeof(Character), ".")]
        Character GetCharacter(int id);

        [Get("characters")]
        [Mapping(typeof(DtoList), With = "WebaoTestProject.Dto.GetCharacters")]
        List<Character> GetList();
    }  
}

