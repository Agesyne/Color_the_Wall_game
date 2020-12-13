using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamer
{
    GameProvider.PlayColors MyTurn(GameGrid gameGrid);
}
