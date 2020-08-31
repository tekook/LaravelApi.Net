using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tekook.LaravelApi.Exceptions;
using FlurlHttpException = Flurl.Http.FlurlHttpException;

namespace Tekook.LaravelApi.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Config config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText("./config.json"));
            Api api = new Api()
            {
                BaseUrl = config.Url,
                AccessToken = config.Token
            };
            try
            {
                var r2 = await api.MyUsers.ShowResponse(1);
                Console.WriteLine($"Fetched {r2.Data.Id} at {r2.DataSetAt}");
                var r3 = await api.MyUsers.IndexResponse();
                Console.WriteLine($"Fetched {r3.Count} users.");
                var response = await api.Users.IndexResponse();
                await response.Chunk(async (x) =>
                {
                    var users = x.Data;
                    Console.WriteLine($"Count: {users.Count}");
                    Thread.Sleep(100);
                    foreach (Resources.User user in users)
                    {
                        Console.WriteLine($"User: {user.Id} -> {user.Lastname}, {user.Firstname}");
                        string f = user.Firstname;
                        user.Firstname = user.Lastname;
                        user.Lastname = f;
                        Resources.User showUser = await api.Users.Update(user);
                        Console.WriteLine($"User: {showUser.Id} -> {showUser.Lastname}, {showUser.Firstname}");
                        Thread.Sleep(100);
                    }
                });
            }
            catch (ApiInvalidRequestException e)
            {
                Console.WriteLine($"Invalid request supplied");
                foreach (string key in e.Error.Errors.Keys)
                {
                    Console.WriteLine($"Error: {key} -> {string.Join(", ", e.Error.Errors[key].ToArray())}");
                }
            }
            catch (ApiServerException e)
            {
                Console.WriteLine($"Caught errro from server: {e.Message}");
            }
            catch (ApiException e)
            {
                Console.WriteLine($"Caught Generic: {e.Message}");
                if (e.InnerException is FlurlHttpException ex)
                {
                    Console.WriteLine(await ex.GetResponseStringAsync());
                }
            }
        }
    }
}