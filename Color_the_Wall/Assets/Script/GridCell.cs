using UnityEngine;


public class GridCell : MonoBehaviour
{
    public GameProvider.PlayColors Color {get; private set;}
    public bool IsNeutralZone {get; private set;}
    public bool IsFirstPlayerZone {get; private set;}
    public bool IsChecked {get; private set;}
    public Transform CellTransform {get; private set;}


    public GridCell()
    {
        IsNeutralZone = true;
        IsFirstPlayerZone = true;
        IsChecked = false;
    }


    public void InitializeTransform()
    {
        CellTransform = GetComponent<Transform>();
    }

    public void SetPlayerZone(bool isFirstPlayerTurn)
    {
        IsNeutralZone = false;
        IsFirstPlayerZone = isFirstPlayerTurn;
    }

    public void SetColor(GameProvider.PlayColors newPlayColor)
    {
        Color = newPlayColor;
    }

    public void SetUnchecked()
    {
        IsChecked = false;
    }

    public void SetChecked()
    {
        IsChecked = true;
    }
}
