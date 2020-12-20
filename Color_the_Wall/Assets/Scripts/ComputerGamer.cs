using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerGamer : IGamer
{
    GameProvider.PlayColors IGamer.MyTurn(GameGrid gameGrid)
    {
    	GameProvider.PlayColors chosenColor = (GameProvider.PlayColors)0;
    	int maxResult = -1;
    	foreach (GameProvider.PlayColors currentColor in GameProvider.PlayColors.GetValues(typeof(GameProvider.PlayColors)))
    	{
    		if (gameGrid.WFSByColor(currentColor, false, true) > maxResult)
    		{
    			chosenColor = currentColor;
    		}
    	}
    	return chosenColor;
    }
}
