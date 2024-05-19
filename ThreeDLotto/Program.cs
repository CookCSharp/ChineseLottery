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
                //FindHelper.Find012Path(1, [1, 1, 1, 0, 0, 2, 2, 0, 1]);
                //FindHelper.Find012Path(2, [0, 2, 2, 1, 2, 1, 1, 0]);
                //FindHelper.Find012Path(3, [0, 1, 0, 0, 1, 0, 1, 2]);
                //FindHelper.FindMax012Path([1, 2, 0, 0, 0, 1, 1, 0]);
                //FindHelper.FindMin012Path([2, 1, 0, 0, 0, 2, 0]);
                //FindHelper.FindMid012Path([2, 0, 1, 2, 2, 0, 2, 2, 2]);

                //FindHelper.FindBigSmall(1, [1,1,0,1, 0, 1, 1, 0, 0, 1, 0]);
                //FindHelper.FindBigSmall(2, [0,0,0,1,0,1,1,0,0,0,1]);
                //FindHelper.FindBigSmall(3, [1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0]);

                //FindHelper.FindOddEven(1, [1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1]);
                //FindHelper.FindOddEven(2, [1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0]);
                //FindHelper.FindOddEven(3, [1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0]);

                //FindHelper.FindPrimeComposite(1, [1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0]);
                //FindHelper.FindPrimeComposite(2, [0, 0, 0, 1, 0, 1, 0, 1, 0, 1]);
                //FindHelper.FindPrimeComposite(3, [1,1, 1, 1, 0, 0, 0, 1, 1, 0]);

                //FindHelper.FindSumOddEven([0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0,0]);
                //FindHelper.FindSum012Path([0, 0, 1, 1, 2, 1, 0, 1]);

                //FindHelper.FindSpanBigSmall([1, 1, 1, 1, 1, 0, 1, 0, 0, 0]);
                //FindHelper.FindSpanOddEven([1, 0, 0, 0, 0, 1, 1, 1, 1, 1]);
                //FindHelper.FindSpanPrimeComposite([0, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1,0]);
                //FindHelper.FindSpan012Path([2, 1, 1, 2, 1, 0, 1, 1]);

                //FindHelper.FindSumMantissaOddEven([0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1]);
                //FindHelper.FindSumMantissaBigSmall([1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1]);
                //FindHelper.FindSumMantissaPrimeComposite([0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,0]);
                //FindHelper.FindSumMantissa012Path([0, 2, 2, 2, 0, 2, 1, 2, 0]);

                //SelectHelper.CaculateDistance5();
                //SelectHelper.CaculateMiddleNumber([1, 2, 3]);

                Forecast.GuessCurrentPeriodLotto();
            }
        }
    }
}
