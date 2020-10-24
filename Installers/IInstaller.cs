using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace PostBook.Installers
{
    interface IInstaller
    {
        void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}
