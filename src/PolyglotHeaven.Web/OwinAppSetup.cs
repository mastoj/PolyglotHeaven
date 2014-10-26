using System.Web.Http;
using PolyglotHeaven.Web;
using Linky;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(OwinAppSetup))]
namespace PolyglotHeaven.Web
{
    public class OwinAppSetup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            configuration.EnsureInitialized();
            configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            LinkyConfiguration.Configure(configuration);
            app.UseWebApi(configuration);

            app.UseDefaultFiles(new DefaultFilesOptions()
            {
                DefaultFileNames = new[] {"index.html"}
            });
            app.UseFileServer(new FileServerOptions()
            {
                FileSystem = new PhysicalFileSystem(@".\content"),
                RequestPath = PathString.Empty,
                EnableDefaultFiles = true
            });

            app.Run((context) =>
            {
                var task = context.Response.WriteAsync("Hello world!");
                return task;
            });
        }
    }
}
