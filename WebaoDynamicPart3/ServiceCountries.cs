using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamicPart3
{
    class ServiceCountries : AbstractAccessObject
    {
        private readonly WebaoCountry webao;
        public ServiceCountries() : this(new HttpRequest())
        {
        }
        public ServiceCountries(IRequest req) : base(req) { }
        public IEnumerable<Country> GetNationality(string name)
        {
            List<Country> list = (List<Country>)Request(name);
            foreach (Country item in list){
                yield return item;
            }
             
        }
    }
}
