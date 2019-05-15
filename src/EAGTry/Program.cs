#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Threading;
using System.Threading.Tasks;
using ProLog.Core;
using ProLog.RowaLog;

namespace EAGTry
{
    class Program
    {
        static async Task Main(string region = null,
             string session = null,
             string package = null,
             string project = null,
             string[] args = null)
        {
            using var rowaLogProxy = new RowaLogProxy(Globals.App.Name, false);

            var  cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => cancellationTokenSource.Cancel();

            var server = new Server();
            var serverTask= server.RunAsync(cancellationTokenSource.Token);
            var client = new Client();
            var clientTask= client.RunAsync(cancellationTokenSource.Token);

            switch (region)
            {
                case "setlights":
                    client.SetLights(client.ClientTableCommunication);
                    break;
                case "turntable":
                    client.TurnTable(client.ClientTableCommunication);
                    break;
            }
            await Task.WhenAll(serverTask, clientTask);
        }
    }
}
