using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    
    public class WebaoCharacterDummy : WebaoDyn, IWebaoCharacter
    {
        public WebaoCharacterDummy(IRequest req) : base(req)
        {
            base.SetUrl("https://anapioficeandfire.com/api/");
            base.SetParameter("format", "json");
        }

        public Character GetCharacter(int id)
        {
            string path = "characters/{id}";
            path = path.Replace("{id}", id.ToString());

            Character character = (Character)base.GetRequest(path, typeof(Character));

            return character;
        }

        public List<Character> GetList()
        {
            string path = "characters";

            DtoList dto = (DtoList)base.GetRequest(path, typeof(DtoList));

            return dto.Results.CharacterMatches.Character;
        }
    }
}
