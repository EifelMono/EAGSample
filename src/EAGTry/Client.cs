#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0060 // Remove unused parameter
using System;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    public static class Client
    {
        public static async Task<int> SetLightsAsync()
        {
            #region setlights
            using var TableCommunication = new Table.ImageProcessingCommunication(6002, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
            {
                Console.WriteLine("Not connected");
                return 0;
            }
            Console.WriteLine("Connected");
            if (await TableCommunication.SendAndWaitAsync<Table.Messages.SetLightsResponse>
                (new Table.Messages.SetLightsRequest
                {
                    Top = true,
                    Bottom = true,
                    SideLeft = false,
                    SideMiddle = false,
                    SideRight = false
                }, TimeSpan.FromSeconds(1)) is var result && result.IsMessageOk())
                Console.WriteLine("Message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EAGTry\EAGTry");
            #endregion
            return 0;
        }

        public static async Task<int> TurnTableAsync()
        {
            #region turntable
            using var TableCommunication = new Table.ImageProcessingCommunication(6002, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
            {
                Console.WriteLine("Not connected");
                return 0;
            }
            Console.WriteLine("Connected");
            if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>(
                new Table.Messages.TurnRelativeRequest
                {
                    Angle = 30.0,
                    Velocity = 100,
                }, TimeSpan.FromSeconds(5)) is var result && result.IsMessageOk())
                Console.WriteLine("Message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EAGTry\EAGTry");
            #endregion
            return 0;
        }

        public static async Task<int> TurnTableErrorAsync()
        {
            #region turntableerror
            using var TableCommunication = new Table.ImageProcessingCommunication(6002, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
            {
                Console.WriteLine("Not connected");
                return 0;
            }
            Console.WriteLine("Connected");
            if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>(
                new Table.Messages.TurnRelativeRequest
                {
                    Angle = 30.0,
                    Velocity = 0,
                }, TimeSpan.FromSeconds(5)) is var result)
                if (result.IsOk()) // Send/Receive Ok
                {
                    Console.WriteLine("Send/Receive OK");
                    if (result.Message.IsOk())
                        Console.WriteLine("Message Ok");
                    else
                    {
                        Console.WriteLine("Messge not ok");
                        Console.WriteLine(result.Message?.Result?.MessagesAsString?? "");
                    }
                }
                else
                {
                    Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EAGTry\EAGTry");
                    Console.WriteLine(result.Exception?.Message?? "");
                }
            #endregion
            return 0;
        }
    }
}
