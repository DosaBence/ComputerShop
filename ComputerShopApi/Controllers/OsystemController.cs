using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ComputerShopApi.Controllers
{

    //ORM-el egyszerűbben történik a végpontok közötti lekérdezés, mint sima sql statement-el
    [Route("osystems")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public ComputerShopDbContext context = new ComputerShopDbContext();


        [HttpPost("newOsystem")]
        public object AddnewOsystem([FromBody] AddNewOsystemDto addNewOsystemDto)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDto.Name,
                Version = addNewOsystemDto.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            if (osystem != null)
            {
                context.Osystems.Add(osystem);
                context.SaveChanges();

                return StatusCode(200, new {message = "Sikeres felvétel", result = osystem });
            }
            return StatusCode(400, new { message = "Sikertelen felvétel", result = osystem });
        }


        [HttpGet("allOsystem")]
        public object AllOsystem()
        {
            var osystems = context.Osystems.ToList();

            if(osystems.Count > 0) 
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystems });
            }
            return StatusCode(404, new { message = "Üres tábla.", result = osystems });

        }

        [HttpGet("osystemById")]
        public object OsystemById([FromQuery] Guid id)
        {
            var osystem = context.Osystems.SingleOrDefault(o => o.Id ==id);

            if (osystem != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystem });

            }
            return StatusCode(404, new { message = "Nincs ilyen Id-vel Osystem.", result = osystem });

        }


        //mikor kell fromquery és mikor frombody?
        [HttpPut("updateOsystem")]
        public object UpdateOsystem([FromQuery] Guid id, [FromBody] UpdateOsystemDto updateOsystemDto)
        {
            var osystem = context.Osystems.Find(id);

            if (osystem != null)
            {
                osystem.Name = updateOsystemDto.Name;
                osystem.Version = updateOsystemDto.Version;
                osystem.UpdateTime = DateTime.Now;

                context.Osystems.Update(osystem);
                context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = osystem });
            }
            return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem.", result = osystem });
        }


        [HttpDelete ("deleteOsystem")]

        public object DeleteOsystem([FromQuery] Guid id)
        {
            var osystem = context.Osystems.FirstOrDefault(o =>o.Id==id);
            if (osystem != null)
            {
                context.Osystems.Remove(osystem);
                context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés"});

            }
            return StatusCode(404, new { message = "Nincs ilyen Id-vel Osystem."});

        }
    }
}
