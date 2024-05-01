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
        /// 红球：2进制、9进制、11进制计算法。
        /// <para>命中4、5、6个的概率几乎为0，命中0、3个概率其次，命中1、2个概率较大</para>
        /// <para>9进制命中概率排序，以1000组数据来看，1=2>0=3>4=5=6，命中概率分别为：33%、35%、15%、15%、和为2%</para>
        /// <para>11进制命中概率排序，以1000组数据来看，1=2>0=3>4=5=6，命中概率分别为：38%、30%、19%、12%、和为1%</para>
        /// 9，11进制数中斜连号99%是下期胆号(我不知道如何判断，尽量不采用吧)
        /// </summary>
        /// <returns></returns>
        public static IList<int> Calculate2911Lotto(int n = 9, bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();

            var binary = Get(2);
            if (isPrint)
                PrintHelper.Print("2进制([]个)，一般出1-2个：", binary);

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

            return n == 2 ? binary : n == 9 ? noveary : undecimal;
        }




        /**************以下是定胆方法*************/


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
                PrintHelper.Print("尾数定胆法([]个，下期尾数)，不是很准：", mantissaNums);

            return mantissaNums;
        }

        /// <summary>
        /// 红球：中间数定胆法。
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateProbableMiddle(bool isPrint = true)
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
                PrintHelper.Print("中间数定胆法([]个)，不是很准：", middleNums);

            return middleNums;
        }

        /// <summary>
        /// 红球：黄金分割定胆法，0.618。
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateProbableGoldedCut(bool isPrint = true)
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
                PrintHelper.Print("黄金分割定胆法([]个)，很准：", goldNums);

            return goldNums;
        }

        /// <summary>
        /// 红球：规律计算定胆法。
        /// </summary>
        /// <returns></returns>
        public static IList<int> CalculateProbableRed(bool isPrint = true)
        {
            //同期2、1号码相减；同期4、1号码相减；同期5、1号码相减
            //同期1、3号码相加；两期1位号码相加；
            //同期第5位数减3；同期第6位数减2；18减同期第1位

            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var nums = new List<int>();
            nums.Add(preRedLotto[1] - preRedLotto[0]);
            nums.Add(preRedLotto[3] - preRedLotto[0]);
            nums.Add(preRedLotto[4] - preRedLotto[0]);
            nums.Add(preRedLotto[0] + preRedLotto[2]);
            nums.Add(preRedLotto[4] - 3);
            nums.Add(preRedLotto[5] - 2);
            nums.Add(18 - preRedLotto[0]);

            nums = nums.Distinct().ToList();
            nums.Sort();

            if (isPrint)
                PrintHelper.Print("规律计算定胆法([]个)，很准：", nums);

            return nums;
        }

        /// <summary>
        /// 红球：差值之和定胆法。
        /// </summary>
        /// <remarks>
        /// 和值减5，十位个位相加确定下一期首号，该种情况要么长时间不出现，要么连续两三期出现或隔一期就会出现
        /// </remarks>
        /// <returns></returns>
        public static IList<int> CalculateSumSubtract(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var nums = new List<int>();

            int sum = 0;
            for (int i = 0; i < preRedLotto.Length - 1; i++)
            {
                sum += preRedLotto[i + 1] - preRedLotto[i];
            }
            var sumSubtract5 = sum - 5;
            var sumPlus5 = sum + 5;

            nums.Add(sum);
            nums.Add(Get(sum));
            nums.Add(sumSubtract5);
            nums.Add(Get(sumSubtract5));
            nums.Add(sumPlus5);
            nums.Add(Get(sumPlus5));

            if (Get(sumSubtract5) >= 10)
            {
                nums.Add(Get(Get(sumSubtract5)));
            }

            if (Get(sumPlus5) >= 10)
            {
                nums.Add(Get(Get(sumPlus5)));
            }

            int Get(int n)
            {
                var num = 0;
                if (n >= 10)
                {
                    num = n / 10 + n % 10;
                }
                else
                {
                    num = n;
                }

                return num;
            }

            nums = nums.Distinct().ToList();
            nums.Sort();

            if (isPrint)
                PrintHelper.Print("差值之和定胆法([]个)，很准：", nums);

            return nums;
        }

        /// <summary>
        /// 红球：篮球和还原值定胆法。
        /// </summary>
        /// <param name="blueLotto"></param>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static IList<int> CalculateByRetoreValue(int blueLotto, bool isPrint = true)
        {
            int[] values = new int[6] { 10, 19, 28, 37, 46, 64 };

            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            var nums = new List<int>();

            for (int i = 0; i < values.Length; i++)
            {
                nums.Add(Math.Abs(values[i] - blueLotto));
                nums.Add(values[i] + blueLotto);
                if (values[i] % blueLotto == 0)
                    nums.Add(values[i] / blueLotto);
                nums.Add(values[i] * blueLotto);
            }

            nums = nums.Distinct().Where(n => n >= 1 && n <= 33).OrderBy(x => x).ToList();

            if (isPrint)
                PrintHelper.Print("篮球和还原值定胆法([]个)：", nums);

            return nums;
        }

        /// <summary>
        /// 第一个红球
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static int CaculateFirstLotto(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();
            int sum = 0;
            for (int i = 0; i < preRedLotto.Length - 1; i++)
            {
                sum += preRedLotto[i + 1] - preRedLotto[i];
            }
            var sumSubtract = sum - 5;
            var num = sumSubtract / 10 + sumSubtract % 10;

            if (isPrint)
                PrintHelper.Print(string.Format("预测{0}期红球首号为：{1}", Data.CurrentPeriod, num));

            return num;
        }

        /// <summary>
        /// 红球：明暗点
        /// <para>前3红合一个数，后3红合一个数，目前对于有的开奖号码不知如何计算的</para>
        /// </summary>
        /// <param name="isPrint"></param>
        public static IList<int> CalculatePassword(bool isPrint = true)
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

            var threeDistance = new List<int>
            {
                clearCodePlusNums.Take(3).Sum(),
                clearCodePlusNums.Skip(3).Sum(),
                clearCodeSubtractNums.Take(3).Sum(),
                clearCodeSubtractNums.Skip(3).Sum(),
                clearCodeRideNums.Take(3).Sum(),
                clearCodeRideNums.Skip(3).Sum(),
                secretCodePlusNums.Take(3).Sum(),
                secretCodePlusNums.Skip(3).Sum(),
                secretCodeSubtractNums.Take(3).Sum(),
                secretCodeSubtractNums.Skip(3).Sum(),
                secretCodeRideNums.Take(3).Sum(),
                secretCodeRideNums.Skip(3).Sum(),
            };

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

            all.Add(333);
            threeDistance = threeDistance.Distinct().OrderBy(x => x).ToList();
            all.AddRange(threeDistance);
            var index = all.IndexOf(333);

            if (isPrint)
            {
                PrintHelper.Print(string.Format("计算号码：{0}", string.Join(" ", Data.PreRedBlueLotto)), ConsoleColor.DarkGreen);
                PrintHelper.Print("明暗点：" + string.Format("{0}/{1}", all[0], all[1]), ConsoleColor.DarkGreen);
                PrintHelper.Print("明暗点尾数：" + string.Join(" ", all.Skip(2).Take(index - 2)), ConsoleColor.DarkGreen);
                PrintHelper.Print("3隔点数为：" + string.Join(" ", all.Skip(index + 1)), ConsoleColor.DarkGreen);
                //PrintHelper.Print("3隔点数为：" + string.Join(" ", threeDistance.Distinct().OrderBy(x => x)), ConsoleColor.DarkGreen);
            }

            return all;
        }

        /// <summary>
        /// 分别计算红球前三位和后三位合数
        /// </summary>
        /// <param name="data"></param>
        public static void CalculateLightAndShade(IList<int[]> data)
        {
            foreach (var d in data)
            {
                var sumOf123 = d[0] + d[1] + d[2];
                var sumOf456 = d[3] + d[4] + d[5];
                var clearCode = sumOf123 / 10 + sumOf123 % 10;
                var secretCode = sumOf456 / 10 + sumOf456 % 10;
                clearCode = clearCode / 10 + clearCode % 10;
                secretCode = secretCode / 10 + secretCode % 10;
                PrintHelper.Print(string.Format("号码{0}，前三位和值{1}，后三位和值{2}", string.Join(" ", d), clearCode, secretCode), ConsoleColor.DarkGreen);
            }
        }



        /**************以下是取红球范围的方法**********/

        /// <summary>
        /// 红球：冷热温计算
        /// </summary>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public static (IList<int>, IList<int>, IList<int>) CalculateColdHotWarm(bool isPrint = true)
        {
            History.Instance.ResolveJsonFile();
            History.Instance.Wait();
            History.Instance.GetData(10, out var historyRedData, out var historyData);

            var nums = historyRedData.Values.SelectMany(x => x).ToList();

            var cold = new List<int>();
            var hot = new List<int>();
            var warm = new List<int>();
            for (int i = 1; i <= 33; i++)
            {
                if (nums.Count(n => n == i) <= 1)
                {
                    cold.Add(i);
                }
                else if (nums.Count(n => n == i) >= 3)
                {
                    hot.Add(i);
                }
                else
                {
                    warm.Add(i);
                }
            }

            if (isPrint)
            {
                PrintHelper.Print("冷号：", cold);
                PrintHelper.Print("热号：", hot);
                PrintHelper.Print("温号：", warm);
            }

            return (cold, hot, warm);
        }


        /**************以下是篮球计算方法*************/

        /// <summary>
        /// 篮球：排除法(个人感觉不准)
        /// </summary>
        /// <param name="num">上一期篮球</param>
        public static void CalculateBlueLotto1()
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
        /// 篮球：红球两两相减排除法，去除大于16的号码(个人感觉不准)
        /// </summary>
        public static IList<int> CalculateBlueWithRedSubtract(bool isPrint = true)
        {
            var preRedLotto = Data.PreRedBlueLotto.Take(6).ToArray();

            var excludeNums = new List<int>();
            Get(excludeNums, 0, 1);
            excludeNums = excludeNums.Distinct().Where(n => n <= 16).ToList();
            excludeNums.Sort();

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

            if (isPrint)
                PrintHelper.Print("篮球排除法：", excludeNums, ConsoleColor.DarkBlue);

            return excludeNums;
        }

        /// <summary>
        /// 篮球：排除法
        /// </summary>
        public static void CalculateExcludeBlueLotto()
        {
            var firstMantissa = Data.PreRedBlueLotto[0] % 10;
            var excludeNums = new List<int>()
            {
                //本期期尾+1，杀蓝
                Data.CurrentPeriod % 10 + 1,
                //红1位+3(大于16减16)，杀蓝
                (Data.PreRedBlueLotto[0] + 3) % 16,
                //红1位乘2(大于16减16)，杀蓝
                Data.PreRedBlueLotto[0] * 2 % 16,               
                //红3位尾+蓝尾(大于16减16)，杀蓝
                (Data.PreRedBlueLotto[2] % 10 + Data.PreRedBlueLotto[^1] % 10) % 16,
                //红3位+蓝尾(大于16减16)，得尾，杀蓝
                (Data.PreRedBlueLotto[2] + Data.PreRedBlueLotto[^1] % 10) % 10,
                //上期期号乘11加第4个红球除以16，得余数，杀蓝
                ((Data.CurrentPeriod - 1) * 11 + Data.PreRedBlueLotto[3]) % 16,
                //上期红球之和除以16得余数，必杀蓝
                Data.PreRedBlueLotto.Take(6).Sum() % 16,

                //以下不准
                ////上期最小红码尾数加5(大于10减10)，杀尾
                //(Data.PreRedBlueLotto[0] + 5) % 10,
                //(Data.PreRedBlueLotto[0] + 5) % 10 + 10,
                ////上期蓝码尾减1，杀尾
                //Data.PreRedBlueLotto[^1] % 10,
                //Data.PreRedBlueLotto[^1] % 10 + 10,
                ////上期红码第4位尾加4，杀尾
                //Data.PreRedBlueLotto[3] % 10 + 4,
                //Data.PreRedBlueLotto[3] % 10 + 4 + 10,

                //8,5,1,5,7,12,16,

                //AC值乘2-6，必杀蓝
                //上两期蓝尾相乘，尾加5，杀蓝
                //上两期蓝号相加(大于16减16)，杀蓝
                //周日蓝号前两期相加(大于16减16)，杀蓝，杀周四、周日
                //周四蓝号前两期相加(大于16减16)，杀蓝，杀周四、周日
                //前3期蓝号尾相加(大于16减16)，杀蓝
                //前3期蓝号相加(大于16减16或16得倍数)，杀蓝
            };

            var nums = excludeNums.Distinct().Where(x => x >= 1 && x <= 16).OrderBy(x => x);
            PrintHelper.Print("篮球排除法：", excludeNums, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// 篮球：合单，合双
        /// </summary>
        public static void CalculateBlueWithCompositeAndSingle()
        {
            //合数单：1,3,5,7,9,10,12,14,16
            //合数双：2,4,6,8,11,13,15


            //篮球80%是红球还原而且一般情况是同时对应2个红球还原，有心的可以找出明暗码不同的每期号码对应一下看看说的是否准确。
            //还原的数值是10,19,28,37,46,64； 
            //如红33，落蓝13那这个就是蓝对应当期红球还原33 + 13 = 46 ，如红07，落蓝04那么这个就是蓝对应当期红还原07 * 04 = 28 ，如红15，落蓝04那这个就是蓝对应当期红球还原15 + 04 = 19 注意的是蓝一般对应2个红同时还原举例 ，
            //如红12,23落蓝02，那么这2个红对应蓝02就是同时还原 12 - 02 = 10    23 * 02 = 46


            //双色球杀蓝秘诀(不准)
            //双色球凡是上期出1尾，下期则不出：06 10
            //双色球凡是上期出2尾，下期则不出：03 07 08 09
            //双色球凡是上期出3尾，下期则不出：03
            //双色球凡是上期出4尾，下期则不出：05 07 09
            //双色球凡是上期出5尾，下期则不出：02 04 10 16
            //双色球凡是上期出6尾，下期则不出：05 08 10 12 16
            //双色球凡是上期出7尾，下期则不出：09 12 15
            //双色球凡是上期出8尾，下期则不出：04 05 06 07 08 10 12 14 16
            //双色球凡是上期出9尾，下期则不出：01 03 05 08 10 11 12 14 16
            //双色球凡是上期出0尾，下期则不出：01 02 03 04 08 09 10 12 13 16


            //福彩双色球第一个号码：取上一期开奖号码中的一个，即重复码。
            //福彩双色球第二个号码：取上一期的某个号的边码。
            //福彩双色球第三个号码：取上三期开奖号码中的夹码。
            //福彩双色球第四个号码：取开奖号码弹出的间隔状态码。
            //福彩双色球第五个号码：最冷的号码。
            //福彩双色球第六个号码：最热的号码。


            //蓝号预测方案
            //例如：43期蓝号+44期蓝号=46期蓝号范围
            //8+6=14 和值大于16减去16 出号范围就是(10-18)
        }
    }
}
