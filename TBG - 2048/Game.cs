using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG___2048
{
    class Game
    {
        private int[][] Dimension = new int[4][];
        private int Score = 0;

        public Game()
        {

            Initiate(Dimension, 4);
            GiveNumbers(Dimension, new int[] { 2, 4 }, 2);

            Show();
            Console.Write("  Press the arrow key...");
        }

        private bool IsOver()
        {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (Dimension[i][j] == 0) return false;
                    if (j != 3 && Dimension[i][j] == Dimension[i][j+1]) return false;
                    if (i != 3 && Dimension[i][j] == Dimension[i + 1][j]) return false;
                }
            }

            return true;
        }

        private void Initiate(int[][] array, int val)
        {
            for (int i = 0; i < val; i++) {
                array[i] = new int[val];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="val"></param>
        /// <param name="number"></param>
        private void GiveNumbers(int[][] array, int[] vals, int number)
        {
            Random rand = new Random();
            int indexI = rand.Next(4);
            int indexJ = rand.Next(4);

            for (int i = 0; i < number; i++) {
                while (array[indexI][indexJ] != 0) {
                    indexI = rand.Next(4);
                    indexJ = rand.Next(4);
                }
                array[indexI][indexJ] = vals[rand.Next(vals.Length)];
            }

        }

        private void SumNumbers(int[] array, bool reversed)
        {
            if (!reversed) {
                for (int i = array.Length - 1; i > 0; i--) {
                    if (array[i] == array[i - 1]) {
                        int sum = array[i] + array[i - 1];
                        array[i] = sum;
                        array[i - 1] = 0;
                        Score += sum;
                    }
                }
            } else {
                for (int i = 0; i < array.Length - 1; i++) {
                    if (array[i] == array[i+1]) {
                        int sum = array[i] + array[i + 1];
                        array[i] = sum;
                        array[i + 1] = 0;
                        Score += sum;
                    }
                }
            }
        }

        private bool Compare(int[][] array1, int[][] array2)
        {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (array1[i][j] != array2[i][j])
                        return true;
                }
            }
            return false;
        }

        private int[] ShiftArray(int[] array)
        {
            var nonZeros = array.Where(x => x != 0);
            var amountofZeros = array.Count() - nonZeros.Count();
            return nonZeros.Concat(Enumerable.Repeat(0, amountofZeros)).ToArray();
        }
        
        private bool Move(string direction = "LEFT")
        {
            bool reversed = false;
            bool rotated = false;
            bool changeBehavior = false;
            if (direction == "LEFT") {
                // OK
            } else if (direction == "RIGHT") {
                Helper.ReverseJaggedArray(Dimension);
                reversed = true;
                changeBehavior = true;
            } else if (direction == "UP") {
                Dimension = Helper.Rotate(Dimension);
                Helper.ReverseJaggedArray(Dimension);
                reversed = true;
                rotated = true;
            } else if (direction == "DOWN") {
                Dimension = Helper.Rotate(Dimension);
                rotated = true;
                changeBehavior = true;
            }

            int[][] oldArray = Helper.CreateJaggedArray(4) ;
            Array.Copy(Dimension, oldArray, Dimension.Length);
            
            for (int i = 0; i < 4; i++) {
                Dimension[i] = Operate(Dimension[i], changeBehavior);
            }

            bool equal = Compare(oldArray, Dimension);
            if (equal) {
                GiveNumbers(Dimension, new int[] { 2 }, 1);
            }

            if (reversed) {
                Helper.ReverseJaggedArray(Dimension);
            }

            if (rotated) {
                Dimension = Helper.Rotate(Dimension);
                Dimension = Helper.Rotate(Dimension);
                Dimension = Helper.Rotate(Dimension);
            }

            return equal;
        }

        private int[] Operate(int[] row, bool reversed)
        {
            row = ShiftArray(row);
            SumNumbers(row, reversed);
            row = ShiftArray(row);
            return row;
        }

        /// <summary>
        /// Prints the table according to format.
        /// </summary>
        /// <param name="format">Default: {0,2}</param>
        private void Show(string format = "{0,6}")
        {
            bool status = IsOver();
            Console.Title = "2048 Game - Console Application | " + (status ? "No moves left" : $"Score : {Score}");
            Console.Clear();
            Console.WriteLine(" #================================#");
            Console.WriteLine("  2048 Game - Console Application");
            Console.WriteLine(" #--------------------------------#");
            Console.WriteLine();
            for (int i = 0; i < 4; i++) {
                Console.Write("{0,3}", "");
                for (int j = 0; j < 4; j++) {
                    Console.Write(format, Dimension[i][j] == 0 ? "*" : Dimension[i][j].ToString());
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine(" #--------------------------------#");
            Console.WriteLine((status ? "\tFinal Score: " : "\tScore: ") + $"{Score}");
            Console.WriteLine(" #================================#");
        }

        public void Play()
        {
            while(!IsOver()) {
                var input = Console.ReadKey(true).Key;
                switch (input) {
                    case ConsoleKey.LeftArrow:
                        if(Move("LEFT"))
                            Show();
                        break;
                    case ConsoleKey.UpArrow:
                        if(Move("UP"))
                            Show();
                        break;
                    case ConsoleKey.RightArrow:
                        if(Move("RIGHT"))
                            Show();
                        break;
                    case ConsoleKey.DownArrow:
                        if(Move("DOWN"))
                            Show();
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.R:
                        Dimension = Helper.Rotate(Dimension);
                        Show();
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
