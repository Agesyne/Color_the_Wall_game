using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProvider : MonoBehaviour
{
    //public GameObject screen;
    public GameGrid GameField {get; private set;}
    private bool isFirstPlayerTurn = true;
    private IGamer firstGamer;
    private IGamer secondGamer;
    private (int NeutralCells, int FirstPlayerCells, int SecondPlayerCells) fieldStatistic = (0, 0, 0);
    private enum GameSizeConstants
    {
    	LITTLE_X = 33,
    	LITTLE_Y = 21,
    	MIDDLE_X = 65,
    	MIDDLE_Y = 41,
    	BIG_X = 97,
    	BIG_Y = 61
    }
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
    	VS_PLAYER,
    	VS_CONNECTED_PLAYER
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
    			default: //GameField.BIG:
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
    			default: //GameField.BIG:
    				return (int)GameSizeConstants.BIG_Y;
    		}
    	}
    }

    public GameProvider(GameFieldSize gameFieldSize, GameFieldType gameFieldType, GameType gameType)
    {
        Debug.Log("Created GameProvider");
    	GameField = new GameGrid(GetGameFieldSize(true, gameFieldSize), GetGameFieldSize(false, gameFieldSize), gameFieldType == GameFieldType.SIMMETRIC);
        //StartGame(gameType);
    }


    private bool isFirstPlayerWon()
    {
        return (fieldStatistic.FirstPlayerCells > fieldStatistic.SecondPlayerCells);
    }

    private bool isGameOver()
    {
        return (Math.Abs(fieldStatistic.FirstPlayerCells - fieldStatistic.SecondPlayerCells) > fieldStatistic.NeutralCells);
    }

    private void StartGame(GameType gameType)
    {
        firstGamer = (IGamer)new ThisGamer();
        if (gameType == GameType.VS_COMPUTER)
        {
            secondGamer = (IGamer)new ComputerGamer();
        }
        else
        {
            secondGamer = (IGamer)new LocalGamer();
        }

        while (!isGameOver())
        {
            PlayColors chosenColor = PlayColors.GRAY;
            if (isFirstPlayerTurn)
            {
                chosenColor = firstGamer.MyTurn(GameField);
            }
            else
            {
                chosenColor = secondGamer.MyTurn(GameField);
            }

            GameField.WFSByColor(chosenColor, isFirstPlayerTurn);
            GameField.DrawGrid();
            fieldStatistic = GameField.GetStatistics();
        }

        //выводим победителя и предлагаем выбор
    }
}
