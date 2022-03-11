using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoTagBackEnd.Dto;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PortalController : Controller
    {
        private readonly AutoTagContext _context;
        private readonly IMapper _mapper;

        public PortalController(AutoTagContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/portal
        [Authorize]
        [HttpPost]
        public IEnumerable<PortalDto> GetAll()
        {
            var listPortalDto = _mapper.Map<List<PortalDto>>(_context.Portals.ToList());
            return listPortalDto;
        }

        // GET api/portal/5
        [HttpPost]
        public PortalDto Get([FromBody] string id)
        {
            int intId = int.Parse(id);
            var portalDto = _mapper.Map<PortalDto>(_context.Portals.Where(p=>p.Id == intId).SingleOrDefault());
            return portalDto;
        }

        // POST api/portal
        public record PortalPostRequest(string Code, string Name);
        [HttpPost]
        public IActionResult Add([FromBody] PortalPostRequest body)
        {
            if(string.IsNullOrEmpty(body.Code) || string.IsNullOrEmpty(body.Name))
            {
                return BadRequest();
            }

            if (_context.Portals.Any())
            {
                int maxOrder = _context.Portals.Max(p => p.Order);
                _context.Portals.Add(new Portal()
                {
                    Code = body.Code,
                    Name = body.Name,
                    Order = maxOrder + 1
                });
            }
            else
            {
                _context.Portals.Add(new Portal()
                {
                    Code = body.Code,
                    Name = body.Name,
                    Order = 1
                });
            }

            _context.SaveChanges();
            return NoContent();
        }
    }
}

