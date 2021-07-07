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
        private readonly ICertificateStore certificateStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler" /> class.
        /// </summary>
        /// <param name="certificateFactory">Factory to use for creating certificates.</param>
        /// <param name="certificateStore">Place to keep certificates in.</param>
        public Handler(
            ICertificateFactory certificateFactory,
            ICertificateStore certificateStore
        )
        {
            this.certificateFactory = certificateFactory;
            this.certificateStore = certificateStore;
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

            // await certificateStore.AddCertificate(certificate, cancellationToken);
            return "OK";
        }
    }
}
