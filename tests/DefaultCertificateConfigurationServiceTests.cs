using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using AutoFixture.NUnit3;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    public class DefaultCertificateConfigurationServiceTests
    {
        [TestFixture]
        [Category("Unit")]
        public class GetConfigurationTests
        {
            [Test, Auto]
            public async Task ShouldRetrieveConfigurationFromSsmWithActiveCertificateHash(
                string activeCertificateHash,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                getParameterResponse.Parameter.Value = $@"{{""ActiveCertificateHash"":""{activeCertificateHash}""}}";

                var result = await service.GetConfiguration(cancellationToken);

                result.ActiveCertificateHash.Should().Be(activeCertificateHash);
                await ssmClient.Received().GetParameterAsync(Is<GetParameterRequest>(req => req.Name == options.ConfigurationParameterName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldRetrieveConfigurationFromSsmWithInactiveCertificateHash(
                string inactiveCertificateHash,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                getParameterResponse.Parameter.Value = $@"{{""InactiveCertificateHash"":""{inactiveCertificateHash}""}}";

                var result = await service.GetConfiguration(cancellationToken);

                result.InactiveCertificateHash.Should().Be(inactiveCertificateHash);
                await ssmClient.Received().GetParameterAsync(Is<GetParameterRequest>(req => req.Name == options.ConfigurationParameterName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldRetrieveConfigurationFromSsmWithBucketName(
                string bucketName,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                getParameterResponse.Parameter.Value = $@"{{""BucketName"":""{bucketName}""}}";

                var result = await service.GetConfiguration(cancellationToken);

                result.BucketName.Should().Be(bucketName);
                await ssmClient.Received().GetParameterAsync(Is<GetParameterRequest>(req => req.Name == options.ConfigurationParameterName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldReturnDefaultConfigurationObjectWithBucketNameIfParameterIsNull(
                string bucketName,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                options.BucketName = bucketName;
                getParameterResponse.Parameter.Value = $@"null";

                var result = await service.GetConfiguration(cancellationToken);

                result.BucketName.Should().Be(bucketName);
                await ssmClient.Received().GetParameterAsync(Is<GetParameterRequest>(req => req.Name == options.ConfigurationParameterName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldReturnDefaultConfigurationObjectWithBucketNameIfSsmThrows(
                string bucketName,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                options.BucketName = bucketName;
                ssmClient.GetParameterAsync(Any<GetParameterRequest>(), Any<CancellationToken>()).Throws<Exception>();

                var result = await service.GetConfiguration(cancellationToken);

                result.BucketName.Should().Be(bucketName);
                await ssmClient.Received().GetParameterAsync(Is<GetParameterRequest>(req => req.Name == options.ConfigurationParameterName), Is(cancellationToken));
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class UpdateConfigurationTests
        {
            [Test, Auto]
            public async Task ShouldUpdateTheCurrentConfigurationValue(
                string bucketName,
                string activeCertificateHash,
                string inactiveCertificateHash,
                Configuration configuration,
                [Frozen] CertificateOptions options,
                [Frozen] GetParameterResponse getParameterResponse,
                [Frozen] IAmazonSimpleSystemsManagement ssmClient,
                [Target] DefaultCertificateConfigurationService service,
                CancellationToken cancellationToken
            )
            {
                configuration.BucketName = bucketName;
                configuration.ActiveCertificateHash = activeCertificateHash;
                configuration.InactiveCertificateHash = inactiveCertificateHash;

                await service.UpdateConfiguration(configuration, cancellationToken);

                await ssmClient.Received().PutParameterAsync(
                    Is<PutParameterRequest>(req =>
                        req.Name == options.ConfigurationParameterName &&
                        req.Type == ParameterType.String &&
                        req.Overwrite &&
                        req.Value == $@"{{""BucketName"":""{bucketName}"",""ActiveCertificateHash"":""{activeCertificateHash}"",""InactiveCertificateHash"":""{inactiveCertificateHash}""}}"
                    ),
                    Is(cancellationToken)
                );
            }
        }
    }
}
