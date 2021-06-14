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
using Projekt_RESTfulWebAPI.DTO.V2;

namespace Projekt_RESTfulWebAPI.Controllers.V2
{
    [Route("api/v{version:apiVersion}/geo-comments")]
    [ApiController]
    [ApiVersion("2.0")]
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
        public async Task<ActionResult<GetGeoMessageDTO>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.Include(g => g.Message).FirstOrDefaultAsync(g => g.Id == id);

            if (geoMessage == null)
                return NotFound();

            var geoMessageDTO = new GetGeoMessageDTO
            {
                Message = new GetGeoMessageDTO
                {
                    Title = geoMessage.Title,
                    Body = geoMessage.Body,
                    Author = geoMessage.Author
                },
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            return Ok(geoMessageDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessageDTO>>> GetGeoMessages()
        {
            return await _context.GeoMessages
                .Select(g =>
                    new GeoMessageDTO
                    {
                        Message = g.Message,
                        Latitude = g.Latitude,
                        Longitude = g.Longitude
                    }
                )
                .ToListAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GeoMessageDTO>> CreateGeoMessage([FromQuery] Guid ApiKey, GeoMessageDTO geoMessageDTO)
        {
            if (geoMessageDTO == null)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(this.User);
            var newGeoMessage = new GeoMessage
            {
                Message = geoMessageDTO.Message,
                Longitude = geoMessageDTO.Longitude,
                Latitude = geoMessageDTO.Latitude
            };

            await _context.AddAsync(newGeoMessage);
            await _context.SaveChangesAsync();

            var getGeoMessage = new GeoMessageDTO
            {
                Message = newGeoMessage.Message,
                Longitude = newGeoMessage.Longitude,
                Latitude = newGeoMessage.Latitude
            };

            return CreatedAtAction(nameof(GeoMessageDTO), new { id = newGeoMessage.Id }, getGeoMessage);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeoMessage>> DeleteGeoMessage([FromQuery] Guid ApiKey, int id)
        {
            var geoMessage = await _context.GeoMessages.FirstOrDefaultAsync(g => g.Id == id);

            _context.Remove(geoMessage);
            await _context.SaveChangesAsync();

            return Ok(geoMessage);
        }
    }
}
