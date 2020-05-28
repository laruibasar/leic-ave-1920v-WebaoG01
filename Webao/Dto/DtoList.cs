using System.Collections.Generic;

namespace Webao.Dto
{ 
    public class DtoList
    {
        public DtoListResults Results { get; set; }

        public List<Character> GetListCharacters()
        {
            return this.Results.CharacterMatches.Character;
        }
    }

    public class DtoListResults
    {
        public DtoCharacterMatches CharacterMatches { get; set; }
    }

    public class DtoCharacterMatches
    {
        public List<Character> Character { get; set; }
    }
}