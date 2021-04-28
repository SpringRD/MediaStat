using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public static class MyAppData
    {
        public static IConfiguration Configuration;

        public static string MyConnectionString;

        public static string MyConnectionStringSSIS;

        public static string MyConnectionStringMaster;
    }

}
