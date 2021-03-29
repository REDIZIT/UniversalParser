using System;
using UnityEngine;

namespace InGame.UI
{
	public class NotSafeExitWindow : Window<NotSafeExitWindow.Arguments>
	{
        public class Arguments
        {
			public Action onExitWithoutSave;
			public Action onSaveAndExit;
			public Action onCancelExit;
        }

		private Arguments args;

		protected override void OnShow(Arguments argument)
		{
			args = argument;
		}

		public void ClickExitWithoutSave()
        {
			args.onExitWithoutSave?.Invoke();
			Close();
        }

		public void ClickSaveAndExit()
        {
			args.onSaveAndExit?.Invoke();
			Close();
		}

		public void ClickCancelExit()
        {
			args.onCancelExit?.Invoke();
			Close();
		}
	}
}