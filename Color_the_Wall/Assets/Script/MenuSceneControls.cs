using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSceneControls : MonoBehaviour
{
	private GameObject MainMenu;
	private GameObject GameMenu;
	private GameObject InfoMenu;
    private Dropdown SizeDropdown;
    private Dropdown TypeDropdown;
    private Dropdown EnemyDropdown;


	void Start()
	{
		MainMenu = GameObject.Find("MainMenu");
		GameMenu = GameObject.Find("GameMenu");
		InfoMenu = GameObject.Find("InfoMenu");
        SizeDropdown = GameObject.Find("Size_Dropdown").GetComponent<Dropdown>();
        TypeDropdown = GameObject.Find("Type_Dropdown").GetComponent<Dropdown>();
        EnemyDropdown = GameObject.Find("Enemy_Dropdown").GetComponent<Dropdown>();
    	
    	MainMenu.SetActive(true);
    	GameMenu.SetActive(false);
    	InfoMenu.SetActive(false);
        
        if (SceneSwitcher.IsPlayMenuLoading)
        {
            PlayButtonPressed();
        }
    }


    public void PlayButtonPressed()
    {
        SizeDropdown.value = (int) SceneSwitcher.GameFieldSize;
        TypeDropdown.value = (int) SceneSwitcher.GameFieldType;
        EnemyDropdown.value = (int) SceneSwitcher.GameType;

    	GameMenu.SetActive(true);
    	MainMenu.SetActive(false);
    }

    public void InfoButtonPressed()
    {
    	InfoMenu.SetActive(true);
    	MainMenu.SetActive(false);
    }

    public void ExitButtonPressed()
    {
    	Application.Quit();
    }


    public void StartGameButtonPressed()
    {
        SceneSwitcher.SetGameSettings((GameProvider.GameFieldSize) SizeDropdown.value, (GameProvider.GameFieldType) TypeDropdown.value, (GameProvider.GameType) EnemyDropdown.value);
    	SceneSwitcher.LoadGameScene();
    }

    public void BackButtonPressed()
    {
    	MainMenu.SetActive(true);
    	GameMenu.SetActive(false);
    	InfoMenu.SetActive(false);
    }
}
