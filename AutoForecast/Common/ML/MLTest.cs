/*
 * Description: MLTest
 * Author: Chance.zheng
 * Creat Time: 2024/09/06 15:35:18 星期五
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.TimeSeries;
using Microsoft.ML.Transforms.TimeSeries;
using Tensorflow;
using Tensorflow.Keras;
// using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Optimizers;
using Tensorflow.NumPy;

namespace AutoForecast;

public class MLTest
{
    public class TimeSeriesData
    {
        [LoadColumn(0)] public float Value { get; set; }
    }

    public class Prediction
    {
        [ColumnName("Forecasted")] public float[]? Forecasted { get; set; }
    }

    public static void ForecastBySSA()
    {
        // 创建 ML.NET 环境
        var mlContext = new MLContext();

        // 示例时间序列数据
        var data = new List<TimeSeriesData>
        {
            new TimeSeriesData { Value = 8 },
            new TimeSeriesData { Value = 6 },
            new TimeSeriesData { Value = 4 },
            new TimeSeriesData { Value = 1 },
            new TimeSeriesData { Value = 1 },
            new TimeSeriesData { Value = 5 },
            new TimeSeriesData { Value = 3 },
            new TimeSeriesData { Value = 8 },
            new TimeSeriesData { Value = 3 },
            new TimeSeriesData { Value = 8 },
            new TimeSeriesData { Value = 6 },
            new TimeSeriesData { Value = 7 },
            new TimeSeriesData { Value = 7 },
            new TimeSeriesData { Value = 7 },
            new TimeSeriesData { Value = 3 },
            new TimeSeriesData { Value = 7 },
            new TimeSeriesData { Value = 3 },
            new TimeSeriesData { Value = 1 }
        };

        // 将数据加载到 IDataView
        var trainData = mlContext.Data.LoadFromEnumerable(data);

        var seriesLength = data.Count;
        var trainSize = data.Count;

        // 配置时间序列预测模型
        var forecastingEstimator = mlContext.Forecasting.ForecastBySsa(
            nameof(Prediction.Forecasted),
            nameof(TimeSeriesData.Value),
            windowSize: 3, // 窗口大小
            seriesLength: seriesLength, // 时间序列长度
            trainSize: trainSize, // 训练数据大小
            horizon: 3); // 预测的步长

        var model = forecastingEstimator.Fit(trainData);

        // // Fit the forecasting model to the specified product's data series.
        // ITransformer forecastTransformer = forecastingEstimator.Fit(trainData);
        // // Create the forecast engine used for creating predictions.
        // TimeSeriesPredictionEngine<TimeSeriesData, Prediction> forecastEngine = forecastTransformer.CreateTimeSeriesEngine<TimeSeriesData, Prediction>(mlContext);
        // var outputModelPath = @".\Assets\TimeSeriesSSA.zip";
        // // Save the forecasting model so that it can be loaded within an end-user app.
        // forecastEngine.CheckPoint(mlContext, outputModelPath);
        // Prediction originalSalesPrediction = forecastEngine.Predict();
        // Console.WriteLine($"预测值: {string.Join(" ", originalSalesPrediction.Forecasted!)}");

        // 创建预测数据
        var forecastDataView = model.Transform(trainData);
        // 预测
        var forecast = mlContext.Data.CreateEnumerable<Prediction>(forecastDataView, reuseRowObject: false).ToList();
        Console.WriteLine($"Feature预测结果({forecast.Count}组):");
        forecast.ForEach(f => Console.WriteLine($"预测值: {string.Join(" ", f.Forecasted!)}"));
    }

    public static void ForecastByLSTM()
    {
        // Sample time series data
        var data = new float[] { 1, 5, 6, 5, 9, 5, 2, 3, 9, 0, 5 };
        var timeStep = 3;

        // Prepare the dataset
        var (X, y) = CreateDataset(data, timeStep);

        // // Define the LSTM model
        // var model = new Sequential()
        // model.Add(new LSTM());
        // model.Add(new LSTM(50));
        // model.Add(new Dense(1));
        //
        // model.compile(optimizer: new Adam(), loss: "mean_squared_error");
        //
        // // Train the model
        // model.fit(X, y, epochs: 100, batch_size: 1, verbose: 1);
        //
        // // Make predictions
        // var predictions = model.predict(X);
        // Console.WriteLine("Predictions:");
        // foreach (var pred in predictions.ToList()  .Data<float>())
        // {
        //     Console.WriteLine(pred);
        // }

        (NDArray X, NDArray y) CreateDataset(float[] data, int timeStep)
        {
            var X = new float[data.Length - timeStep, timeStep, 1];
            var y = new float[data.Length - timeStep];

            for (int i = 0; i < data.Length - timeStep; i++)
            {
                for (int j = 0; j < timeStep; j++)
                {
                    X[i, j, 0] = data[i + j];
                }

                y[i] = data[i + timeStep];
            }

            return (np.array(X), np.array(y));
        }
    }
}