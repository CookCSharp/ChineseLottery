/*
 *Description: Data
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:31:35
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
    public class Data
    {
        /// <summary>
        /// 上期开奖号码
        /// </summary>
        public static int[] PreLotto = [0, 6, 7];

        /// <summary>
        /// 下期开奖期数
        /// </summary>
        public const int CurrentPeriod = 130;

        /// <summary>
        /// 所有号码
        /// </summary>
        public static int[] AllLotto = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        public static void SetLotto(IEnumerable<int> lotto) => PreLotto = lotto.ToArray();
    }
}
