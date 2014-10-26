using System;
using Neo4jClient;

namespace PolyglotHeaven.Helpers
{
    public static class Neo4jClientBuilder
    {
        public static GraphClient Build()
        {
            var uri = "Neo4jUrl".GetConfigSetting(y => new Uri(y + "/db/data"),
                () => new Uri("http://localhost:7474/db/data"));
            var graphClient = new GraphClient(uri);
            graphClient.Connect();
            return graphClient;
        }

    }
}