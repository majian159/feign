using System.Net.Http;

namespace Rabbit.Feign
{
    public class HardCodedTarget<T> : ITarget<T>
    {
        public HardCodedTarget(string url):this(url,url)
        {
            
        }
        /// <inheritdoc />
        public HardCodedTarget(string name, string url)
        {
            Name = name;
            Url = url;
        }

        #region Implementation of ITarget<T>

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Url { get; }

        /// <inheritdoc />
        public HttpRequestMessage Apply(RequestTemplate template)
        {
            return template.Request();
        }

        #endregion
    }
}