using System;
using UnityEngine;

namespace InGame.UI
{
    public class CloseExcelWindow : Window<Exception>
    {
        public CloseExcelWindow()
        {
            GlobalUI.closeExcelWindow = this;
        }

        protected override void OnShow(Exception argument)
        {
            //throw new NotImplementedException();
        }
    }
}