using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;



public class SliderManager : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI _text;


    private void Start(){
        slider = GetComponent<Slider>();
    }

    public void FOVSlider(float FOV){
        StaticData.FOV = FOV;
        _text.text = FOV.ToString();
    }

    public void SensitivitySlider(float sense){
        StaticData.sensitivity = sense;
        _text.text = sense.ToString("0");
    }

    public void VolumeSlider(float volume){
        StaticData.volume = volume;
        _text.text = (volume * 100).ToString("0") + "%";
    }

    public void MusicSlider(float music){
        StaticData.musicVolume = music;
        _text.text = (music * 100).ToString("0") + "%";
    }
}
