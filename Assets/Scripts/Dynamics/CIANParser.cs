using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class CIANParser : DynamicParser
    {
        private StatusElement status;
        private InputFieldElement folderSelect, url;
        private IBrowser browser;

        [Inject]
        private void Construct(StatusElement status, InputFieldElement folderSelect, InputFieldElement url, IBrowser browser)
        {
            this.folderSelect = folderSelect;
            this.status = status;
            this.url = url;
            this.browser = browser;

            status.Setup(new(this)
            {
                onSwitchWorkStatus = SwitchWorkState
            });
            folderSelect.Setup(new()
            {
                labelText = "���� �� �����, ���� ����� ��������� �������",
                placeholderText = "���� �� �����",
                validityCheckFunc = s => Directory.Exists(s)
            });
            url.Setup(new()
            {
                labelText = "������ � ���������",
                placeholderText = "������",
                validityCheckFunc = s => string.IsNullOrWhiteSpace(url.Text) == false
            });
        }

        protected override void OnStart()
        {
            status.Status = "�������� ��������� ��������";
            status.Progress = "-/-";

            // https://spb.cian.ru/cat.php
            // https://spb.cian.ru/export/xls/offers/

            string downloadUrl = "https://spb.cian.ru/export/xls/offers/?" + url.Text.Split('?')[1];

            WebClient c = new WebClient();

            string html = c.DownloadString(url.Text);

            int startIndex = html.IndexOf("�������") + "�������".Length;
            string lotsFoundStr = html.Substring(startIndex, html.IndexOf("�����", startIndex) - startIndex).Replace(" ", "");

            int lotsFound = int.Parse(lotsFoundStr);
            float lotsPerPage = 28;
            float lotsPerFile = 200;
            float pagesToHandle = lotsFound / lotsPerPage;
            int additionalPagesPerDownload = (int)(lotsPerFile / lotsPerPage);
            Debug.Log(additionalPagesPerDownload);
            int page = 1;

            status.Status = "��������";
            while(IsWorking && page <= pagesToHandle)
            {
                string currentFilePath = folderSelect.Text + "/" + page  + ".xlsx";
                c.DownloadFile(downloadUrl + "&p=" + page, currentFilePath);

                status.Status = "����� ����� ��������";
                status.Progress = page + "/" + (int)pagesToHandle;

                page += additionalPagesPerDownload;

                Thread.Sleep(3000);
            }
        }
        protected override void OnStop()
        {
            browser.Close();
        }
    }
}