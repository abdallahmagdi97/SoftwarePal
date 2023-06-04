using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using SoftwarePal.Models;
using SoftwarePal.Models.PayPal;

namespace SoftwarePal.Services
{
    public class PayPalService
    {
        private readonly PayPalOptions _options;
        private readonly PayPalHttpClient _client;
        private readonly ItemService _itemService;

        public PayPalService(IOptions<PayPalOptions> options, ItemService itemService)
        {
            _options = options.Value;
            _client = new PayPalHttpClient(new SandboxEnvironment(_options.ClientId, _options.SecretKey));
            _itemService = itemService;
        }

        public async Task<OrderRequest> CreateOrder(Models.Order order)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(await BuildRequestBodyAsync(order));

            var response = await _client.Execute(request);

            return response.Result<OrderRequest>();
        }

        private async Task<OrderRequest> BuildRequestBodyAsync(Models.Order order)
        {
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext()
                {
                    BrandName = "SoftwarePal",
                    LandingPage = "BILLING",
                    UserAction = "PAY_NOW",
                    ReturnUrl = "https://yourwebsite.com/return-url", // Replace with your actual return URL
                    CancelUrl = "https://yourwebsite.com/cancel-url"  // Replace with your actual cancel URL
                },
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
            new PurchaseUnitRequest()
            {
                AmountWithBreakdown = new AmountWithBreakdown()
                {
                    CurrencyCode = "USD",
                    Value = order.Total.ToString("F2"),
                    AmountBreakdown = new AmountBreakdown()
                    {
                        ItemTotal = new Money()
                        {
                            CurrencyCode = "USD",
                            Value = order.SubTotal.ToString("F2")
                        },
                        Discount = new Money()
                        {
                            CurrencyCode = "USD",
                            Value = order.Discount.ToString("F2")
                        }
                    }
                },
                Items = await GetItemsFromOrder(order.OrderItems)
            }


        }
            };

            return orderRequest;
        }
        private async Task<List<PayPalCheckoutSdk.Orders.Item>> GetItemsFromOrder(List<OrderItem>? orderItems)
        {
            List<PayPalCheckoutSdk.Orders.Item> items = new List<PayPalCheckoutSdk.Orders.Item>();
            if (orderItems != null)
            {
                foreach (var orderItem in orderItems)
                {
                    var item = await _itemService.GetById(orderItem.ItemId);
                    items.Add(
                        new PayPalCheckoutSdk.Orders.Item()
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Sku = "YOUR_PRODUCT_SKU",
                            UnitAmount = new Money()
                            {
                                CurrencyCode = "USD",
                                Value = orderItem.Price.ToString("F2")
                            },
                            Quantity = orderItem.Qty.ToString()
                        });
                }
            }
            return items;

        }
    }

}