using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BingoX.AspNetCore.Extensions
{
    public abstract class DIConfigureServices
    {
        public DIConfigureServices(IConfiguration configuration, IServiceCollection services)
        {

            this.configuration = configuration;
            this.services = services;
            Register();
        }

        protected readonly IConfiguration configuration;
        protected readonly IServiceCollection services;
        protected abstract void Register();


    }
}
