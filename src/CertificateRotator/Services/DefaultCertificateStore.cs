using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <inheritdoc />
    public class DefaultCertificateStore : ICertificateStore
    {
        private readonly IAmazonS3 s3Client;
        private readonly CertificateOptions options;
        private readonly ILogger<DefaultCertificateStore> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCertificateStore" /> class.
        /// </summary>
        /// <param name="s3Client">Client to use for Amazon S3.</param>
        /// <param name="options">Options when dealing with certificates.</param>
        /// <param name="logger">The logger to use.</param>
        public DefaultCertificateStore(
            IAmazonS3 s3Client,
            IOptions<CertificateOptions> options,
            ILogger<DefaultCertificateStore> logger
        )
        {
            this.s3Client = s3Client;
            this.options = options.Value;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task AddCertificate(CertificateInfo certificate, CancellationToken cancellationToken = default)
        {
            var request = new PutObjectRequest
            {
                BucketName = options.BucketName,
                Key = certificate.Hash,
                InputStream = new MemoryStream(certificate.Bytes),
            };

            logger.LogInformation("Sending s3:PutObject request: {@request}", request);
            var response = await s3Client.PutObjectAsync(request, cancellationToken);
            logger.LogInformation("Received s3:PutObject response: {@response}", response);
        }

        /// <inheritdoc />
        public async Task RemoveCertificate(string hash, CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = options.BucketName,
                Key = hash,
            };

            logger.LogInformation("Sending s3:DeleteObject request: {@request}", request);
            var response = await s3Client.DeleteObjectAsync(request, cancellationToken);
            logger.LogInformation("Received s3:DeleteObject response: {@response}", response);
        }
    }
}
