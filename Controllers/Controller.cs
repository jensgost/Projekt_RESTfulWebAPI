using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt_RESTfulWebAPI.Data;
using Projekt_RESTfulWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Projekt_RESTfulWebAPI.DTO.V1;

namespace Projekt_RESTfulWebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/geo-comments")]
    [ApiController]
    [ApiVersion("1.0")]
    public class Controller : ControllerBase
    {
        private readonly OurDbContext _context;
        private readonly UserManager<User> _userManager;
        public Controller(OurDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessageDTO>> GeoMessageDTO (int id)
        {
            var geoMessage = await _context.GeoMessages.Include(g => g.Message).FirstOrDefaultAsync(g => g.Id == id);

            if (geoMessage == null)
                return NotFound();

            return Ok(geoMessage);
            var geoMessageDTO = new GeoMessageDTO
            {
                Id = geoMessage.Id,
                Message = geoMessage.Message.Body,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            return Ok(geoMessageDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessageDTO>>> GetGeoMessages()
        {
            return await _context.GeoMessages
                .Include(g => g.Message)
                .Select(g => new GeoMessageDTO
                {
                    Id = g.Id,
                    Message = g.Message.Body,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude
                }
                )
                .ToListAsync();
        }
    }
}
