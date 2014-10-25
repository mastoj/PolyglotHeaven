using System;
using Nest;

namespace PolyglotHeaven.Helpers
{
    public static class ElasticClientBuilder
    {
        private static string _index = "PolyglotHeaven";

        public static IElasticClient BuildClient()
        {
            var elasticUrl = "ElasticUrl".GetConfigSetting(y => new Uri(y), () => new Uri("http://192.168.50.4:9200/"));
            var settings = new ConnectionSettings(elasticUrl);
            settings.SetDefaultIndex(_index);
            var esClient = new ElasticClient(settings);
            return esClient;
        }
    }
}
