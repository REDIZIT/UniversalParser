using InGame.Dynamics.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class PagingElement : DynamicElement<PagingElement.Model>
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        /// <summary>Shows how many pages selected in range. Will be equal to <see cref="int.MaxValue"/> if start or end is not defined.</summary>
        public int Count { get; private set; }


        [SerializeField] private InputField current, start, end;
        [SerializeField] private ThemedImage rangeImage;

        public class Model
        {

        }

        protected override void OnSetup()
        {
            start.onValueChanged.AddListener((_) => UpdateFields());
            end.onValueChanged.AddListener((_) => UpdateFields());
        }

        private void UpdateFields()
        {
            ClampField(start);
            ClampField(end);

            IsValid = true;

            if (int.TryParse(start.text, out int startPage)) Start = startPage;
            else Start = -1;

            if (int.TryParse(end.text, out int endPage)) End = endPage;
            else End = endPage;

            if (Start != -1 && End != -1)
            {
                IsValid = endPage >= startPage;
                Count = End - Start + 1;
            }
            else
            {
                Count = int.MaxValue;
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