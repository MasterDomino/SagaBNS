using log4net;
using SagaBNS.Core;
using SagaBNS.Entity.Helpers;
using SagaBNS.Enums;
using SagaBNS.GameObject.Networking;
using SagaBNS.Master.Library.Client;
using SagaBNS.Master.Library.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SagaBNS.Lobby
{
    internal static class Program
    {
        #region Members

        private static bool _isDebug;

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
#if DEBUG
            _isDebug = true;
#endif
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Console.Title = $"SagaBNS Lobby Server{(_isDebug ? " Development Environment" : string.Empty)}";

            bool ignoreStartupMessages = false;
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--nomsg":
                        ignoreStartupMessages = true;
                        break;
                }
            }

            // initialize Logger
            Logger.InitializeLogger(LogManager.GetLogger(typeof(Program)));

            int port = Convert.ToInt32(ConfigurationManager.AppSettings["LobbyPort"]);
            if (!ignoreStartupMessages)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string text = $"LOBBY SERVER v{fileVersionInfo.ProductVersion}dev - PORT : {port}";
                int offset = (Console.WindowWidth / 2) + (text.Length / 2);
                string separator = new string('=', Console.WindowWidth);
                Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
            }

            // initialize api
            if (CommunicationServiceClient.Instance.Authenticate(ConfigurationManager.AppSettings["MasterAuthKey"]))
            {
                Logger.Info("API Initialized");
            }

            string ipAddress = ConfigurationManager.AppSettings["IPAddress"];

            // GUID Shall be generated in ServerManager
            CommunicationServiceClient.Instance.RegisterLobbyServer(new SerializableLobbyServer(Guid.NewGuid(), ipAddress, port));

            // initialize DB
            if (!DataAccessHelper.Initialize())
            {
                Console.ReadLine();
                return;
            }
            Logger.Info("Configuration Loaded!");
            try
            {
                // load packets
                //PacketFacility.Initialize(typeof(STSConnectPacket));
                //PacketFacility.Initialize(typeof(LoginStartPacket));
                //PacketFacility.Initialize(typeof(ListMyAccountsPacket));
                NetworkManager manager = new NetworkManager(ipAddress, port, ServerType.Lobby);
                Logger.Info("SagaBNS Networking succesfully initialized!");
            }
            catch (Exception ex)
            {
                Logger.LogEventError("INITIALIZATION_EXCEPTION", "General Error Server", ex);
            }
            Process.GetCurrentProcess().WaitForExit();
        }

        #endregion
    }
}