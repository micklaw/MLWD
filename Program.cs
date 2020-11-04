using System.Threading.Tasks;
using Dotented;
using Microsoft.Extensions.DependencyInjection;
using MLWD.Components;
using MLWD.Pages;

namespace MLWD
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var dotented = services.AddDotented((builder) => 
            {
                return builder
                    .WithComponent<Asset>()
                    .WithComponent<Testimonial>()
                    .WithComponent<Skills>()
                    .WithPage<Me>((options) => 
                    {
                        options.SingleOnly = true;
                    })
                    .Build();
            });

            await dotented.Generate();
        }
    }
}
