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

        public record PortalAccountSaveRequest(int? Id, int PortalId, string Run, string Password);
        [Authorize]
        [HttpPost]
        public IActionResult Save([FromBody] PortalAccountSaveRequest body)
        {
            // verificar que vengan los datos
            if (string.IsNullOrEmpty(body.Run))
            {
                return Ok(new { error = new[] { new { type = "run", message = "Debes ingresar un rut" } } });
            }
            if (string.IsNullOrEmpty(body.Password))
            {
                return Ok(new { error = new[] { new { type = "password", message = "Debes ingresar una contraseña" } } });
            }
            int respPortalAccountId;
            if (body.Id == null)
            {
                // Obtener portal
                Portal portal = _context.Portals.SingleOrDefault(p => p.Id == body.PortalId);
                if (portal == null)
                {
                    throw new Exception("No existe el portal con id: " + body.PortalId.ToString());
                }
                // verificar que no exista otra autopista con el mismo rut
                if (_context.PortalAccounts.Any(pa => pa.AccountId == this.CurrentAccount.Id && pa.Run == body.Run && pa.PortalId == body.PortalId && !pa.Removed))
                {
                    string message = String.Format("Ya existe una credencial de {0} con el rut {1}", portal.Name, body.Run.ToLower());
                    return Ok(new { error = new[] { new { type = "portalId", message = message } } });
                }
                // crear nuevo portalAccount
                PortalAccount portalAccount = new PortalAccount()
                {
                    AccountId = this.CurrentAccount.Id,
                    PortalId = body.PortalId,
                    Run = body.Run.ToLower(),
                    Password = body.Password,
                    Enabled = true,
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
                portalAccount.Run = body.Run.ToLower();
                portalAccount.Password = body.Password;
                portalAccount.Enabled = true;
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

            return Ok(respPortalAccount);
        }

        public record PortalAccountRemoveMultiplesRequest(int[] Ids);
        [HttpPost]
        public IActionResult RemoveMultiples([FromBody] PortalAccountRemoveMultiplesRequest body)
        {
            List<PortalAccount> listPortalAccount = _context.PortalAccounts.Where(p => body.Ids.Contains(p.Id) && p.AccountId == this.CurrentAccount.Id).ToList();
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
            PortalAccount portalAccount = _context.PortalAccounts.Where(p => p.Id == body.Id && p.AccountId == this.CurrentAccount.Id).SingleOrDefault();
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

