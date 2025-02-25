using AElf.Kernel;
using AElf.Modularity;
using AElf.OS.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace AElf.OS
{
    [DependsOn(typeof(CoreKernelAElfModule)), DependsOn(typeof(AbpBackgroundJobsModule))]
    public class CoreOSAElfModule : AElfModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddAssemblyOf<CoreOSAElfModule>();

            Configure<NetworkOptions>(configuration.GetSection("Network"));
        }
        
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var taskQueueManager = context.ServiceProvider.GetService<ITaskQueueManager>();
            taskQueueManager.CreateQueue(NetworkConstants.PeerReconnectionQueueName);
        }
    }
}