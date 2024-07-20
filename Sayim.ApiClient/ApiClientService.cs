using Microsoft.Extensions.Options;
using Sayim.ApiClient.Models;
using Sayim.ApiClient.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sayim.ApiClient
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        public ApiClientService(IOptions<ApiClientOptions> apiClientOptions)
        {
            var options = apiClientOptions.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(options.ApiBaseAddress)
            };
        }
        public async Task<List<AmbarAdres>?> GetAmbarAdres(string ambarNo)
        {
            var url = $"/api/AmbarAdres?ambarNo={ambarNo}";
            return await _httpClient.GetFromJsonAsync<List<AmbarAdres>>(url);
        }
        public async Task<bool> AmbarAdresKontrol(string ambarNo, string adres)
        {
            var url = $"/api/AmbarAdres/Kontrol?ambarNo={ambarNo}&adres={adres}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        public async Task<List<Ambar>?> GetAmbar()
        {
            var url = "/api/Ambar";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Ambar>>();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        public async Task<Kullanici?> GetKullanici(string kullaniciKodu, string sifre)
        {
            var url = $"/api/Kullanici?kullaniciKodu={kullaniciKodu}&sifre={sifre}";
            return await _httpClient.GetFromJsonAsync<Kullanici>(url);
        }
        public async Task<bool> TopNoKontrol(string topNo)
        {
            var url = $"/api/KumasTop/TopNoKontrol?topNo={topNo}";
            return await _httpClient.GetFromJsonAsync<bool>(url);
        }
        public async Task<decimal> KumasTopNetMt(string bilgi)
        {
            var url = $"/api/KumasTop/NetMt?bilgi={bilgi}";
            return await _httpClient.GetFromJsonAsync<decimal>(url);
        }
        public async Task<List<Personel>?> GetPersoneller(string kullaniciKodu)
        {
            var url = $"/api/Personel?kullaniciKodu={kullaniciKodu}";
            return await _httpClient.GetFromJsonAsync<List<Personel>>(url);
        }
        public async Task<bool> PartiNoKontrol(string partiNo)
        {
            var url = $"/api/SiparisParti/PartiNoKontrol?partiNo={partiNo}";
            return await _httpClient.GetFromJsonAsync<bool>(url);
        }
        public async Task<decimal> SiparisPartiMiktar1(string partiNo)
        {
            var url = $"/api/SiparisParti/SiparisPartiMiktar1?partiNo={partiNo}";
            return await _httpClient.GetFromJsonAsync<decimal>(url);
        }
        public async Task<bool> SeriNoKontrol(string seriNo)
        {
            try
            {
                var url = $"/api/ZZZ_StokSayim_SeriNo_/SeriNoKontrol?seriNo={seriNo}";
                return await _httpClient.GetFromJsonAsync<bool>(url);

            }
            catch (Exception ex)
            {
                // Hataları işleyin (örneğin, bir log yazma veya kullanıcıya bir mesaj gösterme)
                Console.WriteLine($"Error in SeriNoKontrol: {ex.Message}");
                return false;
            }
        }

        public async Task<decimal> SeriNoMiktar1(string seriNo)
        {
            var url = $"/api/ZZZ_StokSayim_SeriNo_/SeriNoMiktar1?seriNo={seriNo}";
            return await _httpClient.GetFromJsonAsync<decimal>(url);
        }
        public async Task<int> CountAsync(string tableName, string columnName, string filter = null, bool where = false)
        {
            var url = $"/api/SiparisParti?tableName={tableName}&columnName={columnName}&filter={filter}&where={where}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var count = await response.Content.ReadFromJsonAsync<int>();
                return count;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        public async Task<int> GetMaxSayimNoAsync()
        {
            var url = "/api/ZZZ_StokSayim_SeriNo_/GetMaxSayimNo";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

        public async Task<bool> InsertStokSayimAnaAsync(StokSayimAna stokSayimAna)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/ZZZ_StokSayim_SeriNo_/InsertStokSayimAna", stokSayimAna);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> InsertStokSayimAsync(StokSayim stokSayim)
        {
            var url = "/api/ZZZ_StokSayim_SeriNo_/InsertStokSayim";
            var response = await _httpClient.PostAsJsonAsync(url, stokSayim);
            return response.IsSuccessStatusCode;
     
        }

    }
}
