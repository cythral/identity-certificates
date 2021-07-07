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
            int keyLength,
            HashAlgorithmName hashAlgorithm,
            RSASignaturePadding padding,
            uint days
        )
        {
            using var rsa = RSA.Create(keyLength);
            var distinguishedName = new X500DistinguishedName($"CN={domain}");
            var certificateRequest = new CertificateRequest(distinguishedName, rsa, hashAlgorithm, padding);
            var usage = new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false);
            certificateRequest.CertificateExtensions.Add(usage);

            var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.UtcNow, new DateTimeOffset(DateTime.UtcNow.AddDays(days)));
            return new CertificateInfo
            {
                Hash = certificate.GetCertHashString(),
                Bytes = certificate.Export(X509ContentType.Pkcs12),
            };
        }
    }
}
