/*
 *Description: Forecast
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:25:24
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDLotto
{
    public class Forecast
    {
        public static void GuessCurrentPeriodLotto()
        {
            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllCombinations(10, 3);
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            //data = new List<int[]>()
            //{
            //    //new int[] { 2, 4, 2 },
            //    new int[] { 0, 7, 5 },
            //};

            //上期开组三时使用
            //data = FilterHelper.FilterByGroupThreePreLotto(data);

            data = FilterHelper.FilterByDistance5(data, [1, 2, 3]);
            data = FilterHelper.FilterByPreLotto(data, [0, 1, 2]);

            //data = FilterHelper.FilterByHundreds012Path(data, [1]);
            data = FilterHelper.FilterByHundredsValue(data, [1, 4, 7]);
            //data = FilterHelper.FilterByTens012Path(data, [0, 1]);
            data = FilterHelper.FilterByTensValue(data, [0, 4, 9]);
            //data = FilterHelper.FilterByUnits012Path(data, [0, 1]);
            data = FilterHelper.FilterByUnitsValue(data, [2, 3, 5, 7, 8]);

            //data = FilterHelper.FilterByMax012Path(data, [0, 2]);
            //data = FilterHelper.FilterByMin012Path(data, [0, 1]);
            //data = FilterHelper.FilterByMiddle012Path(data, [1, 2]);
            Console.WriteLine();

            data = FilterHelper.FilterBySum(data);
            data = FilterHelper.FilterBySumMantissa(data, [0, 4, 6, 8, 9]);
            data = FilterHelper.FilterBySpan(data, [5, 6, 7, 8, 9]);
            //Console.WriteLine();
            //data = FilterHelper.FilterByOddEvenRate(data, [3D / 0, 1D / 2, 2D / 1]);

            data.ToList().ForEach(group =>
            {
                PrintHelper.PrintForecastResult(string.Join(" ", group));
            });
        }


        /// <summary>
        /// 从N个元素中选取K个元素进行的排列
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns>排列方式集合</returns>
        private static IList<int[]> GetAllCombinations(int n, int k)
        {
            List<int[]> combinations = new List<int[]>();
            int[] currentCombination = new int[k];
            RecursiveLoop(combinations, currentCombination, 0, n, k);
            return combinations;
        }

        private static void RecursiveLoop(List<int[]> combinations, int[] currentCombination, int start, int n, int k)
        {
            if (k == 0)
            {
                int[] newCombination = new int[currentCombination.Length];
                Array.Copy(currentCombination, newCombination, currentCombination.Length);
                combinations.Add(newCombination);
                return;
            }

            for (int i = start; i < n; i++)
            {
                currentCombination[currentCombination.Length - k] = Data.AllLotto[i];
                RecursiveLoop(combinations, currentCombination, 0, n, k - 1);
            }
        }
    }
}
