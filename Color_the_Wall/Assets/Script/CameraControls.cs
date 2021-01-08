using UnityEngine;


public class CameraControls : MonoBehaviour
{
    private GameGrid GameField;
    private GameObject GameCanvas;


    void Start()
    {
        GameField = GameObject.Find("GameField").GetComponent<GameGrid>();
        GameCanvas = GameObject.Find("Game_Canvas");
    }


    private void SetCameraPosition()
    {
        (float X, float Y) screenShift = GameField.GetCenterPointOfScreen();
        transform.position = new Vector3(screenShift.X, screenShift.Y, 0);
    }

    private void SetCameraSize()
    {
        float cameraSize = 1470f / 1920f * GameField.Size.X / 2f;
        float screenResolutionCoof = 1;
        Vector2 gameScreen = GameCanvas.GetComponent<RectTransform>().sizeDelta;

        if (gameScreen.x < 1921f)
        {
            screenResolutionCoof = gameScreen.y / 1080f;
        }

        GetComponent<Camera>().orthographicSize = cameraSize * screenResolutionCoof;
    }

    public void SetCameraProperties()
    {
        SetCameraPosition();
        SetCameraSize();
    }

}
