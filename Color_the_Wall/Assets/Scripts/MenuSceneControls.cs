using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneControls : MonoBehaviour
{
	private GameObject mainMenu;
	private GameObject gameMenu;
	private GameObject settingsMenu;
	private GameObject infoMenu;

	void Start()
	{
		mainMenu = GameObject.Find("MainMenu");
		gameMenu = GameObject.Find("GameMenu");
		settingsMenu = GameObject.Find("SettingsMenu");
		infoMenu = GameObject.Find("InfoMenu");
    	
    	mainMenu.SetActive(true);
    	gameMenu.SetActive(false);
    	settingsMenu.SetActive(false);
    	infoMenu.SetActive(false);
    }


    public void PlayButtonPressed()
    {
    	gameMenu.SetActive(true);
    	mainMenu.SetActive(false);
    }

    public void InfoButtonPressed()
    {
    	infoMenu.SetActive(true);
    	mainMenu.SetActive(false);
    }

    public void SettingsButtonPressed()
    {
    	settingsMenu.SetActive(true);
    	mainMenu.SetActive(false);
    }

    public void ExitButtonPressed()
    {
    	Application.Quit();
    	//Debug.Log("Exit game");
    }



    public void StartGameButtonPressed()
    {
    	//взять все нужные параметры из дропбоксов
    	SceneManager.LoadScene("GameScene");
    }

    public void BackButtonPressed()
    {
    	mainMenu.SetActive(true);
    	gameMenu.SetActive(false);
    	settingsMenu.SetActive(false);
    	infoMenu.SetActive(false);
    }
}
