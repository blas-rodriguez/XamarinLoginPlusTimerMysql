using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Login.Model
{
    internal class DeviceInfo
    {
        public string Username { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceOS { get; set; }
        public string DateSubmitted { get; set; }
    }
}