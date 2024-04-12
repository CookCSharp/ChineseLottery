/*
 *Description: Extensions
 *Author: Chance.zheng
 *Creat Time: 2024/4/10 11:12:46
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
    public static class Extensions
    {
        public static string ToD2String(this IEnumerable<int> lottos)
        {
            return string.Join(" ", lottos.Select(n => n.ToString("D2")));
        }
    }
}
