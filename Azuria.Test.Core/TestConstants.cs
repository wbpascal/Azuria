namespace Azuria.Test.Core
{
    public class TestConstants
    {
        public const string ApiKeyHeaderName = "proxer-api-key";

        public const string DummySuccessResponseString =
            "{\"error\":0,\"message\":\"Abfrage erfolgreich\",\"data\":\"dataValue\"}";

        public const string LoginTokenHeaderName = "proxer-api-token";
        public const string ProxerApiV1Path = "/api/v1";
        public const string ProxerApiV1Url = "https://proxer.me" + ProxerApiV1Path;
    }
}