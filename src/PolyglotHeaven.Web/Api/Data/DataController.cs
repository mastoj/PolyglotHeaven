using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Types;
using PolyglotHeaven.Domain;

namespace PolyglotHeaven.Web.Api.Data
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        private DomainEntry _domainEntry;

        private DomainEntry DomainEntry
        {
            get
            {
                _domainEntry = _domainEntry ?? ApplicationConfiguration.CreateDomainEntry();
                return _domainEntry;
            }
        }

        [Route]
        public HttpResponseMessage Post()
        {
            var createCustomerCommands = _names.Select(n => new CreateCustomer(Guid.NewGuid(), n)).ToList();
            createCustomerCommands.ForEach(c => DomainEntry.ExecuteCommand(c));

            var createProductCommands = _things.Select(t => new CreateProduct(Guid.NewGuid(), t, t.Length*10)).ToList();
            createProductCommands.ForEach(c => DomainEntry.ExecuteCommand(c));

            var orderCommands = CreatePlaceOrderCommands(createCustomerCommands, createProductCommands);
            orderCommands.ForEach(po => DomainEntry.ExecuteCommand(po));

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private List<PlaceOrder> CreatePlaceOrderCommands(List<CreateCustomer> createCustomerCommands, List<CreateProduct> createProductCommands)
        {
            var random = new Random();
            var placeOrders = new List<PlaceOrder>();
            foreach (var customerCommand in createCustomerCommands)
            {
                var numberOfOrders = random.Next(1, 3);
                for (var i = 0; i < numberOfOrders; i++)
                {
                    var numberOfProducts = random.Next(1, 3);
                    var items = new List<OrderItem>();
                    var selectedProductIds = new List<Guid>();
                    while (items.Count < numberOfProducts)
                    {
                        var index = random.Next(createProductCommands.Count - 1);
                        var product = createProductCommands[index];
                        if (!selectedProductIds.Contains(product.Id))
                        {
                            items.Add(new OrderItem(product.Id, random.Next(1, 30)));
                        }
                    }
                    var fsharpItems = Contracts.Helpers.ToFSharpList(items);
                    placeOrders.Add(new PlaceOrder(Guid.NewGuid(), customerCommand.Id, fsharpItems));
                }
            }
            return placeOrders;
        }

        private static List<string> _names = new List<string>()
        {
            "Terence Fyfe",
            "Troy Conrad",
            "Harlan Carlo",
            "Claris Doty",
            "Twanda Ocon",
            "Sparkle Gamber",
            "Arnita Summa",
            "Jeraldine Irion",
            "Chastity Ansley",
        };

        private static List<string> _things = new List<string>()
        {
            "Apple",
            "Celery",
            "History",
            "Meal",
            "Perch",
            "Polo",
            "Precipitation",
            "Sleep",
            "Sock",
            "Spaghetti"
        };
    }
}