using System.Collections;
using TMPro;
using UnityEngine;
using Input = Utils.Input;

namespace Dialog
{
    public class DialogBoxController : MonoBehaviour
    {
        private TextMeshProUGUI _textBox;

        private void Start()
        {
            _textBox = GetComponentInChildren<TextMeshProUGUI>();
            gameObject.SetActive(false);
        }

        public IEnumerator TypeText(string text, AudioSource audioSource)
        {
            _textBox.text = "";
            foreach (var c in text)
            {
                _textBox.text += c;
                if (c != ' ')
                {
                    audioSource.Play();
                }
                yield return new WaitForSeconds(0.05f);
            }

            yield return StartCoroutine(WaitForClick());
        }

        private static IEnumerator WaitForClick()
        {
            while (!Input.AdvanceDialog())
            {
                yield return null;
            }
        }
    }
}
