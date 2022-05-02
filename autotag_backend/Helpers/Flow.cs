using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using AutoTagBackEnd.Models;

namespace AutoTagBackEnd.Helpers
{
	public static class Flow
	{
        public static async Task<CreatePaymentResponse> GeneratePaymentOrderAsync(AutoTagContext _context, Invoice invoice)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrders.SingleOrDefault(p => p.Id == invoice.PurchaseOrderId);
            if(purchaseOrder == null)
            {
                throw new Exception("No se encontró la orden con id: " + invoice.PurchaseOrderId);
            }
            Account account = _context.Accounts.SingleOrDefault(a => a.Id == purchaseOrder.AccountId);
            if (account == null)
            {
                throw new Exception("No se encontró la cuenta con id: " + purchaseOrder.AccountId);
            }
            // Obtener medio de pago Flow
            Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
            if (gatewayFlow == null)
            {
                throw new Exception("No se encontró el medio de pago flow");
            }

            bool useDevelopmentData = gatewayFlow.UseDevelopmentData || account.UseDevelopmentPurchaseData;
            string apikey = (useDevelopmentData ? gatewayFlow.ApiKeyDev : gatewayFlow.ApiKeyProd) ?? "error";
            string secretKey = (useDevelopmentData ? gatewayFlow.SecretKeyDev : gatewayFlow.SecretKeyProd) ?? "error";
            string restApiUrl = (useDevelopmentData ? gatewayFlow.RestApiUrlDev : gatewayFlow.RestApiUrlProd) ?? "error";

            using (var httpClient = new HttpClient())
            {
                Dictionary<string, string> postData = new();
                postData.Add("apiKey", apikey);
                postData.Add("commerceOrder", invoice.Code ?? "error");
                postData.Add("subject", "ViaSimple - Factura Nº" + invoice.Code);
                postData.Add("currency", "CLP");
                postData.Add("amount", invoice.Amount.ToString("N0").Replace(",", String.Empty));
                postData.Add("email", account.Email);
                postData.Add("paymentMethod", (gatewayFlow.PaymentMethod ?? 9).ToString());
                postData.Add("urlConfirmation", gatewayFlow.UrlConfirmation ?? "error");
                postData.Add("urlReturn", gatewayFlow.BackendUrlReturn ?? "error");

                // generar string con valores ordenados
                string orderedString = postData.OrderBy(p => p.Key).Select(p => p.Key + p.Value).Aggregate((i, j) => i + j);

                // generar firma
                string signature = hmacSHA256(orderedString, secretKey);

                // agregar firma
                postData.Add("s", signature);

                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    string url = restApiUrl + "/payment/create";
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    CreatePaymentResponse createPaymentResponse = await response.Content.ReadAsAsync<CreatePaymentResponse>();

                    return createPaymentResponse;
                }
            }
        }

        public static async Task<PaymentStatus> GetStatusAsync(AutoTagContext _context, string token)
        {
            // Obtener medio de pago
            Gateway gatewayFlow = _context.Gateways.SingleOrDefault(g => g.Code == "flow" && g.Enabled);
            if (gatewayFlow == null)
            {
                throw new Exception("No se encontró el medio de pago flow");
            }

            Account account = (from t in _context.Transactions
                               join i in _context.Invoices
                               on t.InvoiceId equals i.Id
                               join po in _context.PurchaseOrders
                               on i.PurchaseOrderId equals po.Id
                               join a in _context.Accounts
                               on po.AccountId equals a.Id
                               where t.GatewayToken == token && t.GatewayId == gatewayFlow.Id
                               select a
                               ).SingleOrDefault();

            if(account == null)
            {
                throw new Exception("No se encontró cuenta");
            }

            bool useDevelopmentData = gatewayFlow.UseDevelopmentData || account.UseDevelopmentPurchaseData;
            string apikey = (useDevelopmentData ? gatewayFlow.ApiKeyDev : gatewayFlow.ApiKeyProd) ?? "error";
            string secretKey = (useDevelopmentData ? gatewayFlow.SecretKeyDev : gatewayFlow.SecretKeyProd) ?? "error";
            string restApiUrl = (useDevelopmentData ? gatewayFlow.RestApiUrlDev : gatewayFlow.RestApiUrlProd) ?? "error";

            using (var httpClient = new HttpClient())
            {
                Dictionary<string, string> postData = new();
                postData.Add("apiKey", apikey);
                postData.Add("token", token);

                // generar string con valores ordenados
                string orderedString = postData.OrderBy(p => p.Key).Select(p => p.Key + p.Value).Aggregate((i, j) => i + j);

                // generar firma
                string signature = hmacSHA256(orderedString, secretKey);

                // agregar firma
                postData.Add("s", signature);

                using (var content = new FormUrlEncodedContent(postData))
                {
                    string url = restApiUrl + "/payment/getStatus";
                    url = url + "?apiKey=" + postData["apiKey"];
                    url = url + "&token=" + postData["token"];
                    url = url + "&s=" + postData["s"];
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    return await response.Content.ReadAsAsync<PaymentStatus>();
                }
            }
        }

        static string hmacSHA256(string data, string key)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] keyBytes = encoding.GetBytes(key);
            Byte[] hashBytes = encoding.GetBytes(data);
            using (HMACSHA256 hash = new HMACSHA256(keyBytes)) hashBytes = hash.ComputeHash(hashBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    public class CreatePaymentResponse
    {
        public string? Url { get; set; }
        public string? Token { get; set; }
        public int? flowOrder { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
    }

    public class PaymentStatus
    {
        public int? flowOrder { get; set; }
        public string? CommerceOrder { get; set; }
        public string? RequestDate { get; set; }
        public int? status { get; set; }
        public string? Subject { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
        public string? Payer { get; set; }
        public PaymentData? paymentData { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
    }

    public class PaymentData
    {
        public string? Date { get; set; }
        public string? media { get; set; }
        public string? ConversionDate { get; set; }
        public decimal? ConversionRate { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Balance { get; set; }
        public string? TransferDate { get; set; }
    }
}