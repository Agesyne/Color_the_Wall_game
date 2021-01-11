using System;
using UnityEngine;
using UnityEngine.UI;


public class GameProvider : MonoBehaviour
{
    public GameGrid GameField {get; private set;}
    
    public enum GameFieldSize
    {
        LITTLE = 0,
        MIDDLE,
        BIG
    }
    public enum GameFieldType
    {
        USUAL = 0,
        SIMMETRIC
    }
    public enum GameType
    {
        VS_COMPUTER = 0,
        VS_PLAYER
    }
    public enum PlayColors
    {
        GRAY = 0,
        YELLOW,
        RED,
        BLUE,
        GREEN,
        MAGENTA
    }

    private CameraControlsComponent CameraControlsComponent;
    private GameSceneControls GameSceneControls;
    private Text FirstPlayerInfoScreen;
    private Text SecondPlayerInfoScreen;
    private bool IsFirstPlayerTurn = true;
    private bool IsComputerGamer;
    private ComputerGamer SecondGamer = new ComputerGamer();

    private (int NeutralCells, int FirstPlayerCells, int SecondPlayerCells) FieldStatistic;
    private (PlayColors FirstPlayerColor, PlayColors SecondPlayerColor) CurrentChosenColors;
    private enum GameSizeConstants
    {
    	LITTLE_X = 23 /*33*/,
    	LITTLE_Y = 15 /*21*/,
    	MIDDLE_X = 33 /*65*/,
    	MIDDLE_Y = 21 /*41*/,
    	BIG_X = 49 /*97*/,
    	BIG_Y = 31 /*61*/
    }


    void Start()
    {
        CameraControlsComponent = GameObject.Find("Main Camera").GetComponent<CameraControlsComponent>();
        GameField = GameObject.Find("GameField").GetComponent<GameGrid>();
        FirstPlayerInfoScreen = GameObject.Find("FirstPlayerInfo_Text").GetComponent<Text>();
        SecondPlayerInfoScreen = GameObject.Find("SecondPlayerInfo_Text").GetComponent<Text>();
        GameSceneControls = GameObject.Find("Game_Canvas").GetComponent<GameSceneControls>();
        
        GameSceneControls.SetGameProvider(this);
        GameProviderStart(SceneSwitcher.GameFieldSize, SceneSwitcher.GameFieldType, SceneSwitcher.GameType);
    }


    private int GetGameFieldSize(bool isX, GameFieldSize sizeName)
    {
    	if (isX)
    	{
    		switch(sizeName)
    		{
    			case GameFieldSize.LITTLE:
    				return (int)GameSizeConstants.LITTLE_X;
    			case GameFieldSize.MIDDLE:
    				return (int)GameSizeConstants.MIDDLE_X;
    			default:
    				return (int)GameSizeConstants.BIG_X;
    		}
    	}
    	else
    	{
    		switch(sizeName)
    		{
    			case GameFieldSize.LITTLE:
    				return (int)GameSizeConstants.LITTLE_Y;
    			case GameFieldSize.MIDDLE:
    				return (int)GameSizeConstants.MIDDLE_Y;
    			default:
    				return (int)GameSizeConstants.BIG_Y;
    		}
    	}
    }

    public void GameProviderStart(GameFieldSize gameFieldSize, GameFieldType gameFieldType, GameType gameType)
    {
        GameField.GameGridStart(GetGameFieldSize(true, gameFieldSize), GetGameFieldSize(false, gameFieldSize), gameFieldType == GameFieldType.SIMMETRIC);
        CameraControlsComponent.SetCameraProperties();
        StartGame(gameType);
    }



    private bool IsGameOver()
    {
        return (Math.Abs(FieldStatistic.FirstPlayerCells - FieldStatistic.SecondPlayerCells) > FieldStatistic.NeutralCells);
    }

    private void StartGame(GameType gameType)
    {
        if (gameType == GameType.VS_COMPUTER)
        {
            IsComputerGamer = true;
        }
        MakeFirstGameStep();
    }

    private void UpdateScreenStats()
    {
        FieldStatistic = GameField.GetStatistics();
        FirstPlayerInfoScreen.text = FieldStatistic.FirstPlayerCells.ToString();
        SecondPlayerInfoScreen.text = FieldStatistic.SecondPlayerCells.ToString();
    }

    public void ShiftUnactivePlayColors(bool isDeactivating = false)
    {
        foreach (PlayColors currentColor in PlayColors.GetValues(typeof(PlayColors)))
        {
            if (isDeactivating || currentColor == CurrentChosenColors.FirstPlayerColor || currentColor == CurrentChosenColors.SecondPlayerColor)
            {
                GameSceneControls.ChangeButtonsActiveByColor(currentColor, false);
            }
            else
            {
                GameSceneControls.ChangeButtonsActiveByColor(currentColor, true);
            }
        }
    }

    private void MakeFirstGameStep()
    {
        CurrentChosenColors = (PlayColors.GRAY, PlayColors.MAGENTA);
        CurrentChosenColors.FirstPlayerColor = GameField.MakeFirstStep(IsFirstPlayerTurn);
        CurrentChosenColors.SecondPlayerColor = GameField.MakeFirstStep(!IsFirstPlayerTurn);

        GameField.DrawGrid();
        UpdateScreenStats();
        ShiftUnactivePlayColors();
    }

    public void EndGame()
    {
        bool isFirstPlayerWon = (FieldStatistic.FirstPlayerCells > FieldStatistic.SecondPlayerCells) ? true : false;
        GameSceneControls.EndGameHappened(isFirstPlayerWon);
    }

    public void MakeGameStep(PlayColors chosenColor)
    {
        if (IsGameOver())
        {
            return;
        }

        GameField.BFSByColor(chosenColor, IsFirstPlayerTurn);
        GameField.DrawGrid();
        UpdateScreenStats();

        if (IsFirstPlayerTurn)
        {
            CurrentChosenColors.FirstPlayerColor = chosenColor;
        } 
        else
        {
            CurrentChosenColors.SecondPlayerColor = chosenColor;
        }

        ShiftUnactivePlayColors();
        IsFirstPlayerTurn = !IsFirstPlayerTurn;

        if (IsGameOver())
        {
            EndGame();
            return;
        }

        if (IsComputerGamer && !IsFirstPlayerTurn)
        {
            chosenColor = SecondGamer.MyTurn(GameField, CurrentChosenColors);
            MakeGameStep(chosenColor);
        }
    }
}
