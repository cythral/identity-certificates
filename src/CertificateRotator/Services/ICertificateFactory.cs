using System.Security.Cryptography;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Factory for creating certificates.
    /// </summary>
    public interface ICertificateFactory
    {
        /// <summary>
        /// Creates a new signing certificate.
        /// </summary>
        /// <param name="domain">The distinguished name to use for the certificate.</param>
        /// <param name="keyLength">The key length to use for the certificate.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use for the certificate.</param>
        /// <param name="padding">Padding to use for the certificate.</param>
        /// <param name="days">Number of days for the certificate to be valid for.</param>
        /// <returns>The resulting certificate bytes.</returns>
        CertificateInfo CreateSigningCertificate(
            string domain,
            int keyLength,
            HashAlgorithmName hashAlgorithm,
            RSASignaturePadding padding,
            uint days
        );
    }
}
