/*
 *Description: Data
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:31:35
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace ThreeDLotto
{
    public class Data
    {
        /// <summary>
        /// 上期开奖号码
        /// </summary>
        public static int[] PreLotto = [4,6,8];

        /// <summary>
        /// 下期开奖期数
        /// </summary>
        public const int CurrentPeriod = 162;

        /// <summary>
        /// 所有号码
        /// </summary>
        public static int[] AllLotto = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        public static readonly List<int[]> AllTwoLottos = new()
        {
            //new int[2] { 0, 2 },
            //new int[2] { 0, 4 },
            //new int[2] { 0, 5 },
            //new int[2] { 0, 7 },
            //new int[2] { 0, 9 },
            //new int[2] { 2, 4 },
            //new int[2] { 2, 5 },
            new int[2] { 2, 7 },
            new int[2] { 2, 9 },
            new int[2] { 4, 5 },
            new int[] { 4, 7 },
            new int[2] { 4, 9 },
            new int[2] { 5, 7 },
            new int[2] { 5, 9 },
            new int[2] { 7, 9 },
            new int[2] { 0, 0 },
            new int[2] { 2, 2 },
            new int[2] { 4, 4 },
            new int[2] { 5, 5 },
            new int[2] { 7, 7 },
            new int[2] { 9, 9 },
            new int[2] { 1, 3 },
            new int[2] { 1, 6 },
            new int[2] { 1, 8 },
            new int[2] { 3, 6 },
            new int[2] { 3, 8 },
            new int[2] { 6, 8 },
            new int[2] { 1, 1 },
            new int[2] { 3, 3 },
            new int[2] { 6, 6 },
            new int[2] { 8, 8 },
        };

        public static void SetLotto(IEnumerable<int> lotto) => PreLotto = lotto.ToArray();
    }
}
