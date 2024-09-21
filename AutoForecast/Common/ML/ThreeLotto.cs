/*
 * Description: ThreePermutationLotto
 * Author: Chance.zheng
 * Creat Time: 2024/09/06 09:22:07 星期五
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using Microsoft.ML.Data;

namespace AutoForecast;

public class ThreeLotto
{
    [LoadColumn(0)] public float Hundreds { get; set; }

    [LoadColumn(1)] public float Tens { get; set; }

    [LoadColumn(2)] public float Units { get; set; }

    [LoadColumn(3)] public float Target { get; set; }
    
    [LoadColumn(3)] public float Next { get; set; }
}

public class ThreeLottoPrediction
{
    [ColumnName("Score")] public float Predicted { get; set; }

    // [ColumnName("Tens")]
    // public float Tens { get; set; }
    //
    // [ColumnName("Units")]
    // public float Units { get; set; }
}