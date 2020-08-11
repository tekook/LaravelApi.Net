using System;
using Tekook.LaravelApi.Endpoints;
using Tekook.LaravelApi.Example.Resources;

namespace Tekook.LaravelApi.Example
{
    internal class Api : LaravelApi.Api
    {
        public CrudEndpoint<User> Users { get; set; }

        public Api()
        {
            this.BaseUrl = new Uri("https://api.dev.local/api/v1");
            this.ClientIdentifier = "Tekook.LaravelApi.Example";
            this.Users = new CrudEndpoint<User>(this, "users");
        }
    }
}