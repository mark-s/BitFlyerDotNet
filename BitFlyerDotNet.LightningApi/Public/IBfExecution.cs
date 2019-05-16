using System;

namespace BitFlyerDotNet.LightningApi.Public
{
    public interface IBfExecution
    {
        int ExecutionId { get; }
        BfTradeSide Side { get; }
        decimal Price { get; }
        decimal Size { get; }
        DateTime ExecutedTime { get; }
        string ChildOrderAcceptanceId { get; }
    }
}