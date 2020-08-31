using System;
using Tekook.LaravelApi.Contracts;

namespace Tekook.LaravelApi.Example.Responses
{
    internal class MyResponse<T> : IResponse<T> where T : IResource
    {
        private T data;

        public T Data
        {
            get => data; set
            {
                data = value; this.DataSetAt = DateTime.Now;
            }
        }

        public DateTime DataSetAt { get; protected set; }
    }
}