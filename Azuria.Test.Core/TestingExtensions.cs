namespace Azuria.Test.Core
{
    public static class TestingExtensions
    {
        #region Methods

        public static ProxerClientOptions WithTestingClient(this ProxerClientOptions options)
        {
            return options.WithCustomHttpClient(new TestingHttpClient(options.Client));
        }

        #endregion
    }
}