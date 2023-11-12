using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string FirstLevelName; // Here to be able to change the name of the next loaded scene from editor.
    public GameObject OptionsScreen; // Put the options screen gameobject here. The game object should be inactive when starting a game.    


    public void StartGame()
    {
        GameManager.Current.GoTo(GameStateType.Game);
    }

    public void OpenOptions()
    {
        OptionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Pressed Quit");
        Application.Quit();
    }


}
