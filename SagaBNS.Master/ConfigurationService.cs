using SagaBNS.Master.Library.Data;
using SagaBNS.Master.Library.Interface;
using Hik.Communication.ScsServices.Service;
using System;
using System.Configuration;
using System.Linq;

namespace SagaBNS.Master
{
    internal class ConfigurationService : ScsService, IConfigurationService
    {
        public bool Authenticate(string authKey, Guid serverId)
        {
            if (string.IsNullOrWhiteSpace(authKey))
            {
                return false;
            }

            if (authKey == ConfigurationManager.AppSettings["MasterAuthKey"])
            {
                MSManager.Instance.AuthentificatedClients.Add(CurrentClient.ClientId);

                WorldServer ws = MSManager.Instance.WorldServers.Find(s => s.Id == serverId);
                if (ws != null)
                {
                    ws.ConfigurationServiceClient = CurrentClient;
                }
                return true;
            }

            return false;
        }

        public ConfigurationObject GetConfigurationObject()
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return null;
            }
            return MSManager.Instance.ConfigurationObject;
        }

        public void UpdateConfigurationObject(ConfigurationObject configurationObject)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            MSManager.Instance.ConfigurationObject = configurationObject;

            foreach(WorldServer ws in MSManager.Instance.WorldServers)
            {
                ws.ConfigurationServiceClient.GetClientProxy<IConfigurationClient>().ConfigurationUpdated(MSManager.Instance.ConfigurationObject);
            }
        }
    }
}
