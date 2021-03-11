using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Imenik.Helper
{
    public class ImenikAPI
    {
        public HttpClient Inital()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:5001");
            return Client;
        }
    }
}
