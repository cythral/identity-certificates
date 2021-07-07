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
        private readonly ICertificateFactory certificateFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler" /> class.
        /// </summary>
        /// <param name="certificateFactory">Factory to use for creating certificates.</param>
        public Handler(
            ICertificateFactory certificateFactory
        )
        {
            this.certificateFactory = certificateFactory;
        }

        /// <summary>
        /// Handles rotating certificates.
        /// </summary>
        /// <param name="request">The request to rotate certificates.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        public async Task<string> Handle(RotateCertificateRequest request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            // var certificate = certificateFactory.CreateSigningCertificate(
            //     domain: "cythral.com",
            //     keyLength: 4096,
            //     hashAlgorithm: HashAlgorithmName.SHA512,
            //     padding: RSASignaturePadding.Pkcs1,
            //     days: 90
            // );
            return "OK";
        }
    }
}
