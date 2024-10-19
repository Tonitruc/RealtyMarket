using RealtyMarket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Repository
{
    public abstract class IServerRepository<T> where T : class
    {

        public readonly string BaseUrl = AppSettings.BaseApiUri;
        public abstract string Controller { get; }


        public string GetBasetUrl()
        {
            return BaseUrl + "/" + Controller;
        }
    }
}
