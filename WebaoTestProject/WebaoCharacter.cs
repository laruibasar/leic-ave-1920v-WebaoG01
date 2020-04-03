using System;
using System.Collections.Generic;
using Webao;
using Webao.Attributes;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [BaseUrl("https://anapioficeandfire.com/api/")]  
    [AddParameter("format", "json")]
    public class WebaoCharacter : AbstractAccessObject
    {
        public WebaoCharacter(IRequest req) : base(req) { }     

        [Get("characters/{id}")] 
        [Mapping(typeof(Character), ".")]  
        public Character GetCharacter(int id) => (Character)Request(id);

        [Get("characters")]
        [Mapping(typeof(DtoList), ".Results.CharacterMatches.Character")] 
        public List<Character> GetList() => (List<Character>)Request();
    }
}
