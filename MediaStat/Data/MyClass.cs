using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class MyClass
    {
        public string ConnectionString;
        public IConfiguration configuration;

        public MyClass(IConfiguration configuration)
        {
            //ConnectionString = new configuration.GetValue<string>("ConnectionString");
           ConnectionString =  configuration.GetConnectionString("DevConnection");
        }
    }
}
