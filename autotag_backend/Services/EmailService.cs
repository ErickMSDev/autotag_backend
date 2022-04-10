using System;
using AutoTagBackEnd.Entities;
using RestSharp;
using RestSharp.Authenticators;

namespace AutoTagBackEnd.Services
{
	public static class EmailService
	{
        private const string APIKey = "749a77335e691c22785bb0c5f8443065-0677517f-063b382d";
        private const string BaseUri = "https://api.mailgun.net/v3";
        private const string Domain = "viasimple.cl";
        private const string SenderAddress = "noreply@viasimple.cl";
        private const string SenderDisplayName = "ViaSimple";

        public static IRestResponse SendEmail(UserEmailOptions userEmailOptions)
        {

            RestClient client = new RestClient
            {
                BaseUrl = new Uri(BaseUri),
                Authenticator = new HttpBasicAuthenticator("api", APIKey)
            };

            RestRequest request = new RestRequest();
            request.AddParameter("domain", Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"{SenderDisplayName} <{SenderAddress}>");

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                request.AddParameter("to", toEmail);
            }

            request.AddParameter("subject", userEmailOptions.Subject);
            request.AddParameter("template", userEmailOptions.Template);

            foreach(var param in userEmailOptions.Params)
            {
                request.AddParameter("v:" + param.Key, param.Value);
            }

            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}

