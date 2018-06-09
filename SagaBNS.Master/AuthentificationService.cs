using Hik.Communication.ScsServices.Service;
using SagaBNS.DAO.Factory;
using SagaBNS.DTO;
using SagaBNS.Master.Library.Interface;
using System.Configuration;
using System.Linq;

namespace SagaBNS.Master
{
    internal class AuthentificationService : ScsService, IAuthentificationService
    {
        #region Methods

        public bool Authenticate(string authKey)
        {
            if (string.IsNullOrWhiteSpace(authKey))
            {
                return false;
            }

            if (authKey == ConfigurationManager.AppSettings["AuthentificationServiceAuthKey"])
            {
                MSManager.Instance.AuthentificatedClients.Add(CurrentClient.ClientId);
                return true;
            }

            return false;
        }

        public AccountDTO RequestAccount(string name)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)) || string.IsNullOrEmpty(name))
            {
                return null;
            }
            return DAOFactory.AccountDAO.LoadByName(name);
        }

        #endregion
    }
}