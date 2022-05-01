using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTagBackEnd.Dto;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using AutoTagBackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GatewayController : AppController
    {
        private readonly AutoTagContext _context;

        public GatewayController(AutoTagContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> FlowConfirmation([FromForm] string token)
        {
            PaymentStatus? paymentStatus = await Flow.GetStatusAsync(_context, token, this.CurrentAccount);

            if (paymentStatus.Code != null)
            {
                throw new Exception(String.Format(
                    "Error al leer estado de pago con token {0}, error: {1}, mensaje: {2}",
                    token, paymentStatus.Code, paymentStatus.Message));
            }

            List<TransactionState> listTransactionState = _context.TransactionStates.ToList();

            // 1 = pendiente de pago
            if (paymentStatus.status == 1)
            {
                return Ok(new { TransitionStateCode = "pending" });
            }
            // 2 = pagada
            else if (paymentStatus.status == 2)
            {
                // obtener TrasactionState pendiente
                TransactionState transactionStatePending = listTransactionState.SingleOrDefault(t => t.Code == "pending");
                if (transactionStatePending == null)
                {
                    throw new Exception("No se encontró el estado de transacción pending");
                }
                // obtener TrasactionState Pagada
                TransactionState transactionStateCompleted = listTransactionState.SingleOrDefault(t => t.Code == "completed");
                if (transactionStateCompleted == null)
                {
                    throw new Exception("No se encontró el estado de transacción completed");
                }
                // obtener medio de pago
                Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
                if (gatewayFlow == null)
                {
                    throw new Exception("No se encontró el medio de pago flow");
                }
                // obtener transacción
                Transaction transaction = _context.Transactions.SingleOrDefault
                    (t => t.TransactionStateId == transactionStatePending.Id &&
                    t.GatewayId == gatewayFlow.Id &&
                    t.GatewayOrder == paymentStatus.flowOrder &&
                    t.GatewayToken == token);
                if (transaction == null)
                {
                    throw new Exception("No se encontró la transacción con codigo flow: " + paymentStatus.flowOrder);
                }
                // marcar transacción como pagado y fecha
                transaction.TransactionStateId = transactionStateCompleted.Id;
                transaction.PaymentDate = DateTime.Now;

                // marcar factura como pagada
                InvoiceState invoiceStatePaid = _context.InvoiceStates.SingleOrDefault(i => i.Code == "paid");
                if (invoiceStatePaid == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código active");
                }
                Invoice invoice = _context.Invoices.SingleOrDefault(i => i.Id == transaction.InvoiceId);
                if (invoice == null)
                {
                    throw new Exception("No se encontró la factura con id: " + transaction.InvoiceId);
                }
                invoice.InvoiceStateId = invoiceStatePaid.Id;

                // marcar orden como aceptada
                PurchaseOrderState purchaseOrderStateActive = _context.PurchaseOrderStates.SingleOrDefault(pos => pos.Code == "active");
                if (purchaseOrderStateActive == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código active");
                }
                PurchaseOrder purchaseOrder = _context.PurchaseOrders.SingleOrDefault(p => p.Id == invoice.PurchaseOrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception("No se encontró la orden con id: " + invoice.PurchaseOrderId);
                }
                purchaseOrder.PurchaseOrderStateId = purchaseOrderStateActive.Id;
                // actualizar fecha de vencimiento de orden de compra
                // si es la primera vez sumar un mes
                if (purchaseOrder.NextDueDate == null)
                {
                    purchaseOrder.NextDueDate = DateTime.Today.AddMonths(1);
                }
                // si está vencida
                else if (purchaseOrder.NextDueDate <= DateTime.Today)
                {
                    purchaseOrder.NextDueDate = DateTime.Today.AddMonths(1);
                }
                // si está vigente
                else
                {
                    purchaseOrder.NextDueDate = ((DateTime)purchaseOrder.NextDueDate).AddMonths(1);
                }
                // marcar cliente con nuevo rol
                var listProduct =
                    (from pod in _context.PurchaseOrderDetails
                     join p in _context.Products
                     on pod.ProductId equals p.Id
                     where pod.PurchaseOrderId == purchaseOrder.Id
                     select new
                     {
                         PaymentCycleId = pod.PaymentCycleId,
                         ProductId = p.Id,
                         ProductCode = p.Code,
                         ProductName = p.Name
                     }
                     ).ToList();

                Account account = _context.Accounts.SingleOrDefault(a => a.Id == purchaseOrder.AccountId);
                if (account == null)
                {
                    throw new Exception("No se encontró la cuenta con id: " + purchaseOrder.AccountId);
                }
                List<Role> listRole = _context.Roles.ToList();

                foreach (var product in listProduct)
                {
                    if (product.ProductCode == "viasimple_user")
                    {
                        Role roleUser = listRole.SingleOrDefault(r => r.Code == "user");
                        if (roleUser == null) throw new Exception("No se encontró rol user");
                        account.RoleId = roleUser.Id;
                    }
                    else if (product.ProductCode == "viasimple_pyme")
                    {
                        Role roleUser = listRole.SingleOrDefault(r => r.Code == "pyme");
                        if (roleUser == null) throw new Exception("No se encontró rol pyme");
                        account.RoleId = roleUser.Id;
                    }
                    else if (product.ProductCode == "viasimple_business")
                    {
                        Role roleUser = listRole.SingleOrDefault(r => r.Code == "business");
                        if (roleUser == null) throw new Exception("No se encontró rol business");
                        account.RoleId = roleUser.Id;
                    }
                }

                // guardar transacción, factura, orden de compra y usuario
                _context.SaveChanges();

                return Ok(new { TransitionStateCode = "completed" });
            }
            // 3 = rechazada
            else if (paymentStatus.status == 3)
            {
                // obtener TrasactionState pendiente
                TransactionState transactionStatePending = listTransactionState.SingleOrDefault(t => t.Code == "pending");
                if (transactionStatePending == null)
                {
                    throw new Exception("No se encontró el estado de transacción pending");
                }
                // obtener TrasactionState Rechazada
                TransactionState transactionStateDeclined = listTransactionState.SingleOrDefault(t => t.Code == "declined");
                if (transactionStateDeclined == null)
                {
                    throw new Exception("No se encontró el estado de transacción declined");
                }
                // obtener medio de pago
                Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
                if (gatewayFlow == null)
                {
                    throw new Exception("No se encontró el medio de pago flow");
                }
                // obtener transacción
                Transaction transaction = _context.Transactions.SingleOrDefault
                    (t => t.TransactionStateId == transactionStatePending.Id &&
                    t.GatewayId == gatewayFlow.Id &&
                    t.GatewayOrder == paymentStatus.flowOrder &&
                    t.GatewayToken == token);
                if (transaction == null)
                {
                    throw new Exception("No se encontró la transacción con codigo flow: " + paymentStatus.flowOrder);
                }
                // marcar transacción como rechazada
                transaction.TransactionStateId = transactionStateDeclined.Id;

                // marcar factura como anulada
                InvoiceState invoiceStateCanceled = _context.InvoiceStates.SingleOrDefault(i => i.Code == "canceled");
                if (invoiceStateCanceled == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código canceled");
                }
                Invoice invoice = _context.Invoices.SingleOrDefault(i => i.Id == transaction.InvoiceId);
                if (invoice == null)
                {
                    throw new Exception("No se encontró la factura con id: " + transaction.InvoiceId);
                }
                invoice.InvoiceStateId = invoiceStateCanceled.Id;

                // marcar orden como anulada
                PurchaseOrderState purchaseOrderStateCanceled = _context.PurchaseOrderStates.SingleOrDefault(pos => pos.Code == "canceled");
                if (purchaseOrderStateCanceled == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código canceled");
                }
                PurchaseOrder purchaseOrder = _context.PurchaseOrders.SingleOrDefault(p => p.Id == invoice.PurchaseOrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception("No se encontró la orden con id: " + invoice.PurchaseOrderId);
                }
                purchaseOrder.PurchaseOrderStateId = purchaseOrderStateCanceled.Id;

                // guardar transacción, factura y orden de compra
                _context.SaveChanges();

                return Ok(new { TransitionStateCode = "declined" });
            }
            // 4 = anulada
            else if (paymentStatus.status == 4)
            {
                // obtener TrasactionState pendiente
                TransactionState transactionStatePending = listTransactionState.SingleOrDefault(t => t.Code == "pending");
                if (transactionStatePending == null)
                {
                    throw new Exception("No se encontró el estado de transacción pending");
                }
                // obtener TrasactionState Anulada
                TransactionState transactionStateCanceled = listTransactionState.SingleOrDefault(t => t.Code == "canceled");
                if (transactionStateCanceled == null)
                {
                    throw new Exception("No se encontró el estado de transacción canceled");
                }
                // obtener medio de pago
                Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
                if (gatewayFlow == null)
                {
                    throw new Exception("No se encontró el medio de pago flow");
                }
                // obtener transacción
                Transaction transaction = _context.Transactions.SingleOrDefault
                    (t => t.TransactionStateId == transactionStatePending.Id &&
                    t.GatewayId == gatewayFlow.Id &&
                    t.GatewayOrder == paymentStatus.flowOrder &&
                    t.GatewayToken == token);
                if (transaction == null)
                {
                    throw new Exception("No se encontró la transacción con codigo flow: " + paymentStatus.flowOrder);
                }
                // marcar transacción como anulada
                transaction.TransactionStateId = transactionStateCanceled.Id;

                // marcar factura como anulada
                InvoiceState invoiceStateCanceled = _context.InvoiceStates.SingleOrDefault(i => i.Code == "canceled");
                if (invoiceStateCanceled == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código canceled");
                }
                Invoice invoice = _context.Invoices.SingleOrDefault(i => i.Id == transaction.InvoiceId);
                if (invoice == null)
                {
                    throw new Exception("No se encontró la factura con id: " + transaction.InvoiceId);
                }
                invoice.InvoiceStateId = invoiceStateCanceled.Id;

                // marcar orden como anulada
                PurchaseOrderState purchaseOrderStateCanceled = _context.PurchaseOrderStates.SingleOrDefault(pos => pos.Code == "canceled");
                if (purchaseOrderStateCanceled == null)
                {
                    throw new Exception("No se encontró estado de orden de compra con código canceled");
                }
                PurchaseOrder purchaseOrder = _context.PurchaseOrders.SingleOrDefault(p => p.Id == invoice.PurchaseOrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception("No se encontró la orden con id: " + invoice.PurchaseOrderId);
                }
                purchaseOrder.PurchaseOrderStateId = purchaseOrderStateCanceled.Id;

                // guardar transacción, factura y orden de compra
                _context.SaveChanges();
                return Ok(new { TransitionStateCode = "canceled" });
            }
            else
            {
                throw new Exception("Valor inválido para paymentStatus: " + paymentStatus.status);
            }
        }

        [HttpPost]
        public RedirectResult FlowReturn([FromForm] string token)
        {
            Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
            if (gatewayFlow == null)
            {
                throw new Exception("No se encontró el medio de pago flow");
            }
            if (gatewayFlow.UrlReturn == null)
            {
                throw new Exception("No se encontró la url de retorno para flow");
            }
            string urlReturn = string.Format("{0}?token={1}", gatewayFlow.UrlReturn, token);
            return Redirect(urlReturn);
        }

        public record FlowGetStatusRequest(string Token);
        [HttpPost]
        public ActionResult FlowGetStatus([FromBody] FlowGetStatusRequest body)
        {
            Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
            if (gatewayFlow == null)
            {
                throw new Exception("No se encontró el medio de pago flow");
            }
            var data = (
                from t in _context.Transactions
                join ts in _context.TransactionStates
                on t.TransactionStateId equals ts.Id
                join i in _context.Invoices
                on t.InvoiceId equals i.Id
                join o in _context.PurchaseOrders
                on i.PurchaseOrderId equals o.Id
                join a in _context.Accounts
                on o.AccountId equals a.Id
                join r in _context.Roles
                on a.RoleId equals r.Id
                where
                    t.GatewayToken == body.Token &&
                    o.AccountId == this.CurrentAccount.Id &&
                    t.GatewayId == gatewayFlow.Id
                select new
                {
                    TransactionStateCode = ts.Code,
                    AccountRoleCode = r.Code
                }
            ).SingleOrDefault();

            if(data == null)
            {
                return Ok(new { CurrentAccountId = this.CurrentAccount.Id, Token = body.Token });
            }

            return Ok(new { TransitionStateCode = data.TransactionStateCode, AccountRoleCode = data.TransactionStateCode });
        }
    }
}