using SagaBNS.Master.Library.Data;

namespace SagaBNS.Master.Library.Interface
{
    public interface IConfigurationClient
    {
        #region Methods

        void ConfigurationUpdated(ConfigurationObject configurationObject);

        #endregion
    }
}