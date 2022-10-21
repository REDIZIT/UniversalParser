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

        string GetPagedUrl(string url, int page);

        public class Model : ElementModel
        {
            public string pageArgument = "p";
        }
    }
    public class FakePaging : IPaging
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

        public GameObject gameObject => null;
        public bool IsValid => true;

        private int _end;
        private IPaging.Model model;

        public string GetPagedUrl(string url, int page)
        {
            string website = url.Split('?')[0];
            List<string> arguments = url.Split('?')[1].Split('&').ToList();

            bool hasPageArg = false;
            for (int i = 0; i < arguments.Count; i++)
            {
                string argName = arguments[i].Split('=')[0];
                if (argName == model.pageArgument)
                {
                    arguments[i] = argName + "=" + page;
                    hasPageArg = true;
                    break;
                }
            }

            if (hasPageArg == false)
            {
                arguments.Add(model.pageArgument + "=" + page);
            }

            return website + "?" + string.Join("&", arguments);
        }

        public void Setup(IPaging.Model model)
        {
            this.model = model;
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
            string website = url.Split('?')[0];
            List<string> arguments = url.Split('?')[1].Split('&').ToList();

            bool hasPageArg = false;
            for (int i = 0; i < arguments.Count; i++)
            {
                string argName = arguments[i].Split('=')[0];
                if (argName == model.pageArgument)
                {
                    arguments[i] = argName + "=" + page;
                    hasPageArg = true;
                    break;
                }
            }

            if (hasPageArg == false)
            {
                arguments.Add(model.pageArgument + "=" + page);
            }
            
            return website + "?" + string.Join("&", arguments);
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