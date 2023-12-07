using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Pasta;
using System;

public class OptionsScreen : MonoBehaviour
{
    public Toggle FullScreenToggle;
    public Toggle VSyncToggle;

    public Text resolutionLabel;

    public List<ResItem> Resolutions = new List<ResItem>();
    private int SelectedRes;

    public AudioMixer AudioMixer;
    public TMP_Text masterLabel;
    public TMP_Text musicLabel;
    public TMP_Text sfxLabel;

    public Slider masterSlider, musicSlider, sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        FullScreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            VSyncToggle.isOn = false;
        }
        else
        {
            VSyncToggle.isOn = true;
        }

        bool FoundRes = false;
        for (int i = 0; i < Resolutions.Count; i++)
        {
            if (Screen.width == Resolutions[i].horizontal && Screen.height == Resolutions[i].vertical)
            {
                FoundRes = true;

                SelectedRes = i;

                UpdateResText();
            }
        }

        // Below code is if player has diffrent resolution than in resolutions list it will add that resolution to the list.
        if (FoundRes == false)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            Resolutions.Add(newRes);
            SelectedRes = Resolutions.Count - 1;

            UpdateResText();
        }

        float masterVol = AudioManager.GetVolume(VolumeParameter.MasterVolume).FromDBToLinear();
        float musicVol = AudioManager.GetVolume(VolumeParameter.MusicVolume).FromDBToLinear();
        float sfxVol = AudioManager.GetVolume(VolumeParameter.SFXVolume).FromDBToLinear();
        masterSlider.SetValueWithoutNotify(masterVol);
        musicSlider.SetValueWithoutNotify(musicVol / masterVol/ masterVol);
        sfxSlider.SetValueWithoutNotify(sfxVol / masterVol / masterVol);

        masterLabel.text = Mathf.RoundToInt(masterSlider.value * 100).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value * 100).ToString();
    }

    public void ResLeft()
    {
        SelectedRes--;
        if (SelectedRes < 0)
        {
            SelectedRes = 0;
        }
        UpdateResText();
    }

    public void ResRight()
    {
        SelectedRes++;
        if (SelectedRes > Resolutions.Count - 1)
        {
            SelectedRes = Resolutions.Count - 1;
        }
        UpdateResText();
    }

    public void UpdateResText()
    {
        resolutionLabel.text = Resolutions[SelectedRes].horizontal.ToString() + " x " + Resolutions[SelectedRes].vertical.ToString();
    }

    public void ApplyGraphics()
    {

        if (VSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;

        }

        Screen.SetResolution(Resolutions[SelectedRes].horizontal, Resolutions[SelectedRes].vertical, FullScreenToggle.isOn);
    }


    public void SetMasterVolume()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value * 100).ToString();
        SetVolume(masterSlider.value, VolumeParameter.MasterVolume);
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
        SetVolume(masterSlider.value * musicSlider.value, VolumeParameter.MusicVolume);
    }

    public void SetSFXVolume()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value * 100).ToString();
        SetVolume(masterSlider.value * sfxSlider.value, VolumeParameter.SFXVolume);
    }

    private void SetVolume(float value, VolumeParameter param)
    {
        string parameter = param.ToString();
        value = (float)Math.Round(value, 2);
        AudioManager.SetVolume(param, value);
        PlayerPrefs.SetFloat(parameter, value);
        Debug.Log(parameter + value);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;

    public int vertical;
}
