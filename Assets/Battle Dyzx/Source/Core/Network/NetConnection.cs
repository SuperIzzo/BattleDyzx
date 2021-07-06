using System.Net;

namespace BattleDyzx
{
    public class NetConnection
    {
        public IPEndPoint address { get; private set; }
        public NetDriver driver { get; private set; }
    }
}