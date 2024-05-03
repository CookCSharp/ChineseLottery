namespace ThreeDLotto
{
    /// <summary>
    /// 福彩3D
    /// </summary>
    internal class Program
    {
        private static bool _isVerifyHistory = false; //是否进行历史数据验证，还是预测数据

        static void Main(string[] args)
        {
            if (_isVerifyHistory)
            {
                VerifyHelper.VerifyPastResults();
            }
            else
            {
                //FindHelper.Find012Path(1, [0, 1, 1, 2, 2, 0, 2, 2]);
                //FindHelper.Find012Path(2, [0, 0, 2, 0, 0, 0, 1]);
                //FindHelper.Find012Path(3, [1, 2, 2, 1, 0, 0, 0, 2, 2]);
                FindHelper.FindMax012Path([0, 1, 2, 0, 0, 0, 1]);
                //FindHelper.FindPrimeComposite(1, [0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1]);
                //FindHelper.FindPrimeComposite(3, [1, 1, 1, 0, 0, 0, 1]);
                //FindHelper.FindSum012Path([1, 2, 2, 0, 2,0]);
                //FindHelper.FindSumMantissa012Path([2, 0, 0, 1, 2, 0, 2, 2]);
                //FindHelper.FindSpan012Path([0, 2, 1, 0, 0, 0, 2, 0]);

                //FindHelper.FindBigSmall(3, [1,0,1,0,1, 1, 1, 0, 0, 0, 1, 1, 0, 0]);
                //FindHelper.FindOddEven(3, [0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0]);

                //SelectHelper.CaculateDistance5();

                //Forecast.GuessCurrentPeriodLotto();
            }
        }
    }
}
