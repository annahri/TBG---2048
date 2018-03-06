using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cons = Colorful.Console;
using System.IO;
using System.Diagnostics;


namespace TBG___2048
{
    partial class Game
    {
        /// <summary>
        /// Gets the values from the jagged array.
        /// </summary>
        /// <returns>List of values</returns>
        private List<int> GetValues()
        {
            var vals = new List<int>();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    var val = Dimension[i][j];
                    vals.Add(val);
                }
            }
            return vals;
        }

        /// <summary>
        /// Prints the heading
        /// </summary>
        /// <param name="type">False, print normal text heading.</param>
        /// <param name="status">Winning state</param>
        private void Title(bool type, bool status, string str)
        {
            if (!type) {
                Console.Title = "2048 Game - Console Application | " + (status ? "No moves left" : $"Score : {Score}");
                Console.Clear();
                Console.WriteLine(" #================================#");
                Console.WriteLine(str);
                Console.WriteLine(" #--------------------------------#");
                Console.WriteLine();
            } else {
                Console.Title = "2048 Game - Console Application | " + (status ? "No moves left" : $"Score : {Score}");
                Console.Clear();
                Cons.ResetColor();

                // Heading
                Cons.WriteAscii(str, Color.DarkOrange);
            }
        }

        private void Footer(bool type, bool status)
        {
            if (!type) {
                Console.WriteLine(" #--------------------------------#");
                Console.WriteLine((status ? "\tFinal Score: " : "\tScore: ") + $"{Score}");
                Console.WriteLine(" #================================#");
            } else {

            }
        }

        public void MainMenu()
        {
            Cons.WriteAscii(" 2048", Color.DarkOrange);
            Cons.WriteLine("   A clone of 2048 on a console application.", Color.DarkOrange);
            Cons.ResetColor();

            Menu();
        }

        private void Menu()
        {
            var mode = false;
            var choices = new string[] {
                "Play",
                "Highscores",
                "Quit"
            };

            for (int i = 0; i < choices.Length; i++) {
                Cons.Write($"{i + 1,5}. ", Color.DarkRed);
                Cons.WriteLine(choices[i], Color.DarkOrange);
            }
            Console.WriteLine();
            choice:
            Cons.Write("   Select your choice: ", Color.DarkOrange);
            var input = Cons.ReadLine();
            var output = 0;
            if (int.TryParse(input, out output)) {
                if (output > choices.Length || output == 0) {
                    Cons.WriteLine("   Oops, there's no such option.", Color.DarkOrange);
                    Console.ResetColor();
                    goto choice;
                } else {
                    Console.ResetColor();
                    switch (output) {
                        case 1:
                            Play();
                            return;
                        case 2:
                            HighScore();
                            return;
                        case 3:
                            Exit();
                            return;
                    }
                }
            } else {
                Cons.WriteLine("   Oops, numbers only.", Color.DarkOrange);
                goto choice;
            }
        }

        private void HighScore()
        {
            if (!File.Exists("highscores.xml")) {
                try {
                    InitHSFile();
                } catch (Exception) {
                    Cons.WriteLine("   Cannot initialize highscore.xml");
                    Cons.ReadLine();
                    Environment.Exit(0);
                }
            }

            Cons.Clear();
            Cons.WriteAscii(" Highscores", Color.DarkOrange);
            Cons.ResetColor();

            if (Highscores.Count > 0) {
                if (Highscores.Count > 5) {
                    var num = Highscores.Count - 5;
                    Highscores = Highscores.OrderByDescending(o => o.Score).ToList();
                    Highscores.RemoveRange(5, num);
                }

                var formatter = "{0,5} {1,5} {2,10} {3,10} {4,10}";
                var header = new string[] {
                    "No",
                    "Score",
                    "Moves",
                    "Elapsed",
                    "Date"
                };
                var ordered = Highscores.OrderByDescending(o => o.Score).ToArray();
                Console.WriteLine();
                Cons.WriteLine(formatter, Color.DarkRed, header);
                for (int i = 0; i < ordered.Length; i++) {
                    var values = new string[] {
                        (i+1).ToString()+'.',
                        ordered[i].Score.ToString(),
                        ordered[i].Moves.ToString(),
                        ordered[i].Elapsed,
                        ordered[i].Time
                    };
                    Cons.WriteLine(formatter, Color.DarkOrange, values);
                }
            } else {
                Cons.WriteLine("   No highscore", Color.Orange);
            }
            Cons.Write("   ");
            Cons.ReadKey();
            Cons.Clear();
            MainMenu();
        }

        /// <summary>
        /// Prints the table according to format.
        /// </summary>
        /// <param name="format">Default: {0,2}</param>
        private void Show(string format = "{0,6}")
        {
            bool status = IsOver();
            if (status) {
                Console.BackgroundColor = ConsoleColor.DarkRed;
            }
            Title(false, status, "  2048 Game - Console Application");
            for (int i = 0; i < 4; i++) {
                Console.Write("{0,3}", "");
                for (int j = 0; j < 4; j++) {
                    Console.Write(format, Dimension[i][j] == 0 ? "*" : Dimension[i][j].ToString());
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Footer(false, status);
        }

        /// <summary>
        /// Show the game with borders and colors.
        /// </summary>
        /// <param name="pad">Left padding</param>
        private void ShowBoxed(int pad = 3)
        {
            var gap = "";
            gap = gap.PadRight(pad, ' ');

            var len = 7;
            var status = IsOver();
            var hs = Highscores.Max(s => s.Score);


            Title(true, status, " 2048");

            Cons.WriteFormatted("   Score {0}", Color.DarkCyan, Color.DarkOrange, Score);
            Cons.WriteFormatted("\tHighscore {0}", Color.DarkCyan, Color.DarkOrange, hs);
            Console.WriteLine();

            #region Draw boxes
            Cons.Write(gap);
            Cons.Write("┌");
            for (int i = 0; i < 4 * len; i++) {
                if (i == 0) continue;
                if (i % len != 0) Cons.Write("─");
                else Cons.Write("┬");
            }
            Cons.Write("┐\n");

            for (int i = 0; i < 4; i++) {
                Cons.Write(gap);
                for (int j = 0; j < 4; j++) {
                    var separator = "│";
                    var str = "";

                    var blanks = len - 1;
                    var value = Dimension[i][j];
                    var digits = value.ToString().Length;
                    var spaces = blanks - digits;

                    if (value != 0)
                        str = str.PadRight(spaces - 1, ' ') + value + ' ';
                    else
                        str = str.PadRight(blanks, ' ');


                    Cons.Write(separator);
                    Cons.ForegroundColor = Color.White;
                    if (value != 0) {
                        var r = value % 255;
                        var g = (120 + r) % 255;
                        if (g >= 200) Cons.ForegroundColor = Color.Black;
                        Cons.BackgroundColor = Color.FromArgb(r, g, 50);
                    }
                    Cons.Write(str);
                    Cons.ResetColor();

                    if (j == 3) Cons.Write(separator);
                }

                Console.WriteLine();
                Cons.Write(gap);
                if (i != 3) {
                    for (int k = 0; k < 4 * len; k++) {
                        if (k == 0) Cons.Write("├");

                        if (k % len != 0) Cons.Write("─");
                        else if (k != 0) Cons.Write("┼");

                        if (k == 4 * len - 1) Cons.Write("┤");
                    }
                    Console.WriteLine();
                }
            }

            Cons.Write("└");
            for (int i = 0; i < 4 * len; i++) {
                if (i == 0) continue;
                if (i % len != 0) Cons.Write("─");
                else Cons.Write("┴");
            }
            Cons.Write("┘\n");
            #endregion

        }



        /// <summary>
        /// Play the game.
        /// </summary>
        public void Play()
        {
            Initialize();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ShowBoxed();
            var GameOver = IsOver();
            while (!GameOver) {
                var input = Console.ReadKey(true).Key;
                switch (input) {
                    case ConsoleKey.LeftArrow:
                        if (Move("LEFT")) ShowBoxed();
                        break;
                    case ConsoleKey.UpArrow:
                        if (Move("UP")) ShowBoxed();
                        break;
                    case ConsoleKey.RightArrow:
                        if (Move("RIGHT")) ShowBoxed();
                        break;
                    case ConsoleKey.DownArrow:
                        if (Move("DOWN")) ShowBoxed();
                        break;
                    // For debugging purposes
                    // case ConsoleKey.R:
                    //    Dimension = Helper.Rotate(Dimension);
                    //    Show();
                    //    break;
                }
                Moves += 1;
                GameOver = IsOver();
            }
            stopwatch.Stop();
            Cons.Beep(1500, 2000);
            
            // Game is over, store the highscore
            var time = DateTime.Now.ToShortDateString();
            var fscore = new Highscore() {
                Score = this.Score,
                Moves = this.Moves,
                Time = time,
                Elapsed = stopwatch.Elapsed.Seconds.ToString()
            };

            if (AddHighscore(fscore)) {
                Cons.WriteLine($"   Your final score: {Score}");
            } else {
                Cons.WriteLine($"   The highscore is {Highscores[0].Score} and yours is {Score}. Try again.");
            }

            Console.ReadLine();
            Console.Clear();
            InitHSFile();
            MainMenu();
        }

        private int GetIndexHS(List<Highscore> list, int value)
        {
            return list.FindIndex(x => x.Score < value);
        }
    }
}
