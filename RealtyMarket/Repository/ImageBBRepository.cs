using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Repository
{
    public class ImageBBRepository
    {
        public readonly string BaseUrl = "https://api.imgbb.com/1/upload?key";

        public readonly string ApiKey = "497e3bb7c14b3b90409c6292522ecb78";

        private readonly HttpClient _httpClient;


        public ImageBBRepository(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<string> Add(ImageSource imageSource)
        {
            try
            {
                if (imageSource is StreamImageSource streamImageSource)
                {
                    var stream = await streamImageSource.Stream(CancellationToken.None);

                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        var content = new MultipartFormDataContent
                    {
                        { new StreamContent(memoryStream), "image", "uploaded_image.png" }
                    };

                        var response = await _httpClient.PostAsync($"{BaseUrl}={ApiKey}", content);
                        response.EnsureSuccessStatusCode();

                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                        return jsonData.data.url;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            throw new InvalidOperationException("ImageSource должен быть StreamImageSource.");
        }

        public ImageSource Get(string imageUrl)
        {
            return ImageSource.FromUri(new Uri(imageUrl));
        }
    }
}
