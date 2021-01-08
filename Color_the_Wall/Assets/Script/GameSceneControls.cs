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

    private GameProvider GameProviderComponent;
    private Transform GameFieldTransform;
    private GameObject MenuObjects;
    private GameObject EndGameObjects;
    private GameObject MenuPanel;
    private Text WinInfo;


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

        GameFieldTransform = GameObject.Find("GameField").GetComponent<Transform>();
        WinInfo = GameObject.Find("WinInfo_Text").GetComponent<Text>();
		MenuObjects = GameObject.Find("MenuObjects");
		EndGameObjects = GameObject.Find("EndGameObject");
		MenuPanel = GameObject.Find("TopButtons_Panel");
    	

    	MenuObjects.SetActive(false);
    	EndGameObjects.SetActive(false);
    }


    public void ColorButtonPressed(GameProvider.PlayColors buttonColor)
    {
        GameProviderComponent.MakeGameStep(buttonColor);
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
        		//buttonColor.r *= 1.25f/*(buttonColor.r < 0.5f) ? 0.5f : 0f*/;
        		//buttonColor.g *= 1.25f/*(buttonColor.g < 0.5f) ? 0.5f : 0f*/;
        		//buttonColor.b *= 1.25f/*(buttonColor.b < 0.5f) ? 0.5f : 0f*/;
        	}
        	else
        	{
        		buttonColor.a = 0.25f;
        		//buttonColor.r *= 0.8f/*(buttonColor.r > 0.5f) ? 0.5f : 0f*/;
        		//buttonColor.g *= 0.8f/*(buttonColor.g > 0.5f) ? 0.5f : 0f*/;
        		//buttonColor.b *= 0.8f/*(buttonColor.b > 0.5f) ? 0.5f : 0f*/;
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
    	GameProviderComponent = newGameProvider;
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
    	MenuPanel.SetActive(false);
    	MenuObjects.SetActive(true);

        var shiftVector = new Vector3(0, 0, 10f);
        GameFieldTransform.position -= shiftVector;
        GameProviderComponent.ShiftUnactivePlayColors(true);
    }

    public void ContinueButtonPressed()
    {
    	MenuObjects.SetActive(false);
    	EndGameObjects.SetActive(false);
    	MenuPanel.SetActive(true);

        var shiftVector = new Vector3(0, 0, 10f);
        GameFieldTransform.position += shiftVector;
        GameProviderComponent.ShiftUnactivePlayColors();
    }

    public void NewGameButtonPressed()
    {
    	SceneSwitcher.LoadMainScene(true);
    }

    public void GiveUpButtonPressed()
    {
    	MenuObjects.SetActive(false);

        var shiftVector = new Vector3(0, 0, 10f);
        GameFieldTransform.position += shiftVector;
    	GameProviderComponent.EndGame();
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
    	EndGameObjects.SetActive(true);
    	MenuPanel.SetActive(false);

        var shiftVector = new Vector3(0, 0, 10f);
        GameFieldTransform.position -= shiftVector;
        ChangeAllButtonActive(false);        
    	WinInfo.text += (isFirstPlayerWon) ? " слева!" : " справа!";
    }
}
