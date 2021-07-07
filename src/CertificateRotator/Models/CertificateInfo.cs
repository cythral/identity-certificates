using System;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Encapsulates certain information about a certificate.
    /// </summary>
    public class CertificateInfo
    {
        /// <summary>
        /// Gets or sets the certificate hash.
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the certificate bytes.
        /// </summary>
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
    }
}
