using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG___2048
{
    static class Helper
    {
        public static int GetColumnLength(int[][] jaggedArray, int columnIndex)
        {
            int count = 0;
            foreach (int[] row in jaggedArray) {
                if (columnIndex < row.Length) count++;
            }
            return count;
        }

        public static void ReverseJaggedArray(int[][] array)
        {
            for (int rowIndex = 0; rowIndex <= (array.GetUpperBound(0)); rowIndex++) {
                for (int colIndex = 0; colIndex <= (array[rowIndex].GetUpperBound(0) / 2); colIndex++) {
                    int tempHolder = array[rowIndex][colIndex];
                    array[rowIndex][colIndex] = array[rowIndex][array[rowIndex].GetUpperBound(0) - colIndex];
                    array[rowIndex][array[rowIndex].GetUpperBound(0) - colIndex] = tempHolder;
                }
            }
        }

        public static int[][] Transpose(int[][] array)
        {
            var newArray = CreateJaggedArray(4);
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    newArray[i][j] = array[j][i];
                }
            }
            return newArray;
        }

        public static int[][] Rotate(int[][] array)
        {
            array = Transpose(array);
            ReverseJaggedArray(array);
            return array;
        }

        public static int[][] CreateJaggedArray(int len)
        {
            int[][] array = new int[len][];
            for (int i = 0; i < len; i++) {
                array[i] = new int[len];
            }
            return array;
        }
    }
}
