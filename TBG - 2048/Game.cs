using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using static HelperClass.Helper;
using Cons = Colorful.Console;

namespace TBG___2048
{
    partial class Game
    {
        private List<Highscore> Highscores = new List<Highscore>(5);
        private int[][] Dimension = new int[4][];
        private int Score = 0;
        private int Moves = 0;

        public Game()
        {
            Initialize();
            if (File.Exists("highscores.xml")) LoadHSFile();
            else InitHSFile();
            Console.Title = "2048 Game - Console Application";
        }

        private void Initialize()
        {
            Initiate(Dimension, 4);
            GiveNumbers(Dimension, new int[] { 2, 4 }, 2);
            Score = 0;
            Moves = 0;
        }

        /// <summary>
        /// Check if the game is over.
        /// </summary>
        /// <returns>If no moves left, returns true, otherwise false.</returns>
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

        /// <summary>
        /// Find the sum in a array.
        /// Counts from left to right / right to left based on 
        /// </summary>
        /// <param name="array">Desired array.</param>
        /// <param name="istrue">If true, counts from left to right and vice versa.</param>
        private void SumNumbers(int[] array, bool istrue)
        {
            if (!istrue) {
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

        
        /// <summary>
        /// Move the numbers based on keypress.
        /// </summary>
        /// <param name="direction">LEFT, UP, RIGHT, DOWN</param>
        /// <returns>Returns true if it moves.</returns>
        private bool Move(string direction = "LEFT")
        {
            bool reversed = false;
            bool rotated = false;
            bool changeBehavior = false;
            if (direction == "LEFT") {
                // OK
            } else if (direction == "RIGHT") {
                ReverseJaggedArray(Dimension);
                reversed = true;
                changeBehavior = true;
            } else if (direction == "UP") {
                Dimension = Rotate(Dimension);
                ReverseJaggedArray(Dimension);
                reversed = true;
                rotated = true;
            } else if (direction == "DOWN") {
                Dimension = Rotate(Dimension);
                rotated = true;
                changeBehavior = true;
            }

            int[][] oldArray = CreateJaggedArray(4) ;
            Array.Copy(Dimension, oldArray, Dimension.Length);
            
            for (int i = 0; i < 4; i++) {
                Dimension[i] = Operate(Dimension[i], changeBehavior);
            }

            bool equal = Compare(oldArray, Dimension);
            if (equal) GiveNumbers(Dimension, new int[] { 2 }, 1);

            if (reversed) ReverseJaggedArray(Dimension);

            if (rotated) {
                Dimension = Rotate(Dimension);
                Dimension = Rotate(Dimension);
                Dimension = Rotate(Dimension);
            }

            return equal;
        }

        /// <summary>
        /// Wraps the summing and the shifting.
        /// </summary>
        /// <param name="row">Array</param>
        /// <param name="reversed"></param>
        /// <returns>New array</returns>
        private int[] Operate(int[] row, bool reversed)
        {
            row = ShiftArray(row);
            SumNumbers(row, reversed);
            row = ShiftArray(row);
            return row;
        }

    }
}
