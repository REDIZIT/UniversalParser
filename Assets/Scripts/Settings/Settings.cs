using Newtonsoft.Json;

namespace InGame.Settings
{
	public class Settings
	{
        public bool isProxyEnabled = true;
        public string proxyAddress = "http://proxy.ko.wan";
        public int proxyPort = 808;

        public bool enableConsole;
    }
}