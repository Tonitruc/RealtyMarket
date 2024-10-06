using Microsoft.Maui.ApplicationModel.Communication;
using RealtyMarket.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace RealtyMarket.Repository
{
    public class RegisteredUserRepository : IServerRepository<RegisteredUser>
    {
        private readonly HttpClient _httpClient;

        public override string Controller => "User";


        public RegisteredUserRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RegisteredUser> GetByEmail(string email)
        {
            try
            {
                var url = GetBasetUrl() + "/" + email;
                HttpResponseMessage response = await _httpClient.GetAsync(
                    GetBasetUrl() + "/" + email);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    RegisteredUser user = JsonSerializer.Deserialize<RegisteredUser>(responseData);

                    return user;
                }
            }
            catch (HttpRequestException ex) {
                Console.WriteLine(ex.Message);
            }
            catch(Exception)
            {

            }

            return null;
        }

        public override Task<bool> GetAll()
        {
            throw new NotImplementedException();
        }


        public override async Task<bool> Add(RegisteredUser entity)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                    GetBasetUrl(), entity);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    RegisteredUser user = JsonSerializer.Deserialize<RegisteredUser>(responseData);

                    return true;
                }
            }
            catch (HttpRequestException) { }

            return false;
        }
    }
}
