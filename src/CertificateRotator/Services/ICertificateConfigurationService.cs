using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Service to manage the active certificate configuration.
    /// </summary>
    public interface ICertificateConfigurationService
    {
        /// <summary>
        /// Gets the active certificate configuration.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The active configuration.</returns>
        Task<Configuration> GetConfiguration(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the active configuration values.
        /// </summary>
        /// <param name="configuration">The new values to use for the configuration.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        Task UpdateConfiguration(Configuration configuration, CancellationToken cancellationToken = default);
    }
}
