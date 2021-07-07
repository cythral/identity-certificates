using Amazon.S3;

using Lambdajection.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Brighid.Identity.Certificates.CertificateRotator
{
    /// <inheritdoc />
    public class Startup : ILambdaStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseAwsService<IAmazonS3>();
            services.AddSingleton<ICertificateFactory, DefaultCertificateFactory>();
            services.AddSingleton<ICertificateStore, DefaultCertificateStore>();
        }
    }
}
