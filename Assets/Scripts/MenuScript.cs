using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Header("Graphics")]
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle FullscreenToggle;

    [Header("Audio")] 
    public Slider Volume;
    
    private void Start()
    {
        UpdateGraphicsOptionsUI();
        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        var audio = PlayerPrefs.GetFloat(SpaceTruckerConstants.VolumeKey, 1);
        audio = Math.Clamp(audio, 0, 1);
        Volume.value = audio;
        Volume.maxValue = 1;
        Volume.minValue = 0;
    }

    public void ApplyAudio()
    {
        var vol = Volume.value;
        PlayerPrefs.SetFloat(SpaceTruckerConstants.VolumeKey, vol);
        UpdateAudioUI();
    }

    public void Play()
    {
        SceneManager.LoadScene("Space");
    }

    public void Quit()
    {
        Application.Quit();
    }
    

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    
    private void UpdateGraphicsOptionsUI()
    {
        // Resolution
        ResolutionDropdown.ClearOptions();
        List<string> reses = new List<string>();
        int i = 0;
        int currentIndex = 0;
        foreach (ResolutionManager.Resolution res in ResolutionManager.getResolutions())
        {
            reses.Add(res.ToString());
            if (res.isCurrentResolution())
            {
                currentIndex = i;
            }
            i++;
        }
        ResolutionDropdown.AddOptions(reses);
        ResolutionDropdown.value = currentIndex;

        // Fullscreen
        FullscreenToggle.isOn = Screen.fullScreen;

        // Quality
        QualityDropdown.ClearOptions();
        List<string> qs = new List<string>(QualitySettings.names);
        QualityDropdown.AddOptions(qs);
        QualityDropdown.value = QualitySettings.GetQualityLevel(); ;
    }
    
    public void SetResolution(int resolutionIndex, bool fullScreen = true)
    {
        ResolutionManager.Resolution ChosenRes = ResolutionManager.getResolutions()[resolutionIndex];
        Screen.SetResolution(ChosenRes.Width, ChosenRes.Height, FullscreenToggle.isOn);
        QualitySettings.SetQualityLevel(QualityDropdown.value);
    }

    public void ApplyGraphicsChanges()
    {
        SetResolution(ResolutionDropdown.value, FullscreenToggle.isOn);
    }
}
