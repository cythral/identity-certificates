using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Represents a place where certificates are kept.
    /// </summary>
    public interface ICertificateStore
    {
        /// <summary>
        /// Add a certificate to the store.
        /// </summary>
        /// <param name="certificate">The certificate to add to the store.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        Task AddCertificate(CertificateInfo certificate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of active certificate by their hash.
        /// </summary>
        /// <param name="hash">The hash of the certificate to remove.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of active certificate hashes.</returns>
        Task RemoveCertificate(string hash, CancellationToken cancellationToken = default);
    }
}
