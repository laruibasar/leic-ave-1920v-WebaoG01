using System.Collections.Generic;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    public interface IWebaoArtist3b
    {
        Artist GetInfo(string name);
        List<Artist> Search(string name, int page);
    }
}