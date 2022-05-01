using System;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PurchaseController : AppController
    {
        private readonly AutoTagContext _context;

        public PurchaseController(AutoTagContext context)
        {
            _context = context;
        }

        public record PurchaseServiceRequest(int ProductId, string DiscountCode, int PaymentCycleId);
        [HttpPost]
        public async Task<IActionResult> PurchaseServiceAsync([FromBody] PurchaseServiceRequest body)
        {
            // chequear que producto exista y esté habilitado
            bool productExistsAndIsEnabled = _context.Products.Any(p => p.Id == body.ProductId && p.Enabled);
            if (!productExistsAndIsEnabled)
            {
                throw new Exception(string.Format("No se encontró producto con id: {1}", body.ProductId));
            }

            // chequear que esté disponible el ciclo de pago seleccionado
            bool productHasPaymentCycle = _context.Prices.Any(p => p.ProductId == body.ProductId && p.PaymentCycleId == body.PaymentCycleId && p.Enabled);
            if (!productHasPaymentCycle)
            {
                throw new Exception(string.Format("No se encontró ciclo de pago con id: {1}", body.PaymentCycleId));
            }

            // chequear que codigo de descuento sea válido si es que tiene
            // por ahora no se programara

            // crear orden de compra
            PurchaseOrderState purchaseOrderStatePending = _context.PurchaseOrderStates.SingleOrDefault(pos => pos.Code == "pending");
            Price price = _context.Prices.SingleOrDefault(p => p.ProductId == body.ProductId && p.PaymentCycleId == body.PaymentCycleId && p.Enabled);
            if (purchaseOrderStatePending == null)
            {
                throw new Exception("No se encontró estado de orden de compra con código pending");
            }
            if (price == null)
            {
                throw new Exception(string.Format("No se encontró precio para el producto id: {0} y ciclo de pago: {1}", body.ProductId, body.PaymentCycleId));
            }
            PurchaseOrder purchaseOrder = new()
            {
                AccountId = this.CurrentAccount.Id,
                CreationDate = DateTime.Now,
                PurchaseOrderStateId = purchaseOrderStatePending.Id,
                Amount = price.Amount,
                AmountWithoutDiscount = price.Amount
            };
            // guardar orden de compra y obtener id
            _context.SaveChanges();

            // crear detalle de orden de compra
            PurchaseOrderDetail purchaseOrderDetail = new()
            {
                PurchaseOrderId = purchaseOrder.Id,
                ProductId = body.ProductId,
                PaymentCycleId = body.PaymentCycleId,
                Amount = price.Amount
            };

            // crear factura
            InvoiceState invoiceStatePending = _context.InvoiceStates.SingleOrDefault(i => i.Code == "pending");
            if (invoiceStatePending == null)
            {
                throw new Exception("No se encontró estado de factura con código pending");
            }
            Invoice invoice = new()
            {
                PurchaseOrderId = purchaseOrder.Id,
                InvoiceStateId = invoiceStatePending.Id,
                CreationDate = DateTime.Now,
                DueDate = DateTime.Today.AddMonths(1),
                Amount = price.Amount
            };
            // guardar detalle de orden de compra, factura y obtener id
            _context.SaveChanges();

            // crear código de factura
            invoice.Code = string.Format("{0}", invoice.Id);

            // crear detalle de factura
            Product product = _context.Products.SingleOrDefault(p => p.Id == body.ProductId && p.Enabled);
            PaymentCycle paymentCycle = _context.PaymentCycles.SingleOrDefault(pc => pc.Id == body.PaymentCycleId);
            if (product == null)
            {
                throw new Exception(string.Format("No se encontró producto con id: {1}", body.ProductId));
            }
            if (paymentCycle == null)
            {
                throw new Exception(string.Format("No se encontró ciclo de pago con id: {1}", body.PaymentCycleId));
            }
            InvoiceDetail invoiceDetail = new()
            {
                InvoiceId = invoice.Id,
                Description = string.Format("{0} - {1}", product.Description, paymentCycle.Represents),
                Quantity = 1,
                Amount = price.Amount
            };
            // guardar detalle de factura
            _context.SaveChanges();

            // generar orden de pago flow
            CreatePaymentResponse? createPaymentResponse = await Flow.GeneratePaymentOrderAsync(_context, invoice);

            // Obtener medio de pago
            Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
            if (gatewayFlow == null)
            {
                throw new Exception("No se encontró el medio de pago flow");
            }

            // Obtener tipo de transacción pendinte
            TransactionState transactionState = _context.TransactionStates.SingleOrDefault(ts => ts.Code == "pending");
            if (transactionState == null)
            {
                throw new Exception("No se encontró el estado de transacción pending");
            }

            // generar transacción
            if (createPaymentResponse.Code != null)
            {
                throw new Exception(String.Format(
                    "Error al enviar factura {0} a pagar en flow, error: {1}, mensaje: {2}",
                    invoice.Id, createPaymentResponse.Code, createPaymentResponse.Message));
            }
            Account account = _context.Accounts.SingleOrDefault(a => a.Id == purchaseOrder.AccountId);
            if (account == null)
            {
                throw new Exception("No se encontró la cuenta con id: " + purchaseOrder.AccountId);
            }
            bool useDevelopmentData = gatewayFlow.UseDevelopmentData || account.UseDevelopmentPurchaseData;
            Transaction transaction = new()
            {
                InvoiceId = invoice.Id,
                GatewayId = gatewayFlow.Id,
                GatewayOrder = createPaymentResponse.flowOrder,
                GatewayToken = createPaymentResponse.Token,
                GatewayPaymentMethod = gatewayFlow.PaymentMethod ?? 9,
                TransactionStateId = transactionState.Id,
                CreationDate = DateTime.Now,
                Amount = invoice.Amount,
                IsDevelopment = useDevelopmentData
            };
            // guardar transacción
            _context.SaveChanges();

            string paymentUrl = string.Format("{0}?token={1}", createPaymentResponse.Url, createPaymentResponse.Token);

            return Ok(new { PaymentUrl = paymentUrl });
        }
    }
}

