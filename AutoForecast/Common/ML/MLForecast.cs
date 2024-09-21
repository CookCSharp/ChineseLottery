/*
 * Description: MLForecast
 * Author: Chance.zheng
 * Creat Time: 2024/09/06 09:09:20 星期五
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using PLplot;

namespace AutoForecast;

public class MLForecast
{
    private static readonly Lazy<MLForecast> _lazy = new Lazy<MLForecast>(() => new MLForecast());
    public static MLForecast Instance => _lazy.Value;

    private const string ThreePermutationTrainDataRelativePath = "Assets//History-ThreePermutation-Train.csv";
    private const string ThreePermutationTestDataRelativePath = "Assets//History-ThreePermutation-Test.csv";
    private const string ModelDataRelativePath = "Assets//ModelData.zip";
    private readonly string TrainDataPath = GetAbsolutePath(ThreePermutationTrainDataRelativePath);
    private readonly string TestDataPath = GetAbsolutePath(ThreePermutationTestDataRelativePath);
    private readonly string ModelDataPath = GetAbsolutePath(ModelDataRelativePath);


    public void MLAutoForecast(string[] args)
    {
        // SaveToCsv();

        MLContext mlContext = new MLContext(seed: 0);
        BuildTrainEvaluateAndSaveModel(mlContext);
        TestSinglePrediction(mlContext);
        // PlotRegressionChart(mlContext, TestDataPath, 100, args);
    }

    private void SaveToCsv()
    {
        FindManual.Find(LottoType.ThreePermutation);
        var data = FindManual.GetHistoryData().Values.ToList();
        IList<ThreeLotto> lotto = new List<ThreeLotto>();
        for (int i = 0; i < data.Count - 1; i++)
        {
            lotto.Add(new ThreeLotto()
            {
                Hundreds = data[i][0],
                Tens = data[i][1],
                Units = data[i][2],
                Target = data[i][0] * 100 + data[i][1] * 10 + data[i][2],
                Next = data[i + 1][0] * 100 + data[i + 1][1] * 10 + data[i + 1][2]
            });
        }

        // data.ForEach(g => lotto.Add(new ThreeLotto() { Hundreds = g[0], Tens = g[1], Units = g[2], Target = g[0] * 100 + g[1] * 10 + g[2] }));
        using var writer = new StreamWriter(GetAbsolutePath(ThreePermutationTrainDataRelativePath));
        using var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
        csv.WriteRecords(lotto);
    }

    private ITransformer BuildTrainEvaluateAndSaveModel(MLContext mlContext)
    {
        // STEP 1: Common data loading configuration
        IDataView baseTrainingDataView = mlContext.Data.LoadFromTextFile<ThreeLotto>(TrainDataPath, separatorChar: ',', hasHeader: true);
        IDataView testDataView = mlContext.Data.LoadFromTextFile<ThreeLotto>(TestDataPath, separatorChar: ',', hasHeader: true);

        //Sample code of removing extreme data like "outliers" for FareAmounts higher than $150 and lower than $1 which can be error-data 
        // var cnt1 = baseTrainingDataView.GetColumn<float>(nameof(ThreeLotto.FareAmount)).Count();
        // IDataView trainingDataView = mlContext.Data.FilterRowsByColumn(baseTrainingDataView, nameof(TaxiTrip.FareAmount), lowerBound: 1, upperBound: 150);
        // var cnt2 = trainingDataView.GetColumn<float>(nameof(ThreeLotto.FareAmount)).Count();

        // IDataView trainingDataView = baseTrainingDataView;
        IDataView trainingDataView = mlContext.Data.FilterRowsByColumn(baseTrainingDataView, nameof(ThreeLotto.Next));
        // IDataView trainingDataView = mlContext.Data.FilterRowsByMissingValues(baseTrainingDataView, nameof(ThreeLotto.Hundreds), nameof(ThreeLotto.Tens), nameof(ThreeLotto.Units));

        // STEP 2: Common data process configuration with pipeline data transformations
        var dataProcessPipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", nameof(ThreeLotto.Next))
            // .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "HundredsFeatures", inputColumnName: nameof(ThreeLotto.Hundreds)))
            // .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "TensFeatures", inputColumnName: nameof(ThreeLotto.Tens)))
            // .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "UnitsFeatures", inputColumnName: nameof(ThreeLotto.Units)))
            // .Append(mlContext.Transforms.NormalizeMeanVariance(outputColumnName: nameof(ThreeLotto.Hundreds)))
            // .Append(mlContext.Transforms.NormalizeMeanVariance(outputColumnName: nameof(ThreeLotto.Tens)))
            // .Append(mlContext.Transforms.NormalizeMeanVariance(outputColumnName: nameof(ThreeLotto.Units)))
            // .Append(mlContext.Transforms.Concatenate(outputColumnName: "Features", "HundredsFeatures", "TensFeatures", "UnitsFeatures"));
            .Append(mlContext.Transforms.Concatenate(outputColumnName: "Features", nameof(ThreeLotto.Hundreds), nameof(ThreeLotto.Tens), nameof(ThreeLotto.Units), nameof(ThreeLotto.Target)));

        // (OPTIONAL) Peek data (such as 5 records) in training DataView after applying the ProcessPipeline's transformations into "Features" 
        ConsoleHelper.PeekDataViewInConsole(mlContext, trainingDataView, dataProcessPipeline, 5);
        ConsoleHelper.PeekVectorColumnDataInConsole(mlContext, "Features", trainingDataView, dataProcessPipeline, 5);

        // STEP 3: Set the training algorithm, then create and config the modelBuilder - Selected Trainer (SDCA Regression algorithm)                            
        var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
        var trainingPipeline = dataProcessPipeline.Append(trainer);

        // STEP 4: Train the model fitting to the DataSet
        //The pipeline is trained on the dataset that has been loaded and transformed.
        Console.WriteLine("=============== Training the model ===============");
        var trainedModel = trainingPipeline.Fit(trainingDataView);

        var predictions1 = trainedModel.Transform(testDataView);
        var results = mlContext.Data.CreateEnumerable<ThreeLottoPrediction>(predictions1, reuseRowObject: false).ToList();
        
        // STEP 5: Evaluate the model and show accuracy stats
        Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");

        IDataView predictions = trainedModel.Transform(testDataView);
        var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

        ConsoleHelper.PrintRegressionMetrics(trainer.ToString(), metrics);

        // STEP 6: Save/persist the trained model to a .ZIP file
        mlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelDataPath);

        Console.WriteLine("The model is saved to {0}", ModelDataPath);

        return trainedModel;
    }

    private void TestSinglePrediction(MLContext mlContext)
    {
        var threeLottoSample = new ThreeLotto { Hundreds = 3, Tens = 5, Units = 6, Target = 356, Next = 0 };

        ITransformer trainedModel = mlContext.Model.Load(ModelDataPath, out var modelInputSchema);

        // Create prediction engine related to the loaded trained model
        var predEngine = mlContext.Model.CreatePredictionEngine<ThreeLotto, ThreeLottoPrediction>(trainedModel);
        
        var resultPrediction = predEngine.Predict(threeLottoSample);
        Console.WriteLine("**********************************************************************");
        Console.WriteLine($"Predicted lotto: {resultPrediction.Predicted}, actual lotto: 376");
        Console.WriteLine("**********************************************************************");
    }

    private void PlotRegressionChart(MLContext mlContext, string testDataSetPath, int numberOfRecordsToRead, string[] args)
    {
        ITransformer trainedModel;
        using (var stream = new FileStream(ModelDataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            trainedModel = mlContext.Model.Load(stream, out var modelInputSchema);
        }

        // Create prediction engine related to the loaded trained model
        var predFunction = mlContext.Model.CreatePredictionEngine<ThreeLotto, ThreeLottoPrediction>(trainedModel);

        string chartFileName = "";
        using (var pl = new PLStream())
        {
            // use SVG backend and write to SineWaves.svg in current directory
            if (args.Length == 1 && args[0] == "svg")
            {
                pl.sdev("svg");
                chartFileName = "ThreeLotto.svg";
                pl.sfnam(chartFileName);
            }
            else
            {
                pl.sdev("pngcairo");
                chartFileName = "ThreeLotto.png";
                pl.sfnam(chartFileName);
            }

            // use white background with black foreground
            pl.spal0("cmap0_alternate.pal");

            // Initialize plplot
            pl.init();

            // set axis limits
            const int xMinLimit = 0;
            const int xMaxLimit = 10; //Rides larger than $35 are not shown in the chart
            const int yMinLimit = 0;
            const int yMaxLimit = 10; //Rides larger than $35 are not shown in the chart
            pl.env(xMinLimit, xMaxLimit, yMinLimit, yMaxLimit, AxesScale.Independent, AxisBox.BoxTicksLabelsAxes);

            // Set scaling for mail title text 125% size of default
            pl.schr(0, 1.25);

            // The main title
            pl.lab("Measured", "Predicted", "Distribution of Lotto Prediction");

            // plot using different colors
            // see http://plplot.sourceforge.net/examples.php?demo=02 for palette indices
            pl.col0(1);

            int totalNumber = numberOfRecordsToRead;
            var testData = GetDataFromCsv(testDataSetPath, totalNumber).ToList();

            //This code is the symbol to paint
            char code = (char)9;

            // plot using other color
            //pl.col0(9); //Light Green
            //pl.col0(4); //Red
            pl.col0(2); //Blue

            double yTotal = 0;
            double xTotal = 0;
            double xyMultiTotal = 0;
            double xSquareTotal = 0;

            for (int i = 0; i < testData.Count; i++)
            {
                var x = new double[1];
                var y = new double[1];

                //Make Prediction
                var FarePrediction = predFunction.Predict(testData[i]);

                x[0] = testData[i].Hundreds;
                y[0] = FarePrediction.Predicted;

                //Paint a dot
                pl.poin(x, y, code);

                xTotal += x[0];
                yTotal += y[0];

                double multi = x[0] * y[0];
                xyMultiTotal += multi;

                double xSquare = x[0] * x[0];
                xSquareTotal += xSquare;

                double ySquare = y[0] * y[0];

                //{FarePrediction.Tens} {FarePrediction.Units}
                Console.WriteLine($"-------------------------------------------------");
                Console.WriteLine($"Predicted : {FarePrediction.Predicted}");
                Console.WriteLine($"Actual:    {testData[i].Hundreds} {testData[i].Tens} {testData[i].Units}");
                Console.WriteLine($"-------------------------------------------------");
            }

            // Regression Line calculation explanation:
            // https://www.khanacademy.org/math/statistics-probability/describing-relationships-quantitative-data/more-on-regression/v/regression-line-example

            double minY = yTotal / totalNumber;
            double minX = xTotal / totalNumber;
            double minXY = xyMultiTotal / totalNumber;
            double minXsquare = xSquareTotal / totalNumber;

            double m = ((minX * minY) - minXY) / ((minX * minX) - minXsquare);

            double b = minY - (m * minX);

            //Generic function for Y for the regression line
            // y = (m * x) + b;

            double x1 = 1;
            //Function for Y1 in the line
            double y1 = (m * x1) + b;

            double x2 = 39;
            //Function for Y2 in the line
            double y2 = (m * x2) + b;

            var xArray = new double[2];
            var yArray = new double[2];
            xArray[0] = x1;
            yArray[0] = y1;
            xArray[1] = x2;
            yArray[1] = y2;

            pl.col0(4);
            pl.line(xArray, yArray);

            // end page (writes output to disk)
            pl.eop();

            // output version of PLplot
            pl.gver(out var verText);
            Console.WriteLine("PLplot version " + verText);
        } // the pl object is disposed here

        // Open Chart File In Microsoft Photos App (Or default app, like browser for .svg)

        Console.WriteLine("Showing chart...");
        var p = new Process();
        string chartFileNamePath = @".\" + chartFileName;
        // p.StartInfo = new ProcessStartInfo(chartFileNamePath)
        // {
        //     UseShellExecute = true
        // };
        // p.Start();
    }

    private IEnumerable<ThreeLotto> GetDataFromCsv(string dataLocation, int numMaxRecords)
    {
        IEnumerable<ThreeLotto> records = File.ReadAllLines(dataLocation)
            .Skip(1)
            .Select(x => x.Split(','))
            .Select(x => new ThreeLotto()
            {
                Hundreds = float.Parse(x[0], CultureInfo.InvariantCulture),
                Tens = float.Parse(x[1], CultureInfo.InvariantCulture),
                Units = float.Parse(x[2], CultureInfo.InvariantCulture),
            })
            .Take<ThreeLotto>(numMaxRecords);

        return records;
    }

    private static string GetAbsolutePath(string csvFileName)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, csvFileName);
    }
}