using API.Errors;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly string _whSecret;

    private readonly ILogger<PaymentsController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentsController(
        IPaymentService paymentService,
        ILogger<PaymentsController> logger,
        IConfiguration config)
    {
        _paymentService = paymentService;
        _logger = logger;
        _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
    }

    [Authorize]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        CustomerBasket basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

        if (basket is null)
        {
            return BadRequest(new ApiResponse(400, "Problem with your basket"));
        }

        return Ok(basket);
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);

        PaymentIntent intent;

        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment succeeded: {0}", intent.Id);
                await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                break;
            case "payment_intent.payment_failed":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment failed: {0}", intent.Id);
                await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                break;
            default:
                break;
        }

        return new EmptyResult();
    }
}
