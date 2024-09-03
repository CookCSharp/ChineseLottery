/*
 * Description: ThreePermutationTool
 * Author: Chance.zheng
 * Creat Time: 2024/09/02 19:57:39 星期一
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

public static class ThreePermutationTool
{
    public static int[] GetSingle(int[] way012, int[] size)
    {
        return Tool.GetSingleLessThanTen(way012, size);
    }

    public static List<int[]> SumValue012PathFilter(this List<int[]> data, int[] path012)
    {
        return data.Where(g => path012.Contains((g[0] + g[1] + g[2]) % 3)).ToList();
    }

    public static List<int[]> SumValueMantissa012PathFilter(this List<int[]> data, int[] path012)
    {
        return data.Where(g => path012.Contains((g[0] + g[1] + g[2]) % 10 % 3)).ToList();
    }

    public static List<int[]> SumValueMantissaLargeMediumSmallFilter(this List<int[]> data, int[] size)
    {
        return data.Where(g => size.Contains((g[0] + g[1] + g[2]) % 10)).ToList();
    }

    public static List<int[]> SpanValue012PathFilter(this List<int[]> data, int[] path012)
    {
        return data.Where(g => path012.Contains((g.Max() - g.Min()) % 3)).ToList();
    }

    public static List<int[]> SpanValueLargeMediumSmallFilter(this List<int[]> data, int[] size)
    {
        return data.Where(g => size.Contains(g.Max() - g.Min())).ToList();
    }
}