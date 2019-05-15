#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.Core.Log;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    public class Client
    {
        public Table.ImageProcessingCommunication ClientTableCommunication { get; set; }
        public async Task RunAsync(CancellationToken cancelationToken)
        {
            Console.WriteLine($"Client RunAsync started".LogInfo());
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1");
            ClientTableCommunication = TableCommunication;
            TableCommunication.Start();

            await TableCommunication.WaitConnectedAsync(cancelationToken);
            Console.WriteLine($"Client Client RunAsync Connected".LogInfo());

            TableCommunication.OnConnect.Add((communication, socket) =>
            {
                Console.WriteLine($"Client Table new connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });
            TableCommunication.OnDisconnect.Add((communication, socket) =>
            {
                Console.WriteLine($"ClientTable removed connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });

            TableCommunication.OnReceived.Add((communication, message) =>
            {
                Console.WriteLine($"Client Table recevied {message?.GetType().Name ?? "unkown"}".LogInfo());
            });

            await Task.Delay(-1, cancelationToken);
            Console.WriteLine($"Client RunAsync finshed".LogInfo());
        }

        public async void SetLights(Table.ImageProcessingCommunication clientTableCommunication)
        {
            #region setlights
            var result = await clientTableCommunication.Send(new Table.Messages.SetLightsRequest
            {
                Top = true,
                Bottom = true,
                SideLeft = false,
                SideMiddle = false,
                SideRight = false
            }).WaitAsync<Table.Messages.SetLightsResponse>();
            if (result.Ok) // No Timeout
            {
                if (result.Value.IsOk())
                    Console.WriteLine("Client SetLight Ok".LogInfo());
                else
                    Console.WriteLine("Client SetLight Server Error".LogError());
            }
            else
                Console.WriteLine("Client SetLight Client TimeOut".LogError());
            #endregion
        }


        public async void TurnTable(Table.ImageProcessingCommunication clientTableCommunication)
        {
            #region turntable
            var mark = clientTableCommunication.Send(new Table.Messages.TurnRelativeRequest
            {
                Angle = 0.0,
                Velocity = 100,
            });

            var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
            if (result.Ok) // No Timeout
            {
                if (result.Value.IsOk())
                    Console.WriteLine("TurnRelative Ok".LogInfo());
                else
                    Console.WriteLine("TurnRelative Server Error".LogError());
            }
            else
                Console.WriteLine("TurnRelative Client TimeOut".LogError());
            #endregion
        }
    }
}
