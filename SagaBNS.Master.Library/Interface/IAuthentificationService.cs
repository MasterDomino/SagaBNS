using Hik.Communication.ScsServices.Service;
using SagaBNS.DTO;

namespace SagaBNS.Master.Library.Interface
{
    [ScsService(Version = "1.1.0.0")]
    public interface IAuthentificationService
    {
        #region Methods

        /// <summary>
        /// Authenticates a Client to the Service
        /// </summary>
        /// <param name="authKey">The private Authentication key</param>
        /// <returns>true if successful, else false</returns>
        bool Authenticate(string authKey);

        /// <summary>
        /// Returns a requested account for given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AccountDTO RequestAccount(string name);

        #endregion
    }
}