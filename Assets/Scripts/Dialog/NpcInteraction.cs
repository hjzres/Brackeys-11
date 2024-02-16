using System.Collections;
using Interaction;
using UnityEngine;
using Utils;

namespace Dialog
{
    public class NpcInteraction : MonoBehaviour, IInteractable
    {
        public string[] dialog;
        
        private DialogBoxController _dialogBox;
        private AudioSource _audioSource;

        private void Awake()
        {
            _dialogBox = GameObject.Find("Dialog Box").GetComponent<DialogBoxController>();
            _audioSource = GetComponent<AudioSource>();
            Debug.Log(_dialogBox);
        }

        public void Interact()
        {
            StartCoroutine(InitiateDialog());
        }

        private IEnumerator InitiateDialog()
        {
            _dialogBox.gameObject.SetActive(true);
            Utils.Input.Context = InputContext.Dialog;

            yield return StartCoroutine(CycleText());
            
            _dialogBox.gameObject.SetActive(false);
            Utils.Input.Context = InputContext.Gameplay;
        }

        private IEnumerator CycleText()
        {
            foreach (var text in dialog)
            {
                yield return StartCoroutine(_dialogBox.TypeText(text, _audioSource));
            }
        }
    }
}
