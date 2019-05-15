#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0060 // Remove unused parameter
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
        static async Task<int> Main(string region = null,
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
                "setlights" => await new Client().SetLightsAsync(),
                "turntable" => await new Client().TurnTableAsync(),
                _ => 0
            };
        }
    }
}
