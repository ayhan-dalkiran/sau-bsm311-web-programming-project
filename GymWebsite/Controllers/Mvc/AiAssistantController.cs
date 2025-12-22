using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GymWebsite.Controllers.Mvc {
    // [Route("AI/[action]")]  <-- BUNU SİLİYORUZ!
    public class AiAssistantController : Controller {
        // View klasörü "Views/AiAssistant" olmalı
        public IActionResult Index() => View();

        public IActionResult Details(int id) => View();

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id) => View();

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => View();

        [HttpPost]
        public async Task<IActionResult> GetAIPlan(int age, int height, int weight, string gender, string goal) {
            // API Key'inizi buraya yapıştırın
            string apiKey = "AIzaSyB5JDisEFu8f6lLCefpUAEDwXfz0MOFkUE";

            
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            string userPrompt = $@"
Rol: Sen profesyonel bir spor koçu ve diyetisyensin.
Danışan: {age} yaş, {height}cm, {weight}kg, {gender}. Hedef: {goal}.

Görev: HTML formatında (<ul>, <li>, <strong>, <br>) şunları hazırla:
1. <h4 class='text-primary'>Günlük Kalori İhtiyacı</h4> (Kısa özet)
2. <h4 class='text-primary mt-3'>1 Günlük Beslenme</h4> (Sabah, Öğle, Akşam)
3. <h4 class='text-primary mt-3'>Antrenman</h4> (4 temel hareket)

Önemli: Markdown kullanma, sadece HTML etiketleri kullan.";

            var payload = new {
                contents = new[]
                {
                    new { parts = new[] { new { text = userPrompt } } }
                }
            };

            return await CallGeminiApi(apiUrl, payload);
        }

        [HttpPost]
        public async Task<IActionResult> ChatWithCoach(string userMessage, string contextHistory) {
            string apiKey = "AIzaSyAFK1iBsO1xN5IQLRWMTw5cmwrMGmGBrdE";
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

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

            var payload = new {
                contents = new[]
                {
                    new { parts = new[] { new { text = fullPrompt } } }
                }
            };

            return await CallGeminiApi(apiUrl, payload);
        }

        private async Task<IActionResult> CallGeminiApi(string apiUrl, object payload) {
            using var httpClient = new HttpClient();
            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try {
                var response = await httpClient.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, message = "API Hatası: " + response.StatusCode });

                var jsonNode = JsonNode.Parse(responseString);
                // JSON yapısındaki olası null durumlarına karşı güvenli erişim
                string aiResponse = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                return Json(new { success = true, message = aiResponse ?? "Yanıt alınamadı." });
            }
            catch (Exception ex) {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}