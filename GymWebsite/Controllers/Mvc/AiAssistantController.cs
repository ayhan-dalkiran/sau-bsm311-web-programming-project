using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GymWebsite.Controllers.Mvc
{
    public class AiAssistantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAIPlan(int age, int height, int weight, string gender, string goal)
        {
            string apiKey = "AIzaSyAFK1iBsO1xN5IQLRWMTw5cmwrMGmGBrdE";
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            string userPrompt = $@"
Rol: Sen profesyonel bir spor koçu ve diyetisyensin.
Danışan: {age} yaş, {height}cm, {weight}kg, {gender}. Hedef: {goal}.

Görev: HTML formatında (<ul>, <li>, <strong>, <br>) şunları hazırla:
1. <h4 class='text-primary'>Günlük Kalori İhtiyacı</h4> (Kısa özet)
2. <h4 class='text-primary mt-3'>1 Günlük Beslenme</h4> (Sabah, Öğle, Akşam)
3. <h4 class='text-primary mt-3'>Antrenman</h4> (4 temel hareket)

Önemli: Markdown kullanma, sadece HTML etiketleri kullan.";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = userPrompt }
                        }
                    }
                }
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "Hata" });

                var jsonNode = JsonNode.Parse(responseString);
                string aiResponse = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                return Json(new { success = true, message = aiResponse ?? "..." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChatWithCoach(string userMessage, string contextHistory)
        {
            string apiKey = "AIzaSyAFK1iBsO1xN5IQLRWMTw5cmwrMGmGBrdE";
            string apiUrl = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={apiKey}";

            string fullPrompt = $@"
Rol: Sen profesyonel bir spor koçusun.
Mevcut Plan (HTML): '{contextHistory}'.
Kullanıcı İsteği: {userMessage}

KURALLAR:
1. EĞER PLAN DEĞİŞMELİYSE:
   Cevaba SADECE '[GUNCELLE]' kelimesiyle başla.
   Ardından yeni planın HTML kodlarını yaz.
   Sohbet cümlesi kurma.

2. EĞER SADECE SOHBETSE:
   Normal, samimi cevap ver. HTML kullanma.
";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = fullPrompt }
                        }
                    }
                }
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "Hata" });

                var jsonNode = JsonNode.Parse(responseString);
                string aiResponse = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                return Json(new { success = true, message = aiResponse ?? "..." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
