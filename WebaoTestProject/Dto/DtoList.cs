using System.Collections.Generic;

namespace WebaoTestProject.Dto
{ 
    public class DtoList
    {
        //public List<Character> Character { get; set; }
        public DtoListResults Results { get; set; }
        public List<Character> GetCharacters()
        { return this.Results.CharacterMatches.Character;  }
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