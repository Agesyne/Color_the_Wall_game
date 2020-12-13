using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGamer : IGamer
{
    GameProvider.PlayColors IGamer.MyTurn(GameGrid gameGrid)
    {
    	return GameProvider.PlayColors.GREEN;
    }
}
