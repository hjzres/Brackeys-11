using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Puzzles
{   
    public class DirectionShiftRegister : MonoBehaviour
    {
        public GameObject[] panels;

        [SerializeField] private List<bool> _register;

        

        private void Start() {
            UpdatePanel();
        }

        public void Shift(bool val) {
            _register.Add(val);
            if(_register.Count > panels.Length) {
                _register.RemoveAt(0);
            }
            UpdatePanel();
        }

        private void UpdatePanel() {
            for(int i = 0; i<panels.Length; i++) {
                if(i >= _register.Count) {
                    panels[i].SetActive(false);
                    continue;
                }

                panels[i].SetActive(true);
                panels[i].GetComponentInChildren<TextMeshProUGUI>().text = _register[i] ? ">" : "<";

            }
        }
    }
}
