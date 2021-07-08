using System.Threading;
using System.Threading.Tasks;

using AutoFixture.NUnit3;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    public class HandlerTests
    {
        [TestFixture]
        [Category("Unit")]
        public class HandleTests
        {
            [Test, Auto]
            public async Task ShouldFetchTheConfiguration(
                RotateCertificateRequest request,
                [Frozen] ICertificateConfigurationService configService,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                await handler.Handle(request, cancellationToken);

                await configService.Received().GetConfiguration(Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldCreateANewCertificate(
                RotateCertificateRequest request,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateFactory certificateFactory,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                await handler.Handle(request, cancellationToken);

                certificateFactory.Received().CreateSigningCertificate(Is(options.DistinguishedName), Is(options.HashAlgorithmName), Is(options.Lifetime));
            }

            [Test, Auto]
            public async Task ShouldAddTheNewCertificateToTheStore(
                RotateCertificateRequest request,
                [Frozen] CertificateInfo certificateInfo,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateStore certificateStore,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                await handler.Handle(request, cancellationToken);

                await certificateStore.Received().AddCertificate(Is(certificateInfo), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldUpdateTheConfigurationWithInactiveCertificateSetToTheActiveCertificate(
                RotateCertificateRequest request,
                string activeCertificateHash,
                string inactiveCertificateHash,
                [Frozen] Configuration configuration,
                [Frozen] CertificateInfo certificateInfo,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateConfigurationService configService,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                configuration.ActiveCertificateHash = activeCertificateHash;
                configuration.InactiveCertificateHash = inactiveCertificateHash;

                await handler.Handle(request, cancellationToken);

                await configService.Received().UpdateConfiguration(Is<Configuration>(config => config.InactiveCertificateHash == activeCertificateHash), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldUpdateTheConfigurationWithActiveCertificateHashSetToNewCertificateHash(
                RotateCertificateRequest request,
                string activeCertificateHash,
                string inactiveCertificateHash,
                string newCertificateHash,
                [Frozen] Configuration configuration,
                [Frozen] CertificateInfo certificateInfo,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateConfigurationService configService,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                certificateInfo.Hash = newCertificateHash;
                configuration.ActiveCertificateHash = activeCertificateHash;
                configuration.InactiveCertificateHash = inactiveCertificateHash;

                await handler.Handle(request, cancellationToken);

                await configService.Received().UpdateConfiguration(Is<Configuration>(config => config.ActiveCertificateHash == newCertificateHash), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldRemoveTheOldInactiveCertificate(
                RotateCertificateRequest request,
                string inactiveCertificate,
                [Frozen] Configuration configuration,
                [Frozen] CertificateInfo certificateInfo,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateStore certificateStore,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                configuration.InactiveCertificateHash = inactiveCertificate;

                await handler.Handle(request, cancellationToken);

                await certificateStore.Received().RemoveCertificate(Is(inactiveCertificate), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldNotRemoveTheOldInactiveCertificateIfItsNull(
                RotateCertificateRequest request,
                string inactiveCertificate,
                [Frozen] Configuration configuration,
                [Frozen] CertificateInfo certificateInfo,
                [Frozen] CertificateOptions options,
                [Frozen] ICertificateStore certificateStore,
                [Target] Handler handler,
                CancellationToken cancellationToken
            )
            {
                configuration.InactiveCertificateHash = null;

                await handler.Handle(request, cancellationToken);

                await certificateStore.DidNotReceive().RemoveCertificate(Any<string>(), Is(cancellationToken));
            }
        }
    }
}
