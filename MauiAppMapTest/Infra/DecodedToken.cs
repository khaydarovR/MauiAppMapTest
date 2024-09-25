
namespace MauiAppMapTest.Services
{
    internal class DecodedToken
    {
        private string keyId;
        private string issuer;
        private List<string> audience;
        private List<(string Type, string Value)> claims;
        private DateTime validTo;
        private string signatureAlgorithm;
        private string rawData;
        private string subject;
        private DateTime validFrom;
        private string encodedHeader;
        private string encodedPayload;

        public DecodedToken(string keyId, string issuer, List<string> audience, List<(string Type, string Value)> claims, DateTime validTo, string signatureAlgorithm, string rawData, string subject, DateTime validFrom, string encodedHeader, string encodedPayload)
        {
            this.keyId = keyId;
            this.issuer = issuer;
            this.audience = audience;
            this.claims = claims;
            this.validTo = validTo;
            this.signatureAlgorithm = signatureAlgorithm;
            this.rawData = rawData;
            this.subject = subject;
            this.validFrom = validFrom;
            this.encodedHeader = encodedHeader;
            this.encodedPayload = encodedPayload;
        }
    }
}