/*
 *Description: SelectHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 11:04:51
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Microsoft.VisualBasic;

namespace ThreeDLotto
{
    public class SelectHelper
    {
        /// <summary>
        /// 计算个十百位数
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateHundredsTensUnits(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var nums = new List<int>();

            nums.Add(preLotto[0]);
            nums.Add(preLotto[1]);
            nums.Add(preLotto[2]);

            if (isPrint)
                PrintHelper.Print("个十百位数：", nums);

            return nums;
        }

        /// <summary>
        /// 差5定胆计算法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateDistance5(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var nums = new List<int>();

            nums.Add(preLotto[0]);
            nums.Add(preLotto[1]);
            nums.Add(preLotto[2]);
            nums.Add(preLotto[0] >= 5 ? preLotto[0] - 5 : preLotto[0] + 5);
            nums.Add(preLotto[1] >= 5 ? preLotto[1] - 5 : preLotto[1] + 5);
            nums.Add(preLotto[2] >= 5 ? preLotto[2] - 5 : preLotto[2] + 5);

            if (isPrint) PrintHelper.Print("差5定胆计算法：", nums);

            return nums;
        }

        /// <summary>
        /// 十位杀胆号法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static int CalculateExcludeNumberByTens(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var exclude = 0;

            _ = preLotto[1] switch
            {
                0 => exclude = 0,
                1 => exclude = 8,
                2 => exclude = 6,
                3 => exclude = 4,
                4 => exclude = 7,
                5 => exclude = 9,
                6 => exclude = 1,
                7 => exclude = 2,
                8 => exclude = 4,
                9 => exclude = 6,
                _ => throw new NotImplementedException(),
            };

            if (isPrint)
                PrintHelper.Print(string.Format("十位杀胆号法：{0}", exclude));

            return exclude;
        }

        /// <summary>
        /// 个位计算胆号法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateIncludeNumberByUnits(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var include = new List<int>();

            _ = preLotto[2] switch
            {
                0 => include = new List<int>(),
                1 => include = new List<int>() { 1, 0, 2, 9 }, //出1个
                2 => include = new List<int>() { 8, 3, 1 }, //出1-3个
                3 => include = new List<int>(),
                4 => include = new List<int>(),
                5 => include = new List<int>(),
                6 => include = new List<int>(),
                7 => include = new List<int>(),
                8 => include = new List<int>() { 8, 4, 6 },
                9 => include = new List<int>() { 8, 4, 0, 3 },
                _ => throw new NotImplementedException(),
            };

            if (isPrint)
                PrintHelper.Print("个位计算胆号法：", include);

            return include;
        }

        /// <summary>
        /// 十位计算胆号法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateIncludeNumberByTens(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var include = new List<int>();

            _ = preLotto[1] switch
            {
                0 => include = new List<int>(),
                1 => include = new List<int>(),
                2 => include = new List<int>(),
                3 => include = new List<int>(),
                4 => include = new List<int>(),
                5 => include = new List<int>(),
                6 => include = new List<int>() { 7, 3, 4 }, //出1-3个
                7 => include = new List<int>(),
                8 => include = new List<int>(),
                9 => include = new List<int>(),
                _ => throw new NotImplementedException(),
            };

            if (isPrint)
                PrintHelper.Print("十位计算胆号法：", include);

            return include;
        }

        /// <summary>
        /// 百位计算胆号法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateIncludeNumberByHundreds(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            List<int>? include;
            _ = preLotto[0] switch
            {
                0 => include = new List<int>() { 4, 9, 3 }, //出1-3个
                1 => include = new List<int>(),
                2 => include = new List<int>(),
                3 => include = new List<int>(),
                4 => include = new List<int>(),
                5 => include = new List<int>(),
                6 => include = new List<int>() { 9, 7, 4 }, //出1-2个
                7 => include = new List<int>(),
                8 => include = new List<int>() { 1, 0, 9 }, //出1-3个
                9 => include = new List<int>(),
                _ => throw new NotImplementedException(),
            };

            if (isPrint)
                PrintHelper.Print("百位计算胆号法：", include!);

            return include!;
        }

        /// <summary>
        /// 计算中间号码
        /// </summary>
        /// <returns></returns>
        public static int CalculateMiddleNumber(int[] data)
        {
            var max = data.Max();
            var min = data.Min();

            var maxCount = data.Count(n => n == max);
            var minCount = data.Count(n => n == min);

            if (maxCount > minCount)
            {
                return max;
            }
            else if (maxCount < minCount)
            {
                return min;
            }
            else
            {
                if ((maxCount = minCount) == 3)
                    return data.First();
                else
                    return data.Where(n => n != max && n != min).First();
            }
        }


        /// <summary>
        /// 上期开奖号平方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> CalculateIncludeNumbers(int[] data)
        {
            var number = data[0] * 100 + data[1] * 10 + data[2] * 1;
            var power = number * number;
            var new_number = power % 1000;
            var hundred = new_number / 100;
            var ten = (new_number % 100) / 10;
            var unit = new_number % 10;
            var arr = new int[3] { hundred, ten, unit };

            var all_data = Common.GetAllCombinations(10, 3);

            all_data = all_data.Where(g => g.Sum() % 10 != unit).ToList();
            //all_data = all_data.Where(g => g.Max() - g.Min() != unit).ToList();
            //all_data = all_data.Where(g => g.Sum() != hundred + ten + unit).ToList();
            //all_data = all_data.Where(g => g.Max() - g.Min() != arr.Max() - arr.Min()).ToList();

            return all_data;
        }

        public static IList<int> CalculateRelationNumbers(int[] data)
        {
            return new List<int>() { Get(data[0]), Get(data[1]), Get(data[2]) };

            int Get(int n) => n switch
            {
                0 => 8,
                1 => 5,
                2 => 2,
                3 => 7,
                4 => 4,
                5 => 1,
                6 => 9,
                7 => 6,
                8 => 3,
                9 => 0,
                _ => throw new NotImplementedException(),
            };
        }

        public static IList<int> CalculateSpanSumMantissaNumbers(int[] data)
        {
            var number1 = Data.PreLotto.Sum() % 10;
            var number2 = Data.PreLotto.Max() - Data.PreLotto.Min();

            return new List<int>() { number1, number2 };
        }

        public static IList<int> CalculateProbableNumbers(int[] data)
        {
            var probableNumbers = new List<int>();
            probableNumbers.Add(data.Sum() % 10 + 1);
            var nums = data.OrderByDescending(n => n);
            probableNumbers.Add(Math.Abs(Math.Abs(nums.ElementAt(0) - nums.ElementAt(1)) - nums.ElementAt(2)));
            probableNumbers.Add(Math.Abs(data[0] - data[2]));

            return probableNumbers;
        }
    }
}
