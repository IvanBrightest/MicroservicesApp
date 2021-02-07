using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILoggerFactory logger, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                context.Database.Migrate();

                if(!await context.Orders.AnyAsync())
                {
                    await context.AddRangeAsync(GetPreconfigurationOrders());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if(retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = logger.CreateLogger<OrderContextSeed>();
                    log.LogError(ex.Message);

                    await SeedAsync(context, logger, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfigurationOrders()
        {
            return new List<Order>
                {
                new Order()
                {
                    UserName = "swn",
                    FirstName = "User",
                    LastName = "Usleser",
                    EmailAddress = "admin@admin.com",
                    Country = "Russian"
                }
            };
        }
    }
}
