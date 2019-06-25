#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0067 // Dispose objects before losing scope
using System;
using System.Threading;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.Core.Log;
using ProLog.RowaLog;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    class Program
    {
        ///<param name="region">Takes in the --region option from the code fence options in markdown</param>
        ///<param name="session">Takes in the --session option from the code fence options in markdown</param>
        ///<param name="package">Takes in the --package option from the code fence options in markdown</param>
        ///<param name="project">Takes in the --project option from the code fence options in markdown</param>
        ///<param name="args">Takes in any additional arguments passed in the code fence options in markdown</param>
        ///<see>To learn more see <a href="https://aka.ms/learntdn">our documentation</a></see>
        static async Task<int> Main(
             string region = null,
             string session = null,
             string package = null,
             string project = null,
             string[] args = null)
        {
            await Task.Delay(1);
            using var rowaLogProxy = new RowaLogProxy(Globals.App.Name, false);

            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => cancellationTokenSource.Cancel();

            return region switch
            {
                "eagtrytest" => TryTest(),
                "setlights" => await Client.SetLightsAsync(),
                "rowalogzipcleanup" => await Client.RowaLogZipCleanUpAsync(),
                "turntable" => await Client.TurnTableAsync(),
                "turntableerror" => await Client.TurnTableErrorAsync(),
                _ => throw new ArgumentException("A --region argument must be passed", nameof(region))
            };
        }

        public static int TryTest()
        {
            #region eagtrytest
            Console.WriteLine("Hello EagTry");
            #endregion
            return 0;
        }
    }
}
