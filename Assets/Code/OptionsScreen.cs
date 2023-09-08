using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle FullScreenToggle;
    public Toggle VSyncToggle;

    public TMP_Text resolutionLabel;

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

        float Volume = 0;
        AudioMixer.GetFloat("MasterVol", out Volume);
        masterSlider.value = Volume;
        AudioMixer.GetFloat("MusicVol", out Volume);
        musicSlider.value = Volume;
        AudioMixer.GetFloat("SFXVol", out Volume);
        sfxSlider.value = Volume;

        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
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
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        AudioMixer.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }
    public void SetMusicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        AudioMixer.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }
    public void SetSFXVolume()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        AudioMixer.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;

    public int vertical;
}
