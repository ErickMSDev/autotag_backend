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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PortalAccountController : AppController
    {
        private readonly AutoTagContext _context;

        public PortalAccountController(AutoTagContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public IEnumerable<PortalAccountDto> GetAll()
        {
            List<PortalAccountDto> listPortalAccount =
                (from pa in _context.PortalAccounts
                where
                    pa.AccountId == this.CurrentAccount.Id
                    && !pa.Removed
                select new PortalAccountDto
                {
                    Id = pa.Id,
                    PortalId = pa.Portal.Id,
                    PortalCode = pa.Portal.Code,
                    PortalName = pa.Portal.Name,
                    Run = pa.Run,
                    Password = pa.Password,
                    Enabled = pa.Enabled,
                    StatusCode = PortalAccountHelper.GetStatus(pa).Code,
                    StatusName = PortalAccountHelper.GetStatus(pa).Name,
                    StatusDescription = PortalAccountHelper.GetStatus(pa).Description
                }).ToList();

            return listPortalAccount;
        }

        public record PortalAccountGetRequest(int Id);
        [Authorize]
        [HttpPost]
        public PortalAccountDto Get([FromBody] PortalAccountGetRequest body)
        {
            PortalAccountDto portalAccount =
                (from pa in _context.PortalAccounts
                 where
                     pa.AccountId == this.CurrentAccount.Id
                     && !pa.Removed
                     && pa.Id == body.Id
                 select new PortalAccountDto
                 {
                     Id = pa.Id,
                     PortalId = pa.Portal.Id,
                     PortalCode = pa.Portal.Code,
                     PortalName = pa.Portal.Name,
                     Run = pa.Run,
                     Password = pa.Password,
                     Enabled = pa.Enabled,
                     StatusCode = PortalAccountHelper.GetStatus(pa).Code,
                     StatusName = PortalAccountHelper.GetStatus(pa).Name,
                     StatusDescription = PortalAccountHelper.GetStatus(pa).Description
                 }).SingleOrDefault();

            return portalAccount;
        }

        public record PortalAccountSaveRequest(int? Id, int PortalId, string Run, string Password, bool Enabled);
        [Authorize]
        [HttpPost]
        public PortalAccountDto Save([FromBody] PortalAccountSaveRequest body)
        {
            if (string.IsNullOrEmpty(body.Run) || string.IsNullOrEmpty(body.Password))
            {
                return null;
            }
            int respPortalAccountId;
            if (body.Id == null)
            {
                PortalAccount portalAccount = new PortalAccount()
                {
                    AccountId = this.CurrentAccount.Id,
                    PortalId = body.PortalId,
                    Run = body.Run,
                    Password = body.Password,
                    Enabled = body.Enabled,
                    HasPendingProcess = true,
                    CreationDate = DateTime.Now
                };
                _context.PortalAccounts.Add(portalAccount);
                _context.SaveChanges();
                respPortalAccountId = portalAccount.Id;
            }
            else
            {
                PortalAccount portalAccount = _context.PortalAccounts.Where(p => p.Id == body.Id).SingleOrDefault();
                if (portalAccount == null)
                {
                    throw new Exception("No existe el portalAccount para el id: " + body.Id.ToString());
                }
                portalAccount.PortalId = body.PortalId;
                portalAccount.Run = body.Run;
                portalAccount.Password = body.Password;
                portalAccount.Enabled = body.Enabled;
                portalAccount.HasError = false;
                portalAccount.HasLoginError = false;
                portalAccount.ErrorMessage = null;
                _context.SaveChanges();
                respPortalAccountId = portalAccount.Id;
            }
            PortalAccountDto respPortalAccount =
                (from pa in _context.PortalAccounts
                 where
                     pa.AccountId == this.CurrentAccount.Id
                     && !pa.Removed
                     && pa.Id == respPortalAccountId
                 select new PortalAccountDto
                 {
                     Id = pa.Id,
                     PortalId = pa.Portal.Id,
                     PortalCode = pa.Portal.Code,
                     PortalName = pa.Portal.Name,
                     Run = pa.Run,
                     Password = pa.Password,
                     Enabled = pa.Enabled,
                     StatusCode = PortalAccountHelper.GetStatus(pa).Code,
                     StatusName = PortalAccountHelper.GetStatus(pa).Name,
                     StatusDescription = PortalAccountHelper.GetStatus(pa).Description
                 }).SingleOrDefault();

            return respPortalAccount;
        }

        public record PortalAccountRemoveMultiplesRequest(int[] Ids);
        [HttpPost]
        public IActionResult RemoveMultiples([FromBody] PortalAccountRemoveMultiplesRequest body)
        {
            List<PortalAccount> listPortalAccount = _context.PortalAccounts.Where(p => body.Ids.Contains(p.Id)).ToList();
            listPortalAccount.ForEach(p => {
                p.Removed = true;
                p.DeletionDate = DateTime.Now;
            });
            _context.SaveChanges();

            return NoContent();
        }

        public record PortalAccountRemoveRequest(int Id);
        [HttpPost]
        public IActionResult Remove([FromBody] PortalAccountRemoveRequest body)
        {
            PortalAccount portalAccount = _context.PortalAccounts.Where(p => p.Id == body.Id).SingleOrDefault();
            if (portalAccount != null)
            {
                portalAccount.Removed = true;
                portalAccount.DeletionDate = DateTime.Now;
                _context.SaveChanges();
            }
            return NoContent();
        }
    }
}

