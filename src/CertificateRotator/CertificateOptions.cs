using System.Security.Cryptography;

using Lambdajection.Attributes;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Options to use for managing certificates.
    /// </summary>
    [LambdaOptions(typeof(Handler), "Certificate")]
    public class CertificateOptions
    {
        /// <summary>
        /// Gets or sets the name of the bucket where certificates live in.
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the distinguished name for certificates.
        /// </summary>
        public string DistinguishedName { get; set; } = "cythral.com";

        /// <summary>
        /// Gets or sets the name of the hash algorithm to use for certificates.
        /// </summary>
        public HashAlgorithmName HashAlgorithmName { get; set; } = HashAlgorithmName.SHA256;

        /// <summary>
        /// Gets or sets the number of days certificates are good for.
        /// </summary>
        public int Lifetime { get; set; } = 60;

        /// <summary>
        /// Gets or sets the configuration parameter name.
        /// </summary>
        public string ConfigurationParameterName { get; set; } = string.Empty;
    }
}
