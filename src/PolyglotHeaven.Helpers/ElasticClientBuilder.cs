using System;
using Nest;

namespace PolyglotHeaven.Helpers
{
    public static class ElasticClientBuilder
    {
        public static string IndexName = "polyglotheaven";

        public static IElasticClient BuildClient()
        {
            var elasticUrl = "ElasticUrl".GetConfigSetting(y => new Uri(y), () => new Uri("http://192.168.50.4:9200/"));
            var settings = new ConnectionSettings(elasticUrl);
            settings.SetDefaultIndex(IndexName);
            var esClient = new ElasticClient(settings);
            return esClient;
        }
    }
}
