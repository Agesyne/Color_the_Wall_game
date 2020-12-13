using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	
	public GameProvider.GameFieldSize gameFieldSize = GameProvider.GameFieldSize.LITTLE;
    public GameProvider.GameFieldType gameFieldType = GameProvider.GameFieldType.USUAL;
    public GameProvider.GameType gameType = GameProvider.GameType.VS_COMPUTER;
    // Start is called before the first frame update
    void Awake()
    {
		Debug.Log("Awaken");
        GameProvider newGame = new GameProvider(gameFieldSize, gameFieldType, gameType);
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        if (i == 0)
        {
        	Debug.Log("Update");
        	i++;
        }
    }
}
