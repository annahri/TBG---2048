using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBG___2048
{
    static class Helper
    {
        /// <summary>
        /// Reverse the jagged array horizontally.
        /// </summary>
        /// <param name="array">Desired array.</param>
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

        /// <summary>
        /// Transposes an array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Transposed array.</returns>
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

        /// <summary>
        /// Rotates an array 90 degrees.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[][] Rotate(int[][] array)
        {
            array = Transpose(array);
            ReverseJaggedArray(array);
            return array;
        }

        /// <summary>
        /// Creates a jagged array.
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int[][] CreateJaggedArray(int len)
        {
            int[][] array = new int[len][];
            for (int i = 0; i < len; i++) {
                array[i] = new int[len];
            }
            return array;
        }

        public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
