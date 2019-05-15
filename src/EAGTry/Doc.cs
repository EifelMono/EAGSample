#region usinglog
using ProLog.RowaLog;
using ProLog.Core.Log;
#endregion

#region usingcommunication
using Position3D = ProLog3.Communication.ImageProcessing.Position3D;
using Size3D = ProLog3.Communication.ImageProcessing.Size3D;
using Table = ProLog3.Communication.ImageProcessing.Table;
using Ocr = ProLog3.Communication.ImageProcessing.Ocr;
#endregion

using System;
using ProLog.Core;

namespace EAGTry
{
    class Doc
    {

        public void VoidLog()
        {
            #region voidlog
            // RowaLogProxy("Product", "Component")
            // C:\ProgramData\Rowa\Protocol\Product\Component
            var rowaLogProxy = new RowaLogProxy(Globals.App.Name);

            try
            {
                "Info".LogInfo();
                "Error".LogError();
                "Debug".LogDebug();
                "Audit".LogAudit();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            rowaLogProxy.Dispose(); // Ist Notwendig sonst wird ein Thread in RowaLog nicht beendet
            #endregion
        }

        public void voidCommunication()
        {
            #region createcommunication 
            var port = 4711;
            var host = "127.0.0.1";
            // host is optional, 
            // if host is null or empty the communication is running as a server
            var Position3DCommunication = new Position3D.ImageProcessingCommunication(port, host);
            var Size3DCommunication = new Size3D.ImageProcessingCommunication(port, host);
            var TableCommunication = new Table.ImageProcessingCommunication(port, host);
            var OcrCommunication = new Ocr.ImageProcessingCommunication(port, host);
            #endregion
        }
    }
}
