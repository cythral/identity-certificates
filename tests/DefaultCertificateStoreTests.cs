using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using AutoFixture.NUnit3;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    public class DefaultCertificateStoreTests
    {
        [TestFixture]
        [Category("Unit")]
        public class AddCertificateTests
        {
            [Test, Auto]
            public async Task ShouldSaveCertificateToS3WithBucketName(
                CertificateInfo certificate,
                [Frozen] CertificateOptions options,
                [Frozen] IAmazonS3 s3Client,
                [Target] DefaultCertificateStore store,
                CancellationToken cancellationToken
            )
            {
                await store.AddCertificate(certificate, cancellationToken);

                await s3Client.Received().PutObjectAsync(Is<PutObjectRequest>(req => req.BucketName == options.BucketName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldSaveCertificateToS3WithHashAsKey(
                CertificateInfo certificate,
                [Frozen] IAmazonS3 s3Client,
                [Target] DefaultCertificateStore store,
                CancellationToken cancellationToken
            )
            {
                await store.AddCertificate(certificate, cancellationToken);

                await s3Client.Received().PutObjectAsync(Is<PutObjectRequest>(req => req.Key == certificate.Hash), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldSaveCertificateToS3WithBytesAsBody(
                CertificateInfo certificate,
                [Frozen] IAmazonS3 s3Client,
                [Target] DefaultCertificateStore store,
                CancellationToken cancellationToken
            )
            {
                var givenInputBytes = Array.Empty<byte>();
                s3Client.PutObjectAsync(Any<PutObjectRequest>(), Any<CancellationToken>()).Returns(x =>
                {
                    using var destinationStream = new MemoryStream();
                    x.ArgAt<PutObjectRequest>(0).InputStream.CopyTo(destinationStream);
                    givenInputBytes = destinationStream.ToArray();
                    return new PutObjectResponse();
                });

                await store.AddCertificate(certificate, cancellationToken);

                givenInputBytes.Should().BeEquivalentTo(certificate.Bytes);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class ListActiveCertificatesByHash
        {
            [Test, Auto]
            public async Task ShouldSendDeleteObjectRequestWithBucketName(
                string hash,
                [Frozen] CertificateOptions options,
                [Frozen] IAmazonS3 s3Client,
                [Target] DefaultCertificateStore store,
                CancellationToken cancellationToken
            )
            {
                await store.RemoveCertificate(hash, cancellationToken);

                await s3Client.Received().DeleteObjectAsync(Is<DeleteObjectRequest>(req => req.BucketName == options.BucketName), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldSendDeleteObjectRequestWithHashAsKey(
                string hash,
                [Frozen] CertificateOptions options,
                [Frozen] IAmazonS3 s3Client,
                [Target] DefaultCertificateStore store,
                CancellationToken cancellationToken
            )
            {
                await store.RemoveCertificate(hash, cancellationToken);

                await s3Client.Received().DeleteObjectAsync(Is<DeleteObjectRequest>(req => req.Key == hash), Is(cancellationToken));
            }
        }
    }
}
