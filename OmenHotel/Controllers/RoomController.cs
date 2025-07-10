using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmenHotel.Models;

namespace OmenHotel.Controllers
{
    public class RoomController : Controller
    {
        public async Task<IActionResult> RoomIndex()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id=-2092174&search_type=CITY&arrival_date=2025-08-01&departure_date=2025-08-10&adults=1&children_age=0%2C17&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=USD&location=US"),
                Headers =
                {
                    { "x-rapidapi-key", "9b2ad97b7dmsh73dc14b9e6ed0e7p1d4084jsnb1752d00f239" },
                    { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<HotelRoomModel>(body);
                if (apiResponse?.data?.hotels == null)
                {
                    return View(new List<HotelRoomModel.Hotel>()); 
                }

                return View(apiResponse.data.hotels);
            }
        }
        public async Task<IActionResult> RoomDetail(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id=-2092174&search_type=CITY&arrival_date=2025-08-01&departure_date=2025-08-10&adults=1&children_age=0%2C17&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=USD&location=US"),
                Headers =
                {
                    { "x-rapidapi-key", "9b2ad97b7dmsh73dc14b9e6ed0e7p1d4084jsnb1752d00f239" },
                    { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
                },
            };
            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode(); // HTTP hata kodları için istisna fırlatır
                    var body = await response.Content.ReadAsStringAsync();

                    // Tüm API yanıtını HotelRoomModel'e doğru şekilde deserialize edin
                    var apiResponse = JsonConvert.DeserializeObject<OmenHotel.Models.HotelRoomModel>(body);

                    // Genel yanıtın ve verilerin geçerli olup olmadığını kontrol edin
                    if (apiResponse == null || apiResponse.data == null || apiResponse.data.hotels == null)
                    {
                        // Veri veya otel yoksa, Not Found veya boş bir model döndürün
                        return NotFound("Otel verisi bulunamadı veya API yanıtı geçersiz.");
                    }

                    // Oteller listesinden kimliğe göre belirli oteli bulun
                    // Rota'dan gelen 'id'nin 'hotel_id' ile eşleştiği varsayılıyor.
                    var hotel = apiResponse.data.hotels.FirstOrDefault(h => h.hotel_id == id);

                    if (hotel == null)
                    {
                        // Verilen ID'ye sahip otel bulunamazsa, Not Found döndürün
                        return NotFound($"ID {id} ile otel bulunamadı.");
                    }

                    return View(hotel); // Bulunan Hotel nesnesini görünüme iletin
                }
            }
            catch (HttpRequestException e)
            {
                // İstisna günlüğe kaydedin (örn. bir günlük çerçevesi kullanarak)
                Console.WriteLine($"İstek istisnası: {e.Message}");
                return StatusCode(500, "API'den veri alınırken hata oluştu.");
            }
            catch (JsonSerializationException e)
            {
                // İstisna günlüğe kaydedin
                Console.WriteLine($"JSON deserializasyon istisnası: {e.Message}");
                return StatusCode(500, "API yanıtı işlenirken hata oluştu.");
            }
            catch (Exception e)
            {
                // Diğer beklenmedik hataları yakalayın
                Console.WriteLine($"Beklenmedik bir hata oluştu: {e.Message}");
                return StatusCode(500, "Beklenmedik bir hata oluştu.");
            }
        }
    }
}
