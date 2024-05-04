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

namespace ThreeDLotto
{
    public class SelectHelper
    {
        /// <summary>
        /// 计算个十百位数
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CaculateHundredsTensUnits(bool isPrint = true)
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
        public static IList<int> CaculateDistance5(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var nums = new List<int>();

            nums.Add(preLotto[0]);
            nums.Add(preLotto[1]);
            nums.Add(preLotto[2]);
            nums.Add(preLotto[0] >= 5 ? preLotto[0] - 5 : preLotto[0] + 5);
            nums.Add(preLotto[1] >= 5 ? preLotto[1] - 5 : preLotto[1] + 5);
            nums.Add(preLotto[2] >= 5 ? preLotto[2] - 5 : preLotto[2] + 5);

            if (isPrint)
                PrintHelper.Print("差5定胆计算法：", nums);

            return nums;
        }

        /// <summary>
        /// 十位杀胆号法
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static int CaculateExcludeNumberByTens(bool isPrint = true)
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
        public static IList<int> CaculateIncludeNumberByUnits(bool isPrint = true)
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
        public static IList<int> CaculateIncludeNumberByTens(bool isPrint = true)
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
        public static IList<int> CaculateIncludeNumberByHundreds(bool isPrint = true)
        {
            var preLotto = Data.PreLotto.ToArray();
            var include = new List<int>();

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
        public static int CaculateMiddleNumber(int[] data)
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
    }
}
