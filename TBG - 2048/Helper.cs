using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClass
{
    public static class Helper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="val"></param>
        public static void Initiate(int[][] array, int val)
        {
            for (int i = 0; i < val; i++) {
                array[i] = new int[val];
            }
        }

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
        /// Shift the elements of array to the left.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] ShiftArray(int[] array)
        {
            var nonZeros = array.Where(x => x != 0);
            var amountofZeros = array.Count() - nonZeros.Count();
            return nonZeros.Concat(Enumerable.Repeat(0, amountofZeros)).ToArray();
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
        /// Compares 
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool Compare(int[][] array1, int[][] array2)
        {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (array1[i][j] != array2[i][j])
                        return true;
                }
            }
            return false;
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
