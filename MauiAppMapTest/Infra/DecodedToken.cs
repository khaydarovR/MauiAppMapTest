
namespace MauiAppMapTest.Services;

public record DecodedToken(string keyId,
					   string issuer,
					   List<string> audience,
					   List<(string Type, string Value)> claims,
					   DateTime validTo,
					   string signatureAlgorithm,
					   string rawData,
					   string subject,
					   DateTime validFrom,
					   string encodedHeader,
					   string encodedPayload,
					   string roles,
					   Guid id,
					   string email);