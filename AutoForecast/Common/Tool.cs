/*
 * Description: Tool
 * Author: Chance.zheng
 * Creat Time: 2024/08/22 19:30:09 星期四
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

public static class Tool
{
#if !SUPPER
    private static readonly int[] Reds = Enumerable.Range(1, 35).ToArray();
    private static readonly int[] Blues = Enumerable.Range(1, 12).ToArray();
#else
    private static readonly int[] Reds = Enumerable.Range(1, 33).ToArray();
    private static readonly int[] Blues = Enumerable.Range(1, 16).ToArray();
#endif

    public static int[] GetRed(int[] mantissaValues, int[] way012)
    {
        return Reds.FilterByMantissaValue(mantissaValues).FilterByValue012(way012);
    }

    public static int[] GetBlue(int[] mantissaValues, int[] way012)
    {
        return Blues.FilterByMantissaValue(mantissaValues).FilterByValue012(way012);
    }

    public static int[] GetSingleLessThanTen(int[] way012, int[] size)
    {
        return Enumerable.Range(0, 10).ToArray().FilterByValue012(way012).FilterByLargeMediumSmallValue(size);
    }

    public static List<int[]> FindAllSortedCombinations(int[] first, int[] second, int[] third)
    {
        var result = new List<int[]>();
        GenerateCombinations(first, second, third, 0, [], result);
        return result;
    }

    public static List<int[]> FindAllSortedCombinations(int[] first, int[] second, int[] third, int[] fourth, int[] fifth)
    {
        var result = new List<int[]>();
        GenerateCombinations(first, second, third, fourth, fifth, 0, [], result);
        return result;
    }

    public static List<int[]> FindAllSortedCombinations(int[] first, int[] second, int[] third, int[] fourth, int[] fifth, int[] sixth)
    {
        var result = new List<int[]>();
        GenerateCombinations(first, second, third, fourth, fifth, sixth, 0, [], result);
        return result;
    }

    private static void GenerateCombinations(int[] first, int[] second, int[] third, int index, List<int> current, List<int[]> result)
    {
        if (index == 3)
        {
            // 当所有集合都已处理完，添加到结果
            result.Add([..current]);
            return;
        }

        var lists = new List<int[]> { first, second, third };
        var list = lists[index];

        // 递归处理每一个集合的每个元素
        foreach (var value in list)
        {
            current.Add(value);
            GenerateCombinations(first, second, third, index + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }

    private static void GenerateCombinations(int[] first, int[] second, int[] third, int[] fourth, int[] fifth, int index, List<int> current, List<int[]> result)
    {
        if (index == 5)
        {
            // 当所有集合都已处理完，添加到结果
            result.Add([..current]);
            return;
        }

        var lists = new List<int[]> { first, second, third, fourth, fifth };
        var list = lists[index];

        // 递归处理每一个集合的每个元素
        foreach (var value in list)
        {
            if (current.Count == 0 || value > current[^1])
            {
                current.Add(value);
                GenerateCombinations(first, second, third, fourth, fifth, index + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }

    private static void GenerateCombinations(int[] first, int[] second, int[] third, int[] fourth, int[] fifth, int[] sixth, int index, List<int> current, List<int[]> result)
    {
        if (index == 6)
        {
            // 当所有集合都已处理完，添加到结果
            result.Add([..current]);
            return;
        }

        var lists = new List<int[]> { first, second, third, fourth, fifth, sixth };
        var list = lists[index];

        // 递归处理每一个集合的每个元素
        foreach (var value in list)
        {
            if (current.Count == 0 || value > current[^1])
            {
                current.Add(value);
                GenerateCombinations(first, second, third, fourth, fifth, sixth, index + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }

    public static string ToD2String(this IEnumerable<int> data)
        => string.Join(" ", data.Select(n => n.ToString("D2")));

    public static List<int> ToMantissaValues(this IEnumerable<int> mantissaValues012)
    {
        var values = new List<int>();
        foreach (var v in mantissaValues012)
        {
            if (v == 0)
                values.AddRange([0, 3, 6, 9]);
            else if (v == 1)
                values.AddRange([1, 4, 7]);
            else
                values.AddRange([2, 5, 8]);
        }

        return values;
    }

    //小(0-2)：-1  中(3-6)：0  大(7-9)：1
    public static int ToLargeMediumSmallConverterValue(this int value)
    {
        if (value <= 2)
            return -1;
        if (value >= 7)
            return 1;
        return 0;
    }

    public static int[] ToLargeMediumSmallValue(this string description)
    {
        if (description == "大")
            return [7, 8, 9];
        if (description == "中")
            return [3, 4, 5, 6];
        return [0, 1, 2];
    }

    public static string ToLargeMediumSmallDescription(this int value)
    {
        if (value <= 2)
            return "小";
        if (value >= 7)
            return "大";
        return "中";
    }
    
    private static int[] FilterByMantissaValue(this int[] values, int[] mantissaValues)
        => values.Where(n => mantissaValues.Contains(n % 10)).ToArray();

    private static int[] FilterByValue012(this int[] values, int[] way012)
        => values.Where(n => way012.Contains(n % 3)).ToArray();

    private static int[] FilterByLargeMediumSmallValue(this int[] values, int[] size)
        => values.Where(n => size.Contains(n)).ToArray();
}