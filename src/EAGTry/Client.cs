#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0060 // Remove unused parameter
using System;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core.Log;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    class Client
    {

        public async Task<int> SetLights1Async()
        {
            #region setlights1
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
                return 0;
            var result = await TableCommunication.Send(new Table.Messages.SetLightsRequest
            {
                Top = true,
                Bottom = true,
                SideLeft = false,
                SideMiddle = false,
                SideRight = false
            }).WaitAsync<Table.Messages.SetLightsResponse>();
            if (result.IsOk()) // No Timeout
            {
                if (result.Message.IsOk())
                    Console.WriteLine("Client SetLight Ok".LogInfo());
                else
                    Console.WriteLine("Client SetLight Server Error".LogError());
            }
            else
                Console.WriteLine("Client SetLight Client TimeOut".LogError());
            #endregion
            return 0;
        }

        public async Task<int> SetLights2Async()
        {
            #region setlights2
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
                return 0;
            if (await TableCommunication.SendAndWaitAsync<Table.Messages.SetLightsResponse>(new Table.Messages.SetLightsRequest
            {
                Top = true,
                Bottom = true,
                SideLeft = false,
                SideMiddle = false,
                SideRight = false
            }) is var result && result.IsMessageOk())
                Console.WriteLine("message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
            #endregion
            return 0;
        }

        public async Task<int> TurnTable1Async()
        {
            #region turntable1
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
                return 0;

            var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest
            {
                Angle = 0.0,
                Velocity = 100,
            });

            var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
            if (result.IsOk()) // No Timeout
            {
                if (result.Message.IsOk())
                    Console.WriteLine("TurnRelative Ok".LogInfo());
                else
                    Console.WriteLine("TurnRelative Server Error".LogError());
            }
            else
                Console.WriteLine("TurnRelative Client TimeOut".LogError());
            #endregion
            return 0;
        }

        public async Task<int> TurnTable2Async()
        {
            #region turntable2
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
            if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
                return 0;

            if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>(
                                new Table.Messages.TurnRelativeRequest
                                {
                                    Angle = 0.0,
                                    Velocity = 0,
                                }) is var result && result.IsMessageOk())
                Console.WriteLine("message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
            #endregion
            return 0;
        }
    }
}
