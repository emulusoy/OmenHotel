using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmenHotel.Models; // Modelinizin namespace'i

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
                // API yanıtını doğrudan HotelRoomModel tipine deserialize ediyoruz.
                // Çünkü API yanıtının kök yapısı bu modele uyuyor.
                var apiResponse = JsonConvert.DeserializeObject<HotelRoomModel>(body);

                // Kontrol için: eğer data veya hotels null ise boş bir liste döndür.
                if (apiResponse?.data?.hotels == null)
                {
                    return View(new List<HotelRoomModel.Hotel>()); // Boş bir otel listesi döndür
                }

                // Sadece oteller dizisini view'a gönderiyoruz.
                return View(apiResponse.data.hotels);
            }
        }
    }
}