namespace Azuria.Test.Core.Utility
{
    public static class TestingExtensions
    {
        #region Methods

        public static ProxerClientOptions WithTestingHttpClient(this ProxerClientOptions options)
        {
            return options.WithCustomHttpClient(ResponseSetup.GetTestingClient(options.Client));
        }

        #endregion
    }
}