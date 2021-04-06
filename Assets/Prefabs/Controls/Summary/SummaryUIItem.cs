using InGame.Parse;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class SummaryUIItem : MonoBehaviour
    {
		[SerializeField] private Text pageText, handledCountText, errorsCountText;

		public void Refresh(ParseResult result, int pageIndex)
        {
			pageText.text = (pageIndex + 1).ToString();

			handledCountText.text = "Обработано: " + result.lots.Count(l => l.exception == null);
			errorsCountText.text = "Не удалось: " + result.lots.Count(l => l.exception != null);
        }
    }
}