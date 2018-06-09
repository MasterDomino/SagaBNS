using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using log4net;
using SagaBNS.Core;
using SagaBNS.Entity.Helpers;
using SagaBNS.Master.Library.Interface;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace SagaBNS.Master
{
    internal static class Program
    {
        #region Members

        private static readonly ManualResetEvent _run = new ManualResetEvent(true);

        private static bool _isDebug;

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
#if DEBUG
            _isDebug = true;
#endif
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Console.Title = $"SagaBNS Master Server{(_isDebug ? " Development Environment" : string.Empty)}";

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

            int port = Convert.ToInt32(ConfigurationManager.AppSettings["MasterPort"]);
            if (!ignoreStartupMessages)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string text = $"MASTER SERVER v{fileVersionInfo.ProductVersion}dev - PORT : {port}";
                int offset = (Console.WindowWidth / 2) + (text.Length / 2);
                string separator = new string('=', Console.WindowWidth);
                Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
            }

            // initialize DB
            if (!DataAccessHelper.Initialize())
            {
                Console.ReadLine();
                return;
            }
            Logger.Info("Configuration Loaded!");
            try
            {
                string ipAddress = ConfigurationManager.AppSettings["MasterIP"];
                IScsServiceApplication _server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ipAddress, port));
                _server.AddService<ICommunicationService, CommunicationService>(new CommunicationService());
                _server.AddService<IConfigurationService, ConfigurationService>(new ConfigurationService());
                _server.AddService<IAuthentificationService, AuthentificationService>(new AuthentificationService());
                _server.ClientConnected += OnClientConnected;
                _server.ClientDisconnected += OnClientDisconnected;
                _server.Start();
                Logger.Info("Master server started succesfully.");
            }
            catch (Exception ex)
            {
                Logger.Error("General Server ", ex);
            }
        }

        private static void OnClientConnected(object sender, ServiceClientEventArgs e) => Logger.Info("Client Connected: " + e.Client.ClientId);

        private static void OnClientDisconnected(object sender, ServiceClientEventArgs e) => Logger.Info("Client Disconnected: " + e.Client.ClientId);

        #endregion
    }
}