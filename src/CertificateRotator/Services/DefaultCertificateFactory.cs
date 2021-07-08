using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <inheritdoc />
    public class DefaultCertificateFactory : ICertificateFactory
    {
        /// <inheritdoc />
        public CertificateInfo CreateSigningCertificate(
            string domain,
            HashAlgorithmName hashAlgorithm,
            int days
        )
        {
            using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var distinguishedName = new X500DistinguishedName($"CN={domain}");
            var certificateRequest = new CertificateRequest(distinguishedName, key, hashAlgorithm);
            var usage = new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false);
            certificateRequest.CertificateExtensions.Add(usage);

            var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(days));
            return new CertificateInfo
            {
                Hash = certificate.GetCertHashString(),
                Bytes = certificate.Export(X509ContentType.Pkcs12),
            };
        }
    }
}
