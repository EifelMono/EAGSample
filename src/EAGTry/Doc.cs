#region usinglog
using ProLog.RowaLog;
using ProLog.Core.Log;
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
    }
}
