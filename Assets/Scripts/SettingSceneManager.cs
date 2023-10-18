using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingSceneManager : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
