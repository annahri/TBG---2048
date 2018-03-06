using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG___2048
{
    [Serializable()]
    public class Highscore
    {
        public int Score { get; set; }
        public string Elapsed { get; set; }
        public string Time { get; set; }
        public int Moves { get; set; }
    }
}
