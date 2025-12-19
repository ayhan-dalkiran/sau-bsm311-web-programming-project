using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GymWebsite.Controllers
{
    public class AIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAIPlan(int age, int height, int weight, string gender, string goal)
        {
            // 1. BURAYA API KEY'İNİ YAPIŞTIR (Boşluksuz olduğundan emin ol)
            string apiKey = "AIzaSyAFK1iBsO1xN5IQLRWMTw5cmwrMGmGBrdE";

            // GÜNCELLEME BURADA: Model ismini 'gemini-1.5-flash' yaptık. 
            // Bu model şu an en güncel ve hatasız çalışan modeldir.
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            // 2. Prompt (İstek Mesajı)
            string userPrompt = $@"
                Rol: Sen profesyonel bir spor koçu ve diyetisyensin.
                Danışan: {age} yaş, {height}cm, {weight}kg, {gender}. Hedef: {goal}.
                
                Görev: HTML formatında (<ul>, <li>, <strong>, <br>) şunları hazırla:
                1. <h4 class='text-primary'>Günlük Kalori İhtiyacı</h4> (Kısa özet)
                2. <h4 class='text-primary mt-3'>1 Günlük Beslenme</h4> (Sabah, Öğle, Akşam)
                3. <h4 class='text-primary mt-3'>Antrenman</h4> (4 temel hareket)
                
                Önemli: Markdown kullanma, sadece HTML etiketleri kullan.";

            // 3. Veri Paketi
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

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.PostAsync(apiUrl, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonNode = JsonNode.Parse(responseString);

                        // JSON içinden cevabı çekiyoruz
                        string aiResponse = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                        if (string.IsNullOrEmpty(aiResponse))
                        {
                            return Json(new { success = false, message = "AI boş cevap döndürdü." });
                        }

                        return Json(new { success = true, message = aiResponse });
                    }
                    else
                    {
                        // Hata olursa yine detaylı görelim
                        return Json(new { success = false, message = $"Hata Kodu: {response.StatusCode}. Detay: {responseString}" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Bağlantı Hatası: " + ex.Message });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChatWithCoach(string userMessage, string contextHistory)
        {
            // API KEY'İNİ UNUTMA
            string apiKey = "AIzaSyAFK1iBsO1xN5IQLRWMTw5cmwrMGmGBrdE";

            string apiUrl = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={apiKey}";

            // PROMPT GÜNCELLEMESİ:
            string fullPrompt = $@"
        Rol: Sen profesyonel bir spor koçusun.
        Mevcut Plan (HTML): '{contextHistory}'.
        Kullanıcı İsteği: {userMessage}
        
        KURALLAR:
        1. EĞER PLAN DEĞİŞMELİYSE:
           Cevaba SADECE '[GUNCELLE]' kelimesiyle başla.
           Hemen ardından yeni planın HTML kodlarını yaz.
           ASLA 'Tabii ki', 'Merhaba', 'İşte planın' gibi sohbet cümleleri kurma. Sadece kod ver.

        2. EĞER SADECE SOHBETSE:
           Normal, samimi bir cevap ver. HTML kodu kullanma.
        ";

            var payload = new
            {
                contents = new[]
                {
            new { parts = new[] { new { text = fullPrompt } } }
        }
            };

            // ... Kodun geri kalanı aynı (JsonSerializer, HttpClient vb.) ...

            // (Kodun geri kalanını kopyalamana gerek yok, sadece yukarıdaki prompt kısmını değiştirmen yeterli)
            // Ama garanti olsun dersen controllerın tamamını önceki mesajdan alıp promptu değiştirebilirsin.

            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.PostAsync(apiUrl, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Hata" });

                    var jsonNode = JsonNode.Parse(responseString);
                    string aiResponse = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                    return Json(new { success = true, message = aiResponse ?? "..." });
                }
                catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
            }
        }

    }
}