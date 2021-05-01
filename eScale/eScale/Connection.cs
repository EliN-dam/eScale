using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace eScale
{
    public class Connection
    {
        public static bool CheckConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
