using Tekook.LaravelApi.Endpoints;
using Tekook.LaravelApi.Example.Resources;
using Tekook.LaravelApi.Example.Responses;

namespace Tekook.LaravelApi.Example
{
    internal class Api : LaravelApi.Api
    {
        public CrudEndpoint<User, MyCollectionResponse<User>, MyResponse<User>> MyUsers { get; set; }
        public CrudEndpoint<User> Users { get; set; }

        public Api()
        {
            this.ClientIdentifier = "Tekook.LaravelApi.Example";
            this.Users = new CrudEndpoint<User>(this, "users");
            this.MyUsers = new CrudEndpoint<User, MyCollectionResponse<User>, MyResponse<User>>(this, "users");
        }
    }
}