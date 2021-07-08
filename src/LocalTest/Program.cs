using System.Threading.Tasks;

using Brighid.Identity.Certificates.CertificateRotator;

#pragma warning disable CS1591
#pragma warning disable SA1600
#pragma warning disable IDE0060

namespace Brighid.Identity.Certificates.LocalTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Handler.Run(new CertificateRotator.RotateCertificateRequest(), null!);
        }
    }
}
