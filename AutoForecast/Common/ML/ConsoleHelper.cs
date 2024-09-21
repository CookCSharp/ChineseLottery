/*
 * Description: ConsoleHelper
 * Author: Chance.zheng
 * Creat Time: 2024/09/06 11:17:54 星期五
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System.Diagnostics;
using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AutoForecast;

public class ConsoleHelper
{
    public static void PrintRegressionMetrics(string? name, RegressionMetrics metrics)
    {
        Console.WriteLine("*************************************************");
        Console.WriteLine($"*       Metrics for {name} regression model      ");
        Console.WriteLine("*------------------------------------------------");
        Console.WriteLine($"*       LossFn:        {metrics.LossFunction:0.##}");
        Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");
        Console.WriteLine($"*       Absolute loss: {metrics.MeanAbsoluteError:0.##}");
        Console.WriteLine($"*       Squared loss:  {metrics.MeanSquaredError:0.##}");
        Console.WriteLine($"*       RMS loss:      {metrics.RootMeanSquaredError:0.##}");
        Console.WriteLine("*************************************************");
    }

    [Conditional("DEBUG")]
    // This method using 'DebuggerExtensions.Preview()' should only be used when debugging/developing, not for release/production trainings
    public static void PeekDataViewInConsole(MLContext mlContext, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = $"Peek data in DataView: Showing {numberOfRows.ToString()} rows with the columns";
        ConsoleWriteHeader(msg);

        //https://github.com/dotnet/machinelearning/blob/main/docs/code/MlNetCookBook.md#how-do-i-look-at-the-intermediate-data
        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // 'transformedData' is a 'promise' of data, lazy-loading. call Preview
        //and iterate through the returned collection from preview.

        var preViewTransformedData = transformedData.Preview(maxRows: numberOfRows);

        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string lineToPrint = "Row--> ";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
            {
                lineToPrint += $"| {column.Key}:{column.Value}";
            }

            Console.WriteLine(lineToPrint + "\n");
        }
    }

    [Conditional("DEBUG")]
    // This method using 'DebuggerExtensions.Preview()' should only be used when debugging/developing, not for release/production trainings
    public static void PeekVectorColumnDataInConsole(MLContext mlContext, string columnName, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = $"Peek data in DataView: : Show {numberOfRows} rows with just the '{columnName}' column";
        ConsoleWriteHeader(msg);

        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // Extract the 'Features' column.
        var someColumnData = transformedData.GetColumn<float[]>(columnName).Take(numberOfRows).ToList();

        // print to console the peeked rows

        int currentRow = 0;
        someColumnData.ForEach(row =>
        {
            currentRow++;
            String concatColumn = String.Empty;
            foreach (float f in row)
            {
                concatColumn += f.ToString(CultureInfo.InvariantCulture) + " ";
            }

            Console.WriteLine();
            string rowMsg = $"**** Row {currentRow} with '{columnName}' field value ****";
            Console.WriteLine(rowMsg);
            Console.WriteLine(concatColumn);
            Console.WriteLine();
        });
    }

    public static void ConsoleWriteHeader(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }

        var maxLength = lines.Select(x => x.Length).Max();
        Console.WriteLine(new string('#', maxLength));
        Console.ForegroundColor = defaultColor;
    }
}