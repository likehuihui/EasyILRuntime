using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;

namespace MessagePackage
{
    public abstract class DelegateMsgBase : INetEvent
    {
        public abstract bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer);

        public virtual void Error(int status, string errStr) { }
    }
}
