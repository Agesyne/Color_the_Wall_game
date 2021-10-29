using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameSceneControls : MonoBehaviour
{
    public Button GrayButtonL;
    public Button GrayButtonR;
    public Button YellowButtonL;
    public Button YellowButtonR;
    public Button RedButtonL;
    public Button RedButtonR;
    public Button BlueButtonL;
    public Button BlueButtonR;
    public Button GreenButtonL;
    public Button GreenButtonR;
    public Button MagentaButtonL;
    public Button MagentaButtonR;

    private GameProvider gameProviderComponent;
    private Transform gameFieldTransform;
    private GameObject menuObjects;
    private GameObject endGameObjects;
    private GameObject menuPanel;
    private Text winInfo;


    void Start()
    {
        GrayButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.GRAY));
        GrayButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.GRAY));
        YellowButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.YELLOW));
        YellowButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.YELLOW));
        RedButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.RED));
        RedButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.RED));
        BlueButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.BLUE));
        BlueButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.BLUE));
        GreenButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.GREEN));
        GreenButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.GREEN));
        MagentaButtonL.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.MAGENTA));
        MagentaButtonR.onClick.AddListener(() => ColorButtonPressed(GameProvider.PlayColors.MAGENTA));

        gameFieldTransform = GameObject.Find("GameField").GetComponent<Transform>();
        winInfo = GameObject.Find("WinInfo_Text").GetComponent<Text>();
        menuObjects = GameObject.Find("MenuObjects");
        endGameObjects = GameObject.Find("EndGameObject");
        menuPanel = GameObject.Find("TopButtons_Panel");


        menuObjects.SetActive(false);
        endGameObjects.SetActive(false);
    }


    public void ColorButtonPressed(GameProvider.PlayColors buttonColor)
    {
        gameProviderComponent.MakeGameStep(buttonColor);
    }

    private void ChangeButtonActive(Button button, bool isActive)
    {
        if (button.enabled != isActive)
        {
            Image buttonImage = button.gameObject.GetComponent<Image>();
            Color buttonColor = buttonImage.color;

            if (isActive)
            {
                buttonColor.a = 1f;
            }
            else
            {
                buttonColor.a = 0.25f;
            }

            buttonImage.color = buttonColor;
            button.enabled = isActive;
        }
    }

    public void ChangeButtonsActiveByColor(GameProvider.PlayColors currentColor, bool isActive)
    {
        switch(currentColor)
        {
            case GameProvider.PlayColors.GRAY:
                ChangeButtonActive(GrayButtonL, isActive);
                ChangeButtonActive(GrayButtonR, isActive);
                break;

            case GameProvider.PlayColors.YELLOW:
                ChangeButtonActive(YellowButtonL, isActive);
                ChangeButtonActive(YellowButtonR, isActive);
                break;

            case GameProvider.PlayColors.RED:
                ChangeButtonActive(RedButtonL, isActive);
                ChangeButtonActive(RedButtonR, isActive);
                break;

            case GameProvider.PlayColors.BLUE:
                ChangeButtonActive(BlueButtonL, isActive);
                ChangeButtonActive(BlueButtonR, isActive);
                break;

            case GameProvider.PlayColors.GREEN:
                ChangeButtonActive(GreenButtonL, isActive);
                ChangeButtonActive(GreenButtonR, isActive);
                break;

            default:
                ChangeButtonActive(MagentaButtonL, isActive);
                ChangeButtonActive(MagentaButtonR, isActive);
                break;
        };
    }

    public void SetGameProvider(GameProvider newGameProvider)
    {
        gameProviderComponent = newGameProvider;
    }


    private void ChangeAllButtonActive(bool isActive)
    {
        foreach (GameProvider.PlayColors currentColor in GameProvider.PlayColors.GetValues(typeof(GameProvider.PlayColors)))
        {
            ChangeButtonsActiveByColor(currentColor, isActive);
        }
    }

    public void MenuButtonPressed()
    {
        menuPanel.SetActive(false);
        menuObjects.SetActive(true);

        var shiftVector = new Vector3(0, 0, 10f);
        gameFieldTransform.position -= shiftVector;
        gameProviderComponent.ShiftUnactivePlayColors(true);
    }

    public void ContinueButtonPressed()
    {
        menuObjects.SetActive(false);
        endGameObjects.SetActive(false);
        menuPanel.SetActive(true);

        var shiftVector = new Vector3(0, 0, 10f);
        gameFieldTransform.position += shiftVector;
        gameProviderComponent.ShiftUnactivePlayColors();
    }

    public void NewGameButtonPressed()
    {
        SceneSwitcher.LoadMainScene(true);
    }

    public void GiveUpButtonPressed()
    {
        menuObjects.SetActive(false);

        var shiftVector = new Vector3(0, 0, 10f);
        gameFieldTransform.position += shiftVector;
        gameProviderComponent.EndGame();
    }

    public void GoToMainMenuButtonPressed()
    {
        SceneSwitcher.LoadMainScene(false);
    }

    public void OKButtonPressed()
    {
        SceneSwitcher.LoadMainScene(true);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void EndGameHappened(bool isFirstPlayerWon)
    {
        endGameObjects.SetActive(true);
        menuPanel.SetActive(false);

        var shiftVector = new Vector3(0, 0, 10f);
        gameFieldTransform.position -= shiftVector;
        ChangeAllButtonActive(false);        
        winInfo.text += (isFirstPlayerWon) ? " слева!" : " справа!";
    }
}
