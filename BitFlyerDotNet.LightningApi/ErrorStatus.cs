//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System.Net.Sockets;

namespace BitFlyerDotNet.LightningApi
{
    public partial class RealtimeSourceFactory
    {
        public class ErrorStatus
        {
            public SocketError SocketError { get; set; } = SocketError.Success;
            public string Message { get; set; }
        }
    }
}
