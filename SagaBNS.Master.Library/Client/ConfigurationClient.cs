using SagaBNS.Master.Library.Data;
using SagaBNS.Master.Library.Interface;
using System.Threading.Tasks;

namespace SagaBNS.Master.Library.Client
{
    internal  class ConfigurationClient : IConfigurationClient
    {
        public void ConfigurationUpdated(ConfigurationObject configurationObject) => Task.Run(() => ConfigurationServiceClient.Instance.OnConfigurationUpdated(configurationObject));
    }
}
