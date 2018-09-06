using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace simple.websocket.sample.Common
{
    public class WebSocketEvent
    {
        public WebSocketEvent()
        {

        }

        public Guid SenderId { get; set; }

        public ArraySegment<byte> Buffer { get; set; }
        public WebSocketMessageType MessageType { get; set; }
        public bool EndOfMessage { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
