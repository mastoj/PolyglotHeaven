using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CQRSShop.Helpers
{
    public class EventStoreConnectionWrapper
    {
        private static IEventStoreConnection _connection;

        public static IEventStoreConnection Connect()
        {
            ConnectionSettings settings =
                ConnectionSettings.Create()
                    .UseConsoleLogger()
                    .KeepReconnecting()
                    .SetDefaultUserCredentials(new UserCredentials(EventStoreUser, EventStorePassword));
            var endPoint = new IPEndPoint(EventStoreIP, EventStorePort);
            _connection = EventStoreConnection.Create(settings, endPoint, null);
            _connection.ConnectAsync().Wait();
            return _connection;
        }

        private static string EventStoreUser
        {
            get { return "EventStoreUser".GetConfigSetting(y => y, () => "admin"); }
        }

        private static string EventStorePassword
        {
            get { return "EventStorePassword".GetConfigSetting(y => y, () => "changeit"); }
        }

        private static IPAddress EventStoreIP
        {
            get
            {
                return "EventStoreHostName".GetConfigSetting(val => Dns.GetHostAddresses(val)[0], () => IPAddress.Loopback);
            }
        }

        private static int EventStorePort
        {
            get
            {
                return "EventStorePort".GetConfigSetting(int.Parse, () => 1113);
            }
        }
    }
}
