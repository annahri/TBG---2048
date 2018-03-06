using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static HelperClass.Helper;

namespace TBG___2048
{
    partial class Game
    {
		private void Exit()
        {
            InitHSFile();

            Environment.Exit(0);
        }

		private void InitHSFile()
        {
            var serializer = new XmlSerializer(Highscores.GetType(), "Highscores.Scores");
            using (var writer = new StreamWriter("highscores.xml", false)) {
                serializer.Serialize(writer.BaseStream, Highscores);
            }
        }

		private void LoadHSFile()
        {
            var serializer = new XmlSerializer(Highscores.GetType(), "Highscores.Scores");
            object obj;
			using (var reader = new StreamReader("highscores.xml")) {
                obj = serializer.Deserialize(reader.BaseStream);
            }
            Highscores = (List<Highscore>)obj;
        }

		private bool AddHighscore(Highscore hs)
        {
            var count = Highscores.Count(x => x != null);
            var score = hs.Score;
            var index = GetIndexHS(Highscores, score);

			if (index >= Highscores.Count) {
                return false;
            }

            Highscores.Add(hs);
            return true;
        }

    }
}
