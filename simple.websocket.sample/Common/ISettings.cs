using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simple.websocket.sample.Common
{
    public interface ISettings
    {
        int KeepAliveInterval { get; }

        int ReceiveBufferSize { get; }
    }
}
