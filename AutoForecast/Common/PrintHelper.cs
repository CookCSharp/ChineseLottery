/*
 * Description: PrintHelper
 * Author: Chance.zheng
 * Creat Time: 2024/08/31 21:22:54 星期六
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

public class PrintHelper
{
    public static void PrintResult(string str)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(str);
    }

    public static void PrintForecastResult(string str, ConsoleColor color = ConsoleColor.DarkCyan, bool isNewLine = true)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(str);
        if (isNewLine) Console.WriteLine(Environment.NewLine);
    }
}