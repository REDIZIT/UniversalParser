using InGame.UI.Elements;
using IngameDebugConsole;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Settings
{
	public class SettingsUI : MonoBehaviour
	{
		[Header("Proxy")]
		public Switch proxySwitch;
		public InputField proxyAddress, proxyPort;

		[Header("Dev")]
		public Switch enableConsoleSwitch;

		private Settings settings => SettingsManager.settings;
		private bool isRefreshing;

        private void Awake()
        {
			proxySwitch.onValueChanged += (_) => OnAnyValueChanged();
			enableConsoleSwitch.onValueChanged += (_) => OnAnyValueChanged();

		}
        public void Show()
        {
			gameObject.SetActive(true);
			Refresh();

		}
		public void Close()
        {
			gameObject.SetActive(false);
        }

		public void Refresh()
        {
			isRefreshing = true;

			proxySwitch.SetIsOn(settings.isProxyEnabled, true);
			proxyAddress.text = settings.proxyAddress;
			proxyPort.text = settings.proxyPort == 0 ? "" : settings.proxyPort.ToString();


			enableConsoleSwitch.SetIsOn(settings.enableConsole, true);

			isRefreshing = false;
		}

		public void OnAnyValueChanged()
        {
			if (isRefreshing) return;


			settings.isProxyEnabled = proxySwitch.isOn;
			settings.proxyAddress = proxyAddress.text;
			if (int.TryParse(proxyPort.text, out int port))
            {
				settings.proxyPort = port;
			}



			settings.enableConsole = enableConsoleSwitch.isOn;
			DebugLogManager.Instance.gameObject.SetActive(SettingsManager.settings.enableConsole);

			SettingsManager.Save();
        }
	}
}