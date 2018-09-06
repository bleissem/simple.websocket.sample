using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simple.websocket.sample.Common
{
    public class Settings: ISettings
    {
        public Settings(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        private IConfiguration _Configuration;

        public int KeepAliveInterval
        {
            get
            {
                return _Configuration.GetSection("WebsocketSettings").GetValue<int>("KeepAliveInterval");
            }
        }

        public int ReceiveBufferSize
        {
            get
            {
                return _Configuration.GetSection("WebsocketSettings").GetValue<int>("ReceiveBufferSize");
            }
        }
    }
}
