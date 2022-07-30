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

    private CameraControls cameraControlsComponent;
    private GameSceneControls gameSceneControls;
    private Text firstPlayerInfoScreen;
    private Text secondPlayerInfoScreen;
    private bool isFirstPlayerTurn = true;
    private bool isComputerGamer;
    private ComputerGamer secondGamer = new ComputerGamer();

    private (int NeutralCells, int FirstPlayerCells, int SecondPlayerCells) fieldStatistic;
    private (PlayColors FirstPlayerColor, PlayColors SecondPlayerColor) currentChosenColors;
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
        cameraControlsComponent = GameObject.Find("Main Camera").GetComponent<CameraControls>();
        GameField = GameObject.Find("GameField").GetComponent<GameGrid>();
        firstPlayerInfoScreen = GameObject.Find("FirstPlayerInfo_Text").GetComponent<Text>();
        secondPlayerInfoScreen = GameObject.Find("SecondPlayerInfo_Text").GetComponent<Text>();
        gameSceneControls = GameObject.Find("Game_Canvas").GetComponent<GameSceneControls>();
        
        gameSceneControls.SetGameProvider(this);
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
        cameraControlsComponent.SetCameraProperties();
        StartGame(gameType);
    }



    private bool IsGameOver()
    {
        return (Math.Abs(fieldStatistic.FirstPlayerCells - fieldStatistic.SecondPlayerCells) > fieldStatistic.NeutralCells);
    }

    private void StartGame(GameType gameType)
    {
        if (gameType == GameType.VS_COMPUTER)
        {
            isComputerGamer = true;
        }
        MakeFirstGameStep();
    }

    private void UpdateScreenStats()
    {
        fieldStatistic = GameField.GetStatistics();
        firstPlayerInfoScreen.text = fieldStatistic.FirstPlayerCells.ToString();
        secondPlayerInfoScreen.text = fieldStatistic.SecondPlayerCells.ToString();
    }

    public void ShiftUnactivePlayColors(bool isDeactivating = false)
    {
        foreach (PlayColors currentColor in PlayColors.GetValues(typeof(PlayColors)))
        {
            if (isDeactivating || currentColor == currentChosenColors.FirstPlayerColor || currentColor == currentChosenColors.SecondPlayerColor)
            {
                gameSceneControls.ChangeButtonsActiveByColor(currentColor, false);
            }
            else
            {
                gameSceneControls.ChangeButtonsActiveByColor(currentColor, true);
            }
        }
    }

    private void MakeFirstGameStep()
    {
        currentChosenColors = (PlayColors.GRAY, PlayColors.MAGENTA);
        currentChosenColors.FirstPlayerColor = GameField.MakeFirstStep(isFirstPlayerTurn);
        currentChosenColors.SecondPlayerColor = GameField.MakeFirstStep(!isFirstPlayerTurn);

        GameField.DrawGrid();
        UpdateScreenStats();
        ShiftUnactivePlayColors();
    }

    public void EndGame()
    {
        bool isFirstPlayerWon = (fieldStatistic.FirstPlayerCells > fieldStatistic.SecondPlayerCells) ? true : false;
        gameSceneControls.EndGameHappened(isFirstPlayerWon);
    }

    public void MakeGameStep(PlayColors chosenColor)
    {
        if (IsGameOver())
        {
            return;
        }

        GameField.BFSByColor(chosenColor, isFirstPlayerTurn);
        GameField.DrawGrid();
        UpdateScreenStats();

        if (isFirstPlayerTurn)
        {
            currentChosenColors.FirstPlayerColor = chosenColor;
        } 
        else
        {
            currentChosenColors.SecondPlayerColor = chosenColor;
        }

        ShiftUnactivePlayColors();
        isFirstPlayerTurn = !isFirstPlayerTurn;

        if (IsGameOver())
        {
            EndGame();
            return;
        }

        if (isComputerGamer && !isFirstPlayerTurn)
        {
            chosenColor = secondGamer.MyTurn(GameField, currentChosenColors);
            MakeGameStep(chosenColor);
        }
    }
}
