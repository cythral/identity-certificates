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
    }
}
