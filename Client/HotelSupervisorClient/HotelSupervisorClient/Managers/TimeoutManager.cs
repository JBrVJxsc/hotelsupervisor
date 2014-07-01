using System;
using System.Threading;

namespace HotelSupervisorClient.Managers
{
    internal class TimeoutManager
    {
        private ManualResetEvent mTimeoutObject;
        private bool mBoTimeout;
        public DoHandler Do;

        public TimeoutManager()
        {
            mTimeoutObject = new ManualResetEvent(true);
        }

        public bool DoWithTimeout(TimeSpan timeSpan)
        {
            if (Do == null)
            {
                return false;
            }
            mTimeoutObject.Reset();
            mBoTimeout = true;
            Do.BeginInvoke(DoAsyncCallBack, null);
            if (!mTimeoutObject.WaitOne(timeSpan, false))
            {
                mBoTimeout = true;
            }
            return mBoTimeout;
        }

        private void DoAsyncCallBack(IAsyncResult result)
        {
            try
            {
                Do.EndInvoke(result);
                mBoTimeout = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mBoTimeout = true;
            }
            finally
            {
                mTimeoutObject.Set();
            }
        }
    }

    public delegate void DoHandler();
}
