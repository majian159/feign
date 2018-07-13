using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Rabbit.Feign
{
    public class RequestTemplate
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public IDictionary<string, string[]> Headers { get; set; }
        public Stream Body { get; set; }

        public HttpRequestMessage Request()
        {
            var sb = new StringBuilder();

            sb
                .Append(Scheme)
                .Append("://")
                .Append(Host);
            if (Port > 0)
            {
                sb.Append(":").Append(Port);
            }

            sb.Append(Path);

            if (!string.IsNullOrEmpty(QueryString) && QueryString != "?")
            {
                sb.Append(QueryString);
            }

            var url = sb.ToString();

            var request = new HttpRequestMessage(new HttpMethod(Method), url);
            if (Headers != null)
            {
                foreach (var item in Headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            if (Body != null && Body != Stream.Null)
            {
                request.Content = new StreamContent(Body);
            }

            return request;
        }

        public class Builder
        {
            private readonly RequestTemplate _template = new RequestTemplate();
            private IDictionary<string, List<string>> _queries;
            private IDictionary<string, List<string>> _headers;
            private string _charset;

            public Builder Queries(IDictionary<string, IEnumerable<string>> queries)
            {
                if (_queries == null)
                {
                    _queries = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                }

                MergeDictionary(queries, _queries);

                return this;
            }

            public Builder Headers(IDictionary<string, IEnumerable<string>> headers)
            {
                if (_headers == null)
                {
                    _headers = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                }

                MergeDictionary(headers, _headers);

                return this;
            }

            /*            public Builder Url(string url)
                        {
                            var uri = new Uri(url);

                            _template.Scheme = uri.Scheme;
                            _template.Host = uri.Host;
                            _template.Port = uri.Port;
                            _template.Path = uri.PathAndQuery;

                            return this;
                        }*/
            /*

                        public Builder Scheme(string scheme)
                        {
                            _template.Scheme = scheme;
                        }
                        public Builder Host(string host)
                        {
                            _template.Host= host;
                        }
                        public Builder Port(int port)
                        {
                            _template.Port= port;
                        }*/

            public Builder Method(string method)
            {
                _template.Method = method.ToUpper();
                return this;
            }

            /*            public Builder DecodeSlash(bool decodeSlash)
                        {
                            _template.DecodeSlash = decodeSlash;
                            return this;
                        }

                        public Builder Append(string value)
                        {
                            _url.Append(value);
                            return this;
                        }

                        public Builder Insert(int index, string value)
                        {
                            _url.Insert(index, value);
                            return this;
                        }*/

            public Builder Charset(string charset)
            {
                _charset = charset;
                return this;
            }

            public RequestTemplate Build()
            {
                /*_template.Queries =
                    _queries.ToDictionary(i => i.Key, i => i.Value.ToArray(), StringComparer.OrdinalIgnoreCase);*/
                _template.Headers =
                    _headers.ToDictionary(i => i.Key, i => i.Value.ToArray(), StringComparer.OrdinalIgnoreCase);

                return _template;
            }

            public Builder Body(Stream body)
            {
                _template.Body = body;
                return this;
            }

            public Builder Body(byte[] body, string charset)
            {
                _template.Body = new MemoryStream(body);
                return Charset(charset);
            }

            public Builder Body(string bodyText)
            {
                return Body(Encoding.UTF8.GetBytes(bodyText), "UTF-8");
            }

            #region Private Method

            private static void MergeDictionary(IDictionary<string, IEnumerable<string>> source, IDictionary<string, List<string>> target)
            {
                foreach (var item in source)
                {
                    if (target.TryGetValue(item.Key, out var list))
                    {
                        list.AddRange(item.Value);
                    }
                    else
                    {
                        list = new List<string>();
                        list.AddRange(item.Value);
                        target[item.Key] = list;
                    }
                }
            }

            #endregion Private Method
        }

        /*        public RequestTemplate Append(string value)
                {
                    return this;
                }

                public RequestTemplate Insert(int pos, string value)
                {
                    return this;
                }

                public string Url()
                {
                    return "";
                }*/
        /*

                public RequestTemplate Query(bool encoded, string name, params string[] values)
                {
                    return this;
                }*/
        /*public RequestTemplate Query(bool encoded, string name, IEnumerable<string> values)
        {
            return this;
        }*/

        /*        public RequestTemplate Queries(IDictionary<string, string[]> queries)
                {
                    return this;
                }

                public RequestTemplate Headers(IDictionary<string, string[]> queries)
                {
                    return this;
                }*/
    }
}