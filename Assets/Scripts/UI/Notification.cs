using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Notification : MonoBehaviour
    {
        private static Notification _instance;
        
        private TextMeshProUGUI _text;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        private void PopUp(string msg)
        {
            _text.text = msg;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            _canvasGroup.DOFade(1, 0.5f)
                .SetEase(Ease.InOutCubic);
            _canvasGroup.DOFade(0, 0.5f)
                .SetEase(Ease.InOutCubic)
                .SetDelay(3.5f);
        }

        public static void ShowMessage(string msg)
        {
            _instance.PopUp(msg);
        }
    }
}