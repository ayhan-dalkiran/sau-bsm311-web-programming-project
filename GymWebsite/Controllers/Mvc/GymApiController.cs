using GymWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymWebsite.Controllers.Mvc {
    // [Route("api/[controller]")] : Bu, adresi 'localhost/api/gymapi' yapar.
    [Route("api/[controller]")]
    [ApiController] // Bu attribute, bunun bir API servisi olduğunu belirtir.
    public class GymApiController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public GymApiController(ApplicationDbContext context) {
            _context = context;
        }

        // 1. ÖRNEK: Tüm Hizmetleri Getiren API
        // Adres: /api/gymapi/services
        [HttpGet("services")]
        public IActionResult GetAllServices() {
            // LINQ Sorgusu: Sadece isim ve fiyatı seçiyoruz (Projection)
            var services = _context.GymServices
                                   .Select(x => new {
                                       HizmetAdi = x.Name,
                                       Ucret = x.Price,
                                       Sure = x.DurationInMinutes + " dk"
                                   })
                                   .ToList();

            return Ok(services); // JSON formatında döner
        }

        // 2. ÖRNEK: Fiyata Göre Filtreleyen API (LINQ Where kullanımı)
        // Adres: /api/gymapi/services/filter?maxPrice=500
        [HttpGet("services/filter")]
        public IActionResult GetServicesByPrice(decimal maxPrice) {
            // İSTERDEKİ LINQ SORGUSU BURADA:
            var filteredServices = _context.GymServices
                                           .Where(s => s.Price <= maxPrice) // Fiyatı parametreden düşük olanlar
                                           .OrderBy(s => s.Price)           // Fiyata göre sırala
                                           .Select(s => new {               // İsim ve Fiyatı al
                                               s.Name,
                                               s.Price
                                           })
                                           .ToList();

            if (!filteredServices.Any()) {
                return NotFound(new { message = "Bu fiyat aralığında hizmet bulunamadı." });
            }

            return Ok(filteredServices);
        }
    }
}