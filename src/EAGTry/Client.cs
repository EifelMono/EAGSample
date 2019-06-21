#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0060 // Remove unused parameter
using System;
using System.Threading.Tasks;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    public static class Client
    {
        public static async Task<int> WaitAsync()
        {
            #region wait
            Console.WriteLine(DateTime.Now);
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine(DateTime.Now);
            #endregion
            return 0;
        }
        public static async Task<int> SetLightsAsync()
        {
            #region setlights
            using var TableCommunication = new Table.ImageProcessingCommunication(60002, "127.0.0.1").DoRun();
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
                Console.WriteLine("message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EAGTry\EAGTry");
            #endregion
            return 0;
        }

        public static async Task<int> TurnTableAsync()
        {
            #region turntable
            using var TableCommunication = new Table.ImageProcessingCommunication(60002, "127.0.0.1").DoRun();
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
                Console.WriteLine("message Ok");
            else
                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EAGTry\EAGTry");
            #endregion
            return 0;
        }
    }
}
