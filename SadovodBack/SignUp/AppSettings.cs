using System;

namespace SadovodBack
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public AppSettings()
        {
            Secret = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        }
    }
}