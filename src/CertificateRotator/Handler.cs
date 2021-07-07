using System.Security.Cryptography;
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
        private readonly ICertificateConfigurationService certificateConfigService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler" /> class.
        /// </summary>
        /// <param name="certificateFactory">Factory to use for creating certificates.</param>
        /// <param name="certificateStore">Place to keep certificates in.</param>
        /// <param name="certificateConfigService">Service to manage certificate configuration.</param>
        public Handler(
            ICertificateFactory certificateFactory,
            ICertificateStore certificateStore,
            ICertificateConfigurationService certificateConfigService
        )
        {
            this.certificateFactory = certificateFactory;
            this.certificateStore = certificateStore;
            this.certificateConfigService = certificateConfigService;
        }

        /// <summary>
        /// Handles rotating certificates.
        /// </summary>
        /// <param name="request">The request to rotate certificates.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        public async Task<string> Handle(RotateCertificateRequest request, CancellationToken cancellationToken = default)
        {
            var configuration = await certificateConfigService.GetConfiguration(cancellationToken);
            var inactiveCertificate = configuration.InactiveCertificateHash;
            var newCertificate = certificateFactory.CreateSigningCertificate(
                domain: "cythral.com",
                keyLength: 4096,
                hashAlgorithm: HashAlgorithmName.SHA512,
                padding: RSASignaturePadding.Pkcs1,
                days: 90
            );

            await certificateStore.AddCertificate(newCertificate, cancellationToken);

            configuration.InactiveCertificateHash = configuration.ActiveCertificateHash;
            configuration.ActiveCertificateHash = newCertificate.Hash;
            await certificateConfigService.UpdateConfiguration(configuration, cancellationToken);

            if (inactiveCertificate != null)
            {
                await certificateStore.RemoveCertificate(inactiveCertificate, cancellationToken);
            }

            return "OK";
        }
    }
}
