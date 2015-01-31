using System;
using System.Net;
using System.Net.Http;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace PolyglotHeaven.Helpers
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
            EnableCategoryProjection();
            return _connection;
        }

        private static void EnableCategoryProjection()
        {
            var url = new Uri(string.Format("http://{0}:2113/projection/$by_category/command/enable", EventStoreIP));
            var credentials = new NetworkCredential(EventStoreUser, EventStorePassword);
            var handler = new HttpClientHandler
            {
                Credentials = credentials
            };
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.PostAsync(url, null).Wait();
            }
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
