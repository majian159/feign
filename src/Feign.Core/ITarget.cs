using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Rabbit.Feign
{
    public interface ITarget<T>
    {
        string Name { get; }
        string Url { get; }
        HttpRequestMessage Apply(RequestTemplate template);
    }
}
