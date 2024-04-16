/*
 *Description: Common
 *Author: Chance.zheng
 *Creat Time: 2024/4/11 11:22:51
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto
{
    public class CommonHelper
    {
        /// <summary>
        /// 从N个元素中选取K个元素组合的总数量
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns>组合方式数量</returns>
        public static int CalculateCombinations(int n, int k)
        {
            int result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= n--;
                result /= i;
            }
            return result;
        }

        /// <summary>
        /// 从N个元素中选取K个元素进行的组合
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns> 组合方式集合</returns>
        public static List<int[]> GenerateCombinations(int n, int k)
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
                currentCombination[currentCombination.Length - k] = Data.AllRedLotto[i];
                RecursiveLoop(combinations, currentCombination, i + 1, n, k - 1);
            }
        }

        public static void GetSumOfFirstAndSixth()
        {
            var data = new List<int>();
            Get(data, 0, 1);
            data.Sort();

            var ss = data.MaxBy(n => data.Count(m => n == m));

            void Get(List<int> data, int n, int k)
            {
                if (n < Data.AllRedLotto.Length)
                {
                    if (k < Data.AllRedLotto.Length)
                    {
                        data.Add(Data.AllRedLotto[n] + Data.AllRedLotto[k]);
                        k++;
                    }
                    else
                    {
                        n++;
                        k = n + 1;
                    }
                    Get(data, n, k);
                }
                else
                {
                    return;
                }
            }
        }

        public static void GetSubtractOfFirstAndSixth()
        {
            var data = new List<int>();
            Get(data, 0, 1);
            data.Sort();

            var ss = data.MaxBy(n => data.Count(m => n == m));

            void Get(List<int> data, int n, int k)
            {
                if (n < Data.AllRedLotto.Length)
                {
                    if (k < Data.AllRedLotto.Length)
                    {
                        data.Add(Math.Abs(Data.AllRedLotto[n] - Data.AllRedLotto[k]));
                        k++;
                    }
                    else
                    {
                        n++;
                        k = n + 1;
                    }
                    Get(data, n, k);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
