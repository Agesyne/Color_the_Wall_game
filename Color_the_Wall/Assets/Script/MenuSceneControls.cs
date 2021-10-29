using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSceneControls : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject gameMenu;
    private GameObject infoMenu;
    private Dropdown sizeDropdown;
    private Dropdown typeDropdown;
    private Dropdown enemyDropdown;


    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        gameMenu = GameObject.Find("GameMenu");
        infoMenu = GameObject.Find("InfoMenu");
        sizeDropdown = GameObject.Find("Size_Dropdown").GetComponent<Dropdown>();
        typeDropdown = GameObject.Find("Type_Dropdown").GetComponent<Dropdown>();
        enemyDropdown = GameObject.Find("Enemy_Dropdown").GetComponent<Dropdown>();

        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        infoMenu.SetActive(false);
        
        if (SceneSwitcher.IsPlayMenuLoading)
        {
            PlayButtonPressed();
        }
    }


    public void PlayButtonPressed()
    {
        sizeDropdown.value = (int) SceneSwitcher.GameFieldSize;
        typeDropdown.value = (int) SceneSwitcher.GameFieldType;
        enemyDropdown.value = (int) SceneSwitcher.GameType;

        gameMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void InfoButtonPressed()
    {
        infoMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }


    public void StartGameButtonPressed()
    {
        SceneSwitcher.SetGameSettings((GameProvider.GameFieldSize) sizeDropdown.value, (GameProvider.GameFieldType) typeDropdown.value, (GameProvider.GameType) enemyDropdown.value);
        SceneSwitcher.LoadGameScene();
    }

    public void BackButtonPressed()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        infoMenu.SetActive(false);
    }
}
