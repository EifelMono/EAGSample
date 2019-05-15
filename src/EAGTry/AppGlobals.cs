using System;
using System.Collections.Generic;
using System.Text;

namespace EAGTry
{
    public static class AppGlobals
    {
        public static class Server
        {
            public static int WaitSetLightsInSeconds { get; set; } = 0;
            public static int WaitTurnTableInSeconds { get; set; } = 2;
        }
    }
}
