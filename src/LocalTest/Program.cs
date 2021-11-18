using System.IO;
using System.Threading.Tasks;

using Brighid.Identity.Certificates.CertificateRotator;

using JsonSerializer = System.Text.Json.JsonSerializer;

#pragma warning disable CS1591
#pragma warning disable SA1600
#pragma warning disable IDE0060

namespace Brighid.Identity.Certificates.LocalTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, new RotateCertificateRequest());
            stream.Position = 0;
            await Handler.Run(stream, null!);
        }
    }
}
