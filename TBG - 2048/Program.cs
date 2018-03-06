using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG___2048
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.MainMenu();

            Console.ReadKey();
        }
    }
}
