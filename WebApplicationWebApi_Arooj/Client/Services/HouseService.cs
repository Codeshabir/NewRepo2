using Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Client.Services
{
    public class HouseService
    {
        private readonly HttpClient _httpClient;

        public HouseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7118"); // Replace with your Web API URL
        }


        //public async Task<IEnumerable<HouseDTO>> GetHousesAsync()
        //{
        //    var response = await _httpClient.GetAsync("/api/Houses");

        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<IEnumerable<HouseDTO>>(content);
        //}




        public async Task<HouseDTO> GetHouseAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Houses/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HouseDTO>(content);
        }

        public async Task<IEnumerable<HouseDTO>> GetHousesAsync()
        {
            HttpResponseMessage response = null;

            try
            {
                response = await _httpClient.GetAsync("/api/Houses");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var houses = JsonConvert.DeserializeObject<IEnumerable<HouseDTO>>(content);
                return houses ?? Enumerable.Empty<HouseDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Call Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                // Return an API Response in case of any exception
                if (response != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response Content: {responseContent}");
                }

                // Return an empty collection or null
                return Enumerable.Empty<HouseDTO>();
            }
        }

      

        public async Task<HouseDTO> CreateHouseAsync(HouseDTO house)
        {
            HttpResponseMessage response = null;

            try
            {
                if (house.ImageData != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await house.ImageData.CopyToAsync(memoryStream);
                        house.ImageInBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

              

                // Send the request
                response = await _httpClient.PostAsJsonAsync("/api/Houses", house);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<HouseDTO>(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Call Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                // Return an API Response in case of any exception
                if (response != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response Content: {responseContent}");
                }

                return new HouseDTO(); // Return an empty HouseDTO in case of an exception
            }
        }

        public async Task UpdateHouseAsync(HouseDTO house)
        {
            var houseJson = JsonConvert.SerializeObject(house);
            var content = new StringContent(houseJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/Houses/{house.Id}", content);
            response.EnsureSuccessStatusCode();
        }



        public async Task DeleteHouseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Houses/{id}");
            response.EnsureSuccessStatusCode();
        }


    }



}
