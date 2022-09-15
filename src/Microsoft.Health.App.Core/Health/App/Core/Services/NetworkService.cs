// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.NetworkService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Plugin.Connectivity.Abstractions;
using System;
using System.Linq;

namespace Microsoft.Health.App.Core.Services
{
    public class NetworkService : INetworkService
    {
        private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\NetworkService.cs");
        private readonly IConnectivity connectivity;
        private readonly IMessageSender messageSender;
        private bool? connected;

        public NetworkService(IConnectivity connectivity, IMessageSender messageSender)
        {
            this.connectivity = connectivity;
            this.messageSender = messageSender;
        }

        public bool Connected
        {
            get
            {
                if (!this.connected.HasValue)
                    this.RefreshStatus();
                return this.connected.Value;
            }
        }

        public void Initialize()
        {
            NetworkService.Logger.Debug((object)"Network service initializing.");
            // ISSUE: method pointer
            //TODO
            //this.connectivity.ConnectivityChanged += new ConnectivityChangedEventHandler((object) this, __methodptr(OnConnectivityChanged));
            this.RefreshStatus();
        }

        public void RefreshStatus()
        {
            bool internetAvailable = this.IsInternetAvailable;
            NetworkService.Logger.Debug((object)("Refreshing network status. Available: " + internetAvailable.ToString()));
            if (this.connected.HasValue && this.connected.Value == internetAvailable)
                return;
            NetworkService.Logger.Debug((object)"Sending network availability changed message.");
            this.connected = new bool?(internetAvailable);
            this.messageSender.Send<NetworkAvailabilityChangedMessage>(new NetworkAvailabilityChangedMessage()
            {
                Connected = this.connected.Value
            });
        }

        public bool IsInternetAvailable
        { 
            get => true;// => this.connectivity.IsConnected;
        }

        public bool OnWifi
        {
            get
            {
                //TODO
                return true;//this.connectivity.ConnectionTypes.Any<ConnectionType>((Func<ConnectionType, bool>)(c => c == 1));
            }
        }

        /*
        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
          NetworkService.Logger.Debug((object) "Network status change detected. Refreshing network status.");
          this.RefreshStatus();
        }
        */
  }
}
