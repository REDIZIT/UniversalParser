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

		[Header("M2")]
		public InputField m2Login;
		public InputField m2Password;

		[Header("Dev")]
		public Switch enableConsoleSwitch;

		private Settings settings => SettingsManager.settings;
		private bool isRefreshing;

        private void Awake()
        {
			proxySwitch.onValueChanged += (_) => OnAnyValueChanged();
			enableConsoleSwitch.onValueChanged += (_) => OnAnyValueChanged();

			m2Login.onValueChanged.AddListener((_) => OnAnyValueChanged());
			m2Password.onValueChanged.AddListener((_) => OnAnyValueChanged());
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


			m2Login.text = settings.m2Login;
			m2Password.text = settings.m2Password;

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


			settings.m2Login = m2Login.text;
			settings.m2Password = m2Password.text;


			settings.enableConsole = enableConsoleSwitch.isOn;
			DebugLogManager.Instance.gameObject.SetActive(SettingsManager.settings.enableConsole);

			SettingsManager.Save();
        }
	}
}