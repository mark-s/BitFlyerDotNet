namespace BitFlyerDotNet.LightningApi.Domain
{
    public interface IBitFlyerResponse
    {
        string Json { get; }
        bool IsError { get; }
        bool IsNetworkError { get; }
        bool IsApplicationError { get; }
        string ErrorMessage { get; }
        bool IsUnauthorized { get; }
    }
}