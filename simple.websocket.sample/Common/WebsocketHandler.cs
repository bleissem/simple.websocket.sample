using PubSub.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace simple.websocket.sample.Common
{
    public class WebsocketHandler : IDisposable
    {


        public WebsocketHandler(WebSocket webSocket, ISettings settings)
        {
            this.Id = Guid.NewGuid();
            _Cancel = new CancellationTokenSource();
            _WebSocket = webSocket;
            _Settings = settings;
        }

        ~WebsocketHandler()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        private Guid Id;

        private CancellationTokenSource _Cancel;

        private WebSocket _WebSocket;

        private ISettings _Settings;

        public void Cancel()
        {
            _Cancel.Cancel();
        }

        public async Task Run()
        {
            if (_WebSocket == null) return;

            this.Subscribe<WebSocketEvent>(this.OnWebSocketEvent);

            var buffer = new byte[_Settings.ReceiveBufferSize];
            WebSocketReceiveResult result = await _WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _Cancel.Token);
            while (!result.CloseStatus.HasValue)
            {
                this.Publish<WebSocketEvent>(new WebSocketEvent()
                {
                    SenderId = this.Id,
                    Buffer = new ArraySegment<byte>(buffer, 0, result.Count),
                    MessageType = result.MessageType,
                    EndOfMessage = result.EndOfMessage,
                    CancellationToken = _Cancel.Token,
                });

                result = await _WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _Cancel.Token);
            }
            await _WebSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, _Cancel.Token);

            this.Unsubscribe<WebSocketEvent>();
        }

        private async void OnWebSocketEvent(WebSocketEvent webSocketEvent)
        {
            if (webSocketEvent.SenderId == this.Id) return;
            await _WebSocket?.SendAsync(webSocketEvent.Buffer, webSocketEvent.MessageType, webSocketEvent.EndOfMessage, webSocketEvent.CancellationToken);
        }

        private void Dispose(bool disposing)
        {
            _Cancel?.Cancel();
            _Cancel?.Dispose();
            _Cancel = null;

            _WebSocket?.Dispose();
            _WebSocket = null;

            _Settings = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
