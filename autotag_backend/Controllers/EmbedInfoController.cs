﻿using System.Text.Json;
using AutoTagBackEnd.AppModels;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using AutoTagBackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EmbedInfoController : AppController
    {
        private readonly PbiEmbedService pbiEmbedService;
        private readonly IOptions<AzureAd> azureAd;
        private readonly IOptions<PowerBI> powerBI;
        private readonly AutoTagContext _context;

        public EmbedInfoController(AutoTagContext context, PbiEmbedService pbiEmbedService, IOptions<AzureAd> azureAd, IOptions<PowerBI> powerBI)
        {
            _context = context;
            this.pbiEmbedService = pbiEmbedService;
            this.azureAd = azureAd;
            this.powerBI = powerBI;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoDashboard()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportDashboardId), account.Email, rolCode);
            return embedParams;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoTransits()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportTransitsId), account.Email, rolCode);
            return embedParams;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoFreewayAnalysis()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportFreewayAnalysisId), account.Email, rolCode);
            return embedParams;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoVehicleAnalysis()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportVehicleAnalysisId), account.Email, rolCode);
            return embedParams;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoDocumentList()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportDocumentListId), account.Email, rolCode);
            return embedParams;
        }

        [Authorize]
        [HttpPost]
        public EmbedParams GetEmbedInfoVehicleRanking()
        {
            string configValidationResult = ConfigValidatorService.ValidateConfig(azureAd, powerBI);
            if (configValidationResult != null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            Account account = _context.Accounts.Include(db => db.Role).Where(a => a.Id == this.CurrentAccount.Id).SingleOrDefault();
            if (account == null)
            {
                HttpContext.Response.StatusCode = 400;
                throw new Exception("No se encontró el cliente");
            }
            string rolCode = account.Role.Code;

            EmbedParams embedParams = pbiEmbedService.GetEmbedParams(new Guid(powerBI.Value.WorkspaceId), new Guid(powerBI.Value.ReportVehicleRankingId), account.Email, rolCode);
            return embedParams;
        }
    }
}

