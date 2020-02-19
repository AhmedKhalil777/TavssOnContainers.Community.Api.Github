using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commuinity.Api.Options;
using Commuinity.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Commuinity.Api.Installers
{
    public class MongoDBInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CommunitiesStoreDatabaseSettings>(configuration.GetSection(nameof(CommunitiesStoreDatabaseSettings)));
            services.AddSingleton<ICommunitiestoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CommunitiesStoreDatabaseSettings>>().Value);
            services.AddSingleton<IMongoCommunitiesService,MongoCommunitiesService>();
        }
    }
}
