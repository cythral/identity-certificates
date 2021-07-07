using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <inheritdoc />
    public class DefaultCertificateConfigurationService : ICertificateConfigurationService
    {
        private readonly IAmazonSimpleSystemsManagement ssmClient;
        private readonly CertificateOptions options;
        private readonly ILogger<DefaultCertificateConfigurationService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCertificateConfigurationService" /> class.
        /// </summary>
        /// <param name="ssmClient">Client for AWS SSM.</param>
        /// <param name="options">Options to use when dealing with certificates.</param>
        /// <param name="logger">Logger used to log info to some destination(s).</param>
        public DefaultCertificateConfigurationService(
            IAmazonSimpleSystemsManagement ssmClient,
            IOptions<CertificateOptions> options,
            ILogger<DefaultCertificateConfigurationService> logger
        )
        {
            this.ssmClient = ssmClient;
            this.options = options.Value;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<Configuration> GetConfiguration(CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new GetParameterRequest { Name = options.ConfigurationParameterName };
                logger.LogInformation("Sending ssm:GetParameter request: {@request}", request);

                var response = await ssmClient.GetParameterAsync(request, cancellationToken);
                logger.LogInformation("Received ssm:GetParameter response: {@response}", response);

                var configuration = JsonSerializer.Deserialize<Configuration>(response.Parameter.Value);
                return configuration ?? new Configuration { BucketName = options.BucketName };
            }
            catch (Exception exception)
            {
                logger.LogError("Got error retrieving configuration parameter from SSM.", exception);
                return new Configuration { BucketName = options.BucketName };
            }
        }

        /// <inheritdoc />
        public async Task UpdateConfiguration(Configuration configuration, CancellationToken cancellationToken = default)
        {
            var request = new PutParameterRequest
            {
                Name = options.ConfigurationParameterName,
                Type = ParameterType.String,
                Overwrite = true,
                Value = JsonSerializer.Serialize(configuration),
            };

            logger.LogInformation("Sending ssm:PutParameter with request: {@request}", request);
            var response = await ssmClient.PutParameterAsync(request, cancellationToken);
            logger.LogInformation("Received ssm:PutParameter response: {@response}", response);
        }
    }
}
