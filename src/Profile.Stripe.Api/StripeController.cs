using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Profile.Stripe.Api
{
    [Route("/stripe")]
    public class StripeController : ControllerBase
    {
        /// <summary>
        /// Webhook for stripe.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            const string endpointSecret = "whsec_2f59ed39c77d9bc79133b18ddcbfe83573dc1c477efe933f81410128a8e4e26b";  
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var couponService = new CouponService();
            var coupon = await couponService.GetAsync("vRRTW068");
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                    signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                var a = e.Message;
                return StatusCode(500);
            }
        }
    }
}