using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tekook.LaravelApi.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Api api = new Api();
            var response = await api.Users.IndexResponse();
            await response.Chunk(api, async (x) =>
            {
                var users = x.Data;
                Console.WriteLine($"Count: {users.Count}");
                Thread.Sleep(1000);
                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.Id} -> {user.Lastname}, {user.Firstname}");
                    var showUser = await api.Users.Show(user);
                    Console.WriteLine($"User: {showUser.Id} -> {showUser.Lastname}, {showUser.Firstname}");
                }
            });
        }
    }
}