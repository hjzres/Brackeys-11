using UnityEngine;
using Data;
using TMPro;
using UnityEngine.UI;



public class SliderManager : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private TextMeshProUGUI text;


    private void Start(){
        _slider = GetComponent<Slider>();
    }

    public void FOVSlider(float FOV){
        StaticData.FOV = FOV;
        text.text = FOV.ToString();
    }

    public void SensitivitySlider(float sense){
        StaticData.Sensitivity = sense;
        text.text = sense.ToString("0");
    }

    public void VolumeSlider(float volume){
        StaticData.Volume = volume;
        text.text = (volume * 100).ToString("0") + "%";
    }

    public void MusicSlider(float music){
        StaticData.MusicVolume = music;
        text.text = (music * 100).ToString("0") + "%";
    }
}
