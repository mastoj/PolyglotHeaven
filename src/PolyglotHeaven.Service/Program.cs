using Topshelf;

namespace PolyglotHeaven.Service
{
    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<IndexingServie>(s =>
                {
                    s.ConstructUsing(name => new IndexingServie());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("PolyglotHeaven.Service");
                x.SetDisplayName("PolyglotHeaven.Service");
                x.SetServiceName("PolyglotHeaven.Service");
            });
        }
    }
}
