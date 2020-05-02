using System;
using System.Collections.Generic;
using System.Text;

namespace WebaoBenchMark
{
    class WebaoBench
    {
        public static void Main()
        {
            ArtistBench.Run();
            BoredomBench.Run();
            CharacterBench.Run();
            CountryBench.Run();
            TrackBench.Run();

        }
    }
}
