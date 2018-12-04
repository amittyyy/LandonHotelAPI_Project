using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandonAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly HotelApiDbContext _dbContext;

        public RoomsController(HotelApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name= nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }

        //Get /rooms/{roomid}  
        [HttpGet ("{roomId}", Name = nameof(GetRoomById))]
        public async Task<ActionResult<Room>> GetRoomById(Guid roomId)
        {
            var entity = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);
            if(entity == null)
            {
                return NotFound();
            }

            var resource = new Room
            {
                Href = Url.Link(nameof(GetRoomById), new { roomId = entity.Id }),
                Name = entity.Name,
                Rate = entity.Rate / 100.0m
            };

            return resource;
        }
    }
}