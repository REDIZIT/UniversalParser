using InGame.Dynamics.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class PagingElement : DynamicElement<PagingElement.Model>
    {
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

            if (int.TryParse(start.text, out int startPage) && int.TryParse(end.text, out int endPage))
            {
                IsValid = endPage >= startPage;
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