using System.Threading;
using System.Threading.Tasks;

using Lambdajection.Attributes;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        private readonly CertificateOptions certificateOptions;
        private readonly ILogger<Handler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler" /> class.
        /// </summary>
        /// <param name="certificateFactory">Factory to use for creating certificates.</param>
        /// <param name="certificateStore">Place to keep certificates in.</param>
        /// <param name="certificateConfigService">Service to manage certificate configuration.</param>
        /// <param name="certificateOptions">Options to use when dealing with certificates.</param>
        /// <param name="logger">Logger used to log info to some destination(s).</param>
        public Handler(
            ICertificateFactory certificateFactory,
            ICertificateStore certificateStore,
            ICertificateConfigurationService certificateConfigService,
            IOptions<CertificateOptions> certificateOptions,
            ILogger<Handler> logger
        )
        {
            this.certificateFactory = certificateFactory;
            this.certificateStore = certificateStore;
            this.certificateConfigService = certificateConfigService;
            this.certificateOptions = certificateOptions.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Handles rotating certificates.
        /// </summary>
        /// <param name="request">The request to rotate certificates.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting task.</returns>
        public async Task<string> Handle(RotateCertificateRequest request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Received request: {@request}", request);

            var configuration = await certificateConfigService.GetConfiguration(cancellationToken);
            var inactiveCertificate = configuration.InactiveCertificateHash;
            var newCertificate = certificateFactory.CreateSigningCertificate(
                domain: certificateOptions.DistinguishedName,
                hashAlgorithm: certificateOptions.HashAlgorithmName,
                days: certificateOptions.Lifetime
            );

            await certificateStore.AddCertificate(newCertificate, cancellationToken);

            configuration.InactiveCertificateHash = configuration.ActiveCertificateHash;
            configuration.ActiveCertificateHash = newCertificate.Hash;
            await certificateConfigService.UpdateConfiguration(configuration, cancellationToken);

            if (inactiveCertificate != null)
            {
                await certificateStore.RemoveCertificate(inactiveCertificate!, cancellationToken);
            }

            return "OK";
        }
    }
}
