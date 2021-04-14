using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ThemisMobile.Interfaces
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
