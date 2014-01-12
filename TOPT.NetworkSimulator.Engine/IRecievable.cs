using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    interface IReceivable
    {
        void ReceivePacket(Packet packet);
    }
}
