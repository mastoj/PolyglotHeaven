using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace CQRSShop.Elasticsearch
{
    public static class ElasticClientBuilder
    {
        private static string _index = "cqrsshop";

        public static IElasticClient BuildClient()
        {
            var settings = new ConnectionSettings(new Uri("http://192.168.50.4:9200/"));
            settings.SetDefaultIndex(_index);
            var esClient = new ElasticClient(settings);
            return esClient;
        }
    }
}
