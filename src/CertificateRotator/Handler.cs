using System.Threading;
using System.Threading.Tasks;

using Lambdajection.Attributes;

#pragma warning disable IDE0060
#pragma warning disable CA1822

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <summary>
    /// Handles rotating a certificate.
    /// </summary>
    [Lambda(typeof(Startup))]
    public partial class Handler
    {
        /// <summary>
        /// Handles rotating certificates.
        /// </summary>
        /// <param name="request">The request to rotate certificates.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        public async Task<string> Handle(RotateCertificateRequest request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return "OK";
        }
    }
}
