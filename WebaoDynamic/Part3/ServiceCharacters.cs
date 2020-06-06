using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamic.Part3
{
    class ServiceCharacters : AbstractAccessObject
    {
        private readonly WebaoCharacter webao;
        public ServiceCharacters() : this(new HttpRequest())
        {
        }
        public ServiceCharacters(IRequest req) :base(req) { }
        public IEnumerable<Character> GetList()
        {
            List<Character> list = (List<Character>)Request();
            foreach (Character item in list)
            {
                yield return item;
            }
        }
    }
}
