using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductWPF.DTO;

namespace ProductWPF.API
{
    internal class Client
    {
        HttpClient httpClient = new HttpClient();
        public Client()
        {
            httpClient.BaseAddress = new Uri(@"https://localhost:7149/");
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            try
            {
                var response = await httpClient.GetAsync("Product/GetProduct");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ProductDTO>>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<StatusDTO>> GetStatuses()
        {
            try
            {
                var response = await httpClient.GetAsync("Status/GetStatuses");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<StatusDTO>>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ProductDTO> EditProduct(ProductDTO product, int id)
        {
            using StringContent jsonContent = new(
                   System.Text.Json.JsonSerializer.Serialize(product),
                   Encoding.UTF8,
                   "application/json");
            using HttpResponseMessage response = await httpClient.PutAsync("Product/" + product.Id, jsonContent);
            response.EnsureSuccessStatusCode();
            // MessageBox.Show(response.StatusCode.ToString());
            return product;
        }

        public async Task DeleteProduct(int id)
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("Product/" + id);
            response.EnsureSuccessStatusCode();

        }

        public async Task AddProductAsync(ProductDTO product)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(product);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("Product/AddProduct", httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(".");
                }
            }
        }

        static Client instance = new();
        public static Client Instance
        {
            get
            {
                if (instance == null)
                    instance = new Client();
                return instance;
            }
        }
    }
}
