using UnityEngine.SceneManagement;


public static class SceneSwitcher
{
	public static bool IsPlayMenuLoading {get; private set;}
	public static GameProvider.GameFieldSize GameFieldSize {get; private set;}
    public static GameProvider.GameFieldType GameFieldType {get; private set;}
    public static GameProvider.GameType GameType {get; private set;}

    public static void LoadGameScene()
    {
    	SceneManager.LoadScene("GameScene");
    }

    public static void SetGameSettings(GameProvider.GameFieldSize newGameFieldSize, GameProvider.GameFieldType newGameFieldType, GameProvider.GameType newGameType)
    {
    	GameFieldSize = newGameFieldSize;
    	GameFieldType = newGameFieldType;
    	GameType = newGameType;
    }

    public static void LoadMainScene(bool newIsPlayMenuLoading)
    {
    	IsPlayMenuLoading = newIsPlayMenuLoading;
    	SceneManager.LoadScene("MenuScene");
    }
}
