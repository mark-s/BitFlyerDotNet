﻿using System.Net.Sockets;

namespace BitFlyerDotNet.LightningApi.Domain
{
    public class ErrorStatus
    {
        public SocketError SocketError { get; set; } = SocketError.Success;
        public string Message { get; set; }
    }
}
