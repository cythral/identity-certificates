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
            services.AddSingleton<ICertificateFactory, DefaultCertificateFactory>();
        }
    }
}
