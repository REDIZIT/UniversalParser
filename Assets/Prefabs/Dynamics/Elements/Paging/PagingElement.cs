using InGame.Dynamics.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public interface IPaging : IElement<IPaging.Model>
    {
        int RequestPagesCount { get; set; }
        int Count { get; }
        int Start { get; }
        int End { get; }

        string GetPagedUrl(string url, int page);

        public abstract class Model : ElementModel
        {
            public abstract string GetPagedUrl(string originalUrl, int page);
        }
        public class ArgumentModel : Model
        {
            public string pageArgument = "p";

            public override string GetPagedUrl(string originalUrl, int page)
            {
                string website = originalUrl.Split('?')[0];
                List<string> arguments = originalUrl.Split('?')[1].Split('&').ToList();

                bool hasPageArg = false;
                for (int i = 0; i < arguments.Count; i++)
                {
                    string argName = arguments[i].Split('=')[0];
                    if (argName == pageArgument)
                    {
                        arguments[i] = argName + "=" + page;
                        hasPageArg = true;
                        break;
                    }
                }

                if (hasPageArg == false)
                {
                    arguments.Add(pageArgument + "=" + page);
                }

                return website + "?" + string.Join("&", arguments);
            }
        }
        public class PageModel : Model
        {
            public string pagePattern = "page-{0}";

            public override string GetPagedUrl(string originalUrl, int page)
            {
                string[] splitted = originalUrl.Split('?');
                string url = splitted[0] + "/" + string.Format(pagePattern, page);
                if (splitted.Length > 1) url+= "?" + splitted[1];
                return url;
            }
        }
    }
    public class PagingElement : DynamicElement<IPaging.Model>, IPaging
    {
        public int Start { get; private set; }
        public int End
        {
            get
            {
                if (_end == -1) return RequestPagesCount;
                else return Mathf.Clamp(_end, 1, RequestPagesCount);
            }
        }

        /// <summary>Shows how many pages selected in range. Will be equal to <see cref="int.MaxValue"/> if start or end is not defined.</summary>
        public int Count => End - Start + 1;

        public int RequestPagesCount { get; set; }


        [SerializeField] private InputField current, start, end;
        [SerializeField] private ThemedImage rangeImage;

        private int _end;

        protected override void OnSetup()
        {
            start.onValueChanged.AddListener((_) => UpdateFields());
            end.onValueChanged.AddListener((_) => UpdateFields());

            UpdateFields();
        }

        public string GetPagedUrl(string url, int page)
        {
            return model.GetPagedUrl(url, page);
        }

        private void UpdateFields()
        {
            ClampField(start);
            ClampField(end);

            IsValid = true;

            if (int.TryParse(start.text, out int startPage)) Start = startPage;
            else Start = 1;

            if (int.TryParse(end.text, out int endPage)) _end = endPage;
            else _end = -1;

            if (_end != -1)
            {
                IsValid = _end >= Start;
            }

            rangeImage.SetColor(IsValid ? ColorLayer.Button : ColorLayer.Error);
        }
        private void ClampField(InputField field)
        {
            if (string.IsNullOrWhiteSpace(field.text) == false)
            {
                if (int.TryParse(field.text, out int page) && page < 1)
                {
                    field.text = "1";
                }
            }
        }
    }
}