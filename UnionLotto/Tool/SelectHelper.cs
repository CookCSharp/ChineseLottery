/*
 *Description: SelectHelper
 *Author: Chance.zheng
 *Creat Time: 2024/4/8 16:10:31
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto
{
    /// <summary>
    /// 根据各个模型选出满足条件的号
    /// </summary>
    public class SelectHelper
    {
        /// <summary>
        /// 红球：两两加减计算法。
        /// <para>命中0、1、6个的概率很低，命中2、5个概率其次，命中3、4个概率较大</para>
        /// <para>命中概率排序，以1000组数据来看，4=3>5=2>1>6>0，命中概率分别为：32%、28%、18%、12%、6%、3%、1%</para>
        /// </summary>
        /// <remarks>
        /// 计算得到的是较大范围的一组数据
        /// </remarks>
        public static IList<int> CalculatePlusAndSubtractLotto(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var forecastNums = new HashSet<int>();
            for (int i = 0; i < preRedLotto.Length; i++)
            {
                for (int j = i + 1; j < preRedLotto.Length; j++)
                {
                    Add(preRedLotto[i], preRedLotto[j]);
                }
            }

            void Add(int a, int b)
            {
                if (Math.Abs(a - b) >= 1 && Math.Abs(a - b) <= 33)
                {
                    forecastNums.Add(Math.Abs(a - b));
                }

                if (a + b >= 1 && a + b <= 33)
                {
                    forecastNums.Add(a + b);
                }
            }

            var nums = forecastNums.ToList();
            nums.Sort();

            if (isPrint)
                PrintHelper.Print("两两相互加减（[]个)，命中0、1、6个的概率很低，命中2、5个概率其次，命中3、4个概率较大", nums);

            return nums;
        }

        /// <summary>
        /// 红球：和值取商计算法。和值减去每个号码，再除以对应号码，得商，取商的同尾数。
        /// <para>命中0、5、6个的概率很低，命中1个概率其次，命中2、3、4个概率较大</para>
        /// <para>命中概率排序，以1000组数据来看，3>2>4>1>5>0>6，命中概率分别为：30%、26%、22%、12%、7%、2%、1%</para>
        /// </summary>
        /// <remarks>
        /// 计算得到的是较大范围的一组数据
        /// </remarks>
        public static IList<int> CalculateSumDivisionLotto(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var forecastNums = new List<int>();
            var sum = preRedLotto.Sum();
            for (int i = 0; i < preRedLotto.Length; i++)
            {
                //个位数
                var temp = (sum - preRedLotto[i]) / preRedLotto[i] % 10;
                if (temp <= 0) temp += 10;
                while (temp >= 1 && temp <= 33)
                {
                    forecastNums.Add(temp);
                    temp += 10;
                }
            }
            var nums = forecastNums.Distinct().ToList();
            nums.Sort();

            if (isPrint)
                PrintHelper.Print("和值计算法([]个)，命中0、5、6个的概率很低，命中1个概率其次，命中2、3、4个概率较大", nums);

            return nums;
        }


        /// <summary>
        /// 红球：9进制与11进制计算法。
        /// <para>命中4、5、6个的概率几乎为0，命中0、3个概率其次，命中1、2个概率较大</para>
        /// <para>9进制命中概率排序，以1000组数据来看，1=2>0=3>4=5=6，命中概率分别为：33%、35%、15%、15%、和为2%</para>
        /// <para>11进制命中概率排序，以1000组数据来看，1=2>0=3>4=5=6，命中概率分别为：38%、30%、19%、12%、和为1%</para>
        /// 9，11进制数中斜连号99%是下期胆号(我不知道如何判断，尽量不采用吧)
        /// </summary>
        /// <returns></returns>
        public static IList<int> Calculate9And11Lotto(int n = 9, bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var noveary = Get(9);
            if (isPrint)
                PrintHelper.Print("9进制([]个)，一般出1-2个：", noveary);

            var undecimal = Get(11);
            if (isPrint)
                PrintHelper.Print("11进制([]个)，一般出1-2个：", undecimal);

            IList<int> Get(int except)
            {
                var forecastNums = new List<int>();
                for (int i = 0; i < preRedLotto.Length; i++)
                {
                    if (preRedLotto[i] + except >= 1 && preRedLotto[i] + except <= 33)
                        forecastNums.Add(preRedLotto[i] + except);

                    if (preRedLotto[i] - except >= 1 && preRedLotto[i] - except <= 33)
                        forecastNums.Add(preRedLotto[i] - except);
                }
                var nums = forecastNums.Distinct().ToList();
                nums.Sort();

                return nums;
            }

            return n == 9 ? noveary : undecimal;
        }




        /************** 以下是定胆方法*************/


        /// <summary>
        /// 红球：尾数定胆法。
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateProbableMantissa(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var mantissaNums = new List<int>
            {
                (preRedLotto[2] + 4) % 10,
                (preRedLotto[2] + 4 + 3) % 10
            };

            if (isPrint)
                PrintHelper.Print("尾数定胆([]个，下期尾数)，不是很准：", mantissaNums);

            return mantissaNums;
        }

        /// <summary>
        /// 红球：中间数定胆。
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalulateProbableMiddle(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var middleNums = new List<int>();
            for (int i = 0; i < preRedLotto.Length - 1; i++)
            {
                var diffValue = preRedLotto[i + 1] - preRedLotto[i];
                if (diffValue > 1)
                {
                    if ((diffValue & 1) == 0)
                    {
                        middleNums.Add(preRedLotto[i] + diffValue / 2);
                    }
                    else
                    {
                        middleNums.Add(preRedLotto[i] + diffValue / 2);
                        middleNums.Add(preRedLotto[i] + diffValue / 2 + 1);
                    }
                }
            }
            if (isPrint)
                PrintHelper.Print("中间数定胆([]个)，不是很准：", middleNums);

            return middleNums;
        }

        /// <summary>
        /// 红球：黄金分割定胆，0.618。
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalulateProbableGoldedCut(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var goldNums = new List<int>();
            for (int i = 0; i < preRedLotto.Length; i++)
            {
                var num = preRedLotto[i] * 0.618;
                if (num - Math.Truncate(num) < 0.1)
                {
                    goldNums.Add((int)Math.Truncate(num));
                }
                else
                {
                    if ((int)Math.Floor(num) > 0)
                        goldNums.Add((int)Math.Floor(num));
                    goldNums.Add((int)Math.Ceiling(num));
                }
            }

            goldNums = goldNums.Distinct().ToList();

            if (isPrint)
                PrintHelper.Print("黄金分割定胆([]个)，很准：", goldNums);

            return goldNums;
        }

        /// <summary>
        /// 红球：明暗点
        /// <para>前3红合一个数，后3红合一个数，目前对于有的开奖号码不知如何计算的</para>
        /// </summary>
        /// <param name="isPrint"></param>
        public static IList<int> CalulatePassword(bool isPrint = true)
        {
            //07 08 10 13 25 27----07 明暗点: 90/81  合10
            //02 10 15 16 17 33----13 明暗点: 91/64  合10
            //01 04 15 17 27 31----01 明暗点: 96/87  合15 小6
            //04 08 12 13 23 29----01 明暗点: 87/78  合15 小6
            //02 03 05 11 15 32----15 明暗点: 91/82  合10
            //08 09 12 18 25 27----12 明暗点: 95/86  合14 小5

            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var preBlueLotto = Data.PreRedBlueLotto[^1];

            var clearCodePlusNums = new List<int>();
            var clearCodeSubtractNums = new List<int>();
            var clearCodeRideNums = new List<int>();

            var secretCodePlusNums = new List<int>();
            var secretCodeSubtractNums = new List<int>();
            var secretCodeRideNums = new List<int>();

            for (int i = 0; i < preRedLotto.Length; i++)
            {
                if ((preRedLotto[i] + preBlueLotto) % 9 == 1)
                {
                    clearCodePlusNums.Add(10);
                }
                else if ((preRedLotto[i] + preBlueLotto) % 9 == 0)
                {
                    clearCodePlusNums.Add(9);
                }
                else
                {
                    clearCodePlusNums.Add((preRedLotto[i] + preBlueLotto) % 9);
                }

                if (Math.Abs(preRedLotto[i] - preBlueLotto) != 1 && Math.Abs(preRedLotto[i] - preBlueLotto) % 9 == 1)
                {
                    clearCodeSubtractNums.Add(10);
                }
                else if (preRedLotto[i] - preBlueLotto != 0 && Math.Abs(preRedLotto[i] - preBlueLotto) % 9 == 0)
                {
                    clearCodeSubtractNums.Add(9);
                }
                else
                {
                    clearCodeSubtractNums.Add(Math.Abs(preRedLotto[i] - preBlueLotto) % 9);
                }

                if (preRedLotto[i] * preBlueLotto != 1 && preRedLotto[i] * preBlueLotto % 9 == 1)
                {
                    clearCodeRideNums.Add(10);
                }
                else if (preRedLotto[i] * preBlueLotto % 9 == 0)
                {
                    clearCodeRideNums.Add(9);
                }
                else
                {
                    clearCodeRideNums.Add(preRedLotto[i] * preBlueLotto % 9);
                }


                if ((preRedLotto[i] + preBlueLotto) % 9 == 0)
                    secretCodePlusNums.Add(9);
                else
                    secretCodePlusNums.Add((preRedLotto[i] + preBlueLotto) % 9);

                if (preRedLotto[i] - preBlueLotto != 0 && Math.Abs(preRedLotto[i] - preBlueLotto) % 9 == 0)
                    secretCodeSubtractNums.Add(9);
                else
                    secretCodeSubtractNums.Add(Math.Abs(preRedLotto[i] - preBlueLotto) % 9);

                if (preRedLotto[i] * preBlueLotto % 9 == 0)
                    secretCodeRideNums.Add(9);
                else
                    secretCodeRideNums.Add(preRedLotto[i] * preBlueLotto % 9);
            }

            var clearCodePlusSum = clearCodePlusNums.Sum(x => x);
            var clearCodeSubtractSum = clearCodeSubtractNums.Sum(x => x);
            var clearCodeRideSum = clearCodeRideNums.Sum(x => x);

            var secretCodePlusSum = secretCodePlusNums.Sum(x => x);
            var secretCodeSubtractSum = secretCodeSubtractNums.Sum(x => x);
            var secretCodeRideSum = secretCodeRideNums.Sum(x => x);

            var clearCode = clearCodePlusSum + clearCodeSubtractSum + clearCodeRideSum;
            var secretCode = secretCodePlusSum + secretCodeSubtractSum + secretCodeRideSum;

            var all = new List<int>();
            var digitsClearCode = Math.Floor(Math.Log10(clearCode) + 1);
            all.Add(clearCode % 10);
            all.Add(clearCode / 10 % 10);
            var digitsSecretCode = Math.Floor(Math.Log10(clearCode) + 1);
            all.Add(secretCode % 10);
            all.Add(secretCode / 10 % 10);

            var compositeValue = Get(clearCode);
            all.Add(compositeValue);

            int Get(int number)
            {
                var compositeValue = number / 10 + number % 10;
                if (compositeValue >= 10)
                {
                    compositeValue = Get(compositeValue);
                }

                return compositeValue;
            }

            all.ToList().ForEach(n =>
            {
                var v = n < 5 ? n + 5 : n - 5;
                all.Add(v);
            });

            all = all.Distinct().OrderBy(x => x).ToList();
            all.Insert(0, clearCode);
            all.Insert(1, secretCode);

            if (isPrint)
            {
                PrintHelper.Print("明暗点：" + string.Format("{0}/{1}", all[0], all[1]), ConsoleColor.DarkGreen);
                PrintHelper.Print("明暗点尾数：" + string.Join(" ", all.Skip(2).OrderBy(x => x)), ConsoleColor.DarkGreen);
            }

            return all;
        }



        /************** 以下是篮球计算方法*************/


        /// <summary>
        /// 篮球：排除法(个人感觉不准)
        /// </summary>
        /// <param name="num">上一期篮球</param>
        public static void CalculateBlueLotto()
        {
            var ExcludeNums = new List<int>();
            int temp;

            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var preBlueLotto = Data.PreRedBlueLotto[^1];

            //尾数预测法
            if (preBlueLotto + 6 <= 16)
            {
                temp = (preBlueLotto + 6) % 10;
                ExcludeNums.Add(temp);
                ExcludeNums.Add(temp + 10);
            }
            else
            {
                temp = (preBlueLotto + 6 - 16) % 10;
                ExcludeNums.Add(temp);
                ExcludeNums.Add(temp + 10);
            }
            if (preBlueLotto + 10 <= 16)
            {
                temp = (preBlueLotto + 10) % 10;
                ExcludeNums.Add(temp);
                ExcludeNums.Add(temp + 10);
            }
            else
            {
                temp = (preBlueLotto + 10 - 16) % 10;
                ExcludeNums.Add(temp);
                ExcludeNums.Add(temp + 10);
            }

            //绝对值尾数法
            temp = Math.Abs(preBlueLotto - 7) % 10;
            ExcludeNums.Add(temp);
            ExcludeNums.Add(temp + 10);

            //期数与日期法
            //temp = (33 + 26) / 16;
            //result.Add(temp);
            //temp = (34 + 28) / 16;
            //result.Add(temp);
            //temp = (35 + 31) / 16;
            //result.Add(temp);
            //temp = (36 + 2) / 16;
            //ExcludeNums.Add(temp);
            temp = (37 + 4) / 16;
            ExcludeNums.Add(temp);
            temp = (38 + 7) / 16;
            ExcludeNums.Add(temp);

            //月份法
            ExcludeNums.Add(DateTime.Now.Month);

            //尾数加1法
            temp = (preBlueLotto % 10 + 1) % 10;
            ExcludeNums.Add(temp);
            ExcludeNums.Add(temp + 10);

            //红色第一位加+3，排除
            //篮球号码十位和各位互换位置，若大于16，则减16，排除
            //上上期篮球号码除以3的余数+上期蓝色号码，排除
            //上两期蓝号尾数相加、相减，各取临近3位，舍弃没有同在两组的尾数，如4，14，4+4=8，5678901，4-4=0，7890123，排除

            var nums = ExcludeNums.Distinct().Where(x => x >= 1 && x <= 16).OrderBy(x => x);
            PrintHelper.Print("篮球排除法，个人感觉不准：", ExcludeNums, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// 篮球：红球两两相减法，去除大于16的号码
        /// </summary>
        public static void CalculateBlueWithRedSubtract()
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();

            var data = new List<int>();
            Get(data, 0, 1);
            data = data.Distinct().Where(n => n <= 16).ToList();
            data.Sort();

            void Get(List<int> data, int n, int k)
            {
                if (n < preRedLotto.Length)
                {
                    if (k < preRedLotto.Length)
                    {
                        data.Add(Math.Abs(preRedLotto[n] - preRedLotto[k]));
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

            PrintHelper.Print("红球两两相减法([]个)", data, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// 篮球：合单，合双
        /// </summary>
        public static void CalculateBlueWithCompositeAndSingle()
        {
            //合数单：1,3,5,7,9,10,12,14,16
            //合数双：2,4,6,8,11,13,15
        }
    }
}
