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
            var geoMessage = await _context.GeoMessages.FirstOrDefaultAsync(g => g.Id == id);

            if (geoMessage == null)
                return NotFound();

            var geoMessageDTO = new GetGeoMessageDTO
            {
                Message = new GetMessageDTO
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
        public async Task<ActionResult<IEnumerable<GetGeoMessageDTO>>> GetGeoMessagesQuery([FromQuery] double minLon, [FromQuery] double minLat, [FromQuery] double maxLon, [FromQuery] double maxLat)
        {
            var geoMessages = await _context.GeoMessages
                .Select(g =>
                    new GetGeoMessageDTO
                    {
                        Message = new GetMessageDTO
                        {
                            Title = g.Title,
                            Body = g.Body,
                            Author = g.Author
                        },
                        Latitude = g.Latitude,
                        Longitude = g.Longitude
                    }
                )
                .ToListAsync();

            if (Request.Query.ContainsKey("minLon") && Request.Query.ContainsKey("minLat") && Request.Query.ContainsKey("maxLon") && Request.Query.ContainsKey("maxLat"))
                geoMessages = geoMessages.Where(g =>
                g.Longitude > minLon && g.Longitude < maxLon && g.Latitude > minLat && g.Latitude < maxLat).ToList();

            return Ok(geoMessages);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetGeoMessageDTO>> CreateGeoMessage([FromQuery] Guid ApiKey, AddGeoMessageDTO addGeoMessage)
        {
            if (addGeoMessage == null)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(this.User);
            var newGeoMessage = new GeoMessage
            {
                Title = addGeoMessage.Message.Title,
                Body = addGeoMessage.Message.Body,
                Author = $"{user.FirstName} {user.LastName}",
                Longitude = addGeoMessage.Longitude,
                Latitude = addGeoMessage.Latitude
            };

            await _context.AddAsync(newGeoMessage);
            await _context.SaveChangesAsync();

            var getGeoMessage = new GetGeoMessageDTO
            {
                Message = new GetMessageDTO
                {
                    Title = newGeoMessage.Title,
                    Body = newGeoMessage.Body,
                    Author = newGeoMessage.Author
                },
                Longitude = newGeoMessage.Longitude,
                Latitude = newGeoMessage.Latitude
            };

            return CreatedAtAction(nameof(GetGeoMessage), new { id = newGeoMessage.Id }, getGeoMessage);
        }
    }
}
