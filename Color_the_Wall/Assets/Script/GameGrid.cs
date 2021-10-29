using System.Collections.Generic;
using UnityEngine;


public class GameGrid : MonoBehaviour
{
    public GridCell CellPrefab;
    public GameObject RowLinePrefab;
    public GameObject ColumnLinePrefab;
    public GameObject BorderRowLinePrefab;
    public GameObject BorderColumnLinePrefab;

    public (int X, int Y) Size {get; private set;}
    public float CellSize {get; private set;}
    public GridCell[,] GameField {get; private set;}

    private GridMesh gameFieldMesh;
    private (int X, int Y) beginningCell1;
    private (int X, int Y) beginningCell2;
    private bool isSymmetric;
    private System.Random randomizer;
    private enum Directions
    {
        UP = 0,
        RIGHT,
        DOWN,
        LEFT
    }


    public (float, float) GetCenterPointOfScreen()
    {
        Transform centralCellTransform = GameField[Size.Y / 2, Size.X / 2].CellTransform;
        return (centralCellTransform.position.x + CellSize / 2, centralCellTransform.position.y + CellSize / 2);
    }


    public void GameGridStart(int newXSize, int newYSize, bool newIsSymmetric, float newCellSize = 1f)
    {
        Size = (newXSize, newYSize);
        isSymmetric = newIsSymmetric;
        CellSize = newCellSize;
        randomizer = new System.Random();
        beginningCell1 = (Size.X - 1, 0);
        beginningCell2 = (0, Size.Y - 1);

        GenerateGrid();
        gameFieldMesh = GetComponentInChildren<GridMesh>();
        gameFieldMesh.MakeMesh();
        DrawGrid();
    }



    private int GetDirection(bool isX, Directions directionName)
    {
        if (isX)
        {
            switch(directionName)
            {
                case Directions.UP:
                    return 0;
                case Directions.RIGHT:
                    return 1;
                case Directions.DOWN:
                    return 0;
                default:
                    return -1;
            }
        }
        else
        {
            switch(directionName)
            {
                case Directions.UP:
                    return 1;
                case Directions.RIGHT:
                    return 0;
                case Directions.DOWN:
                    return -1;
                default:
                    return 0;
            }
        }
    }


    private (int, int) GetCellByDirection((int X, int Y) cell, Directions direction)
    {
        (int X, int Y) newCell;
        switch (direction)
        {
            case Directions.UP:
                newCell = (cell.X + GetDirection(true, Directions.UP), cell.Y + GetDirection(false, Directions.UP));
                break;
            case Directions.RIGHT:
                newCell = (cell.X + GetDirection(true, Directions.RIGHT), cell.Y + GetDirection(false, Directions.RIGHT));
                break;
            case Directions.DOWN:
                newCell = (cell.X + GetDirection(true, Directions.DOWN), cell.Y + GetDirection(false, Directions.DOWN));
                break;
            default:
                newCell = (cell.X + GetDirection(true, Directions.LEFT), cell.Y + GetDirection(false, Directions.LEFT));
                break;
        }

        return newCell;
    }



    public GameProvider.PlayColors GeneratePlayColor()
    {
        return (GameProvider.PlayColors) randomizer.Next(6);
    }

    private bool CheckIfOnlyDifferentColorsNearby((int X, int Y) cell)
    {
        (int X, int Y) newCell;

        if (cell.Y != 0)
        {
            newCell = GetCellByDirection(cell, Directions.DOWN);
            if (GameField[cell.Y, cell.X].Color == GameField[newCell.Y, newCell.X].Color)
            {
                return false;
            }
        }
        if (cell.X != 0)
        {
            newCell = GetCellByDirection(cell, Directions.LEFT);
            if (GameField[cell.Y, cell.X].Color == GameField[newCell.Y, newCell.X].Color)
            {
                return false;
            }
        }
        if (cell.Y != Size.Y - 1)
        {
            newCell = GetCellByDirection(cell, Directions.UP);
            if (GameField[cell.Y, cell.X].Color == GameField[newCell.Y, newCell.X].Color)
            {
                return false;
            }
        }
        if (cell.X != Size.X - 1)
        {
            newCell = GetCellByDirection(cell, Directions.RIGHT);
            if (GameField[cell.Y, cell.X].Color == GameField[newCell.Y, newCell.X].Color)
            {
                return false;
            }
        }

        return true;
    }

    private void ExpandSizeOfColorGroups()
    {
        int shouldBeReplaced = 0;
        int colorCopyDirection = 0;
        for (var y = 0; y < Size.Y; y++)
        {
            for (var x = 0; x < Size.X; x++)
            {
                if (CheckIfOnlyDifferentColorsNearby((x, y)) && shouldBeReplaced == 0)
                {
                    (int X, int Y) cell = (x, y);
                    (int X, int Y) newCell;

                    if (y == 0 || x == 0 || y == Size.Y - 1 || x == Size.X - 1)
                    {
                        if (y != 0)
                        {
                            newCell = GetCellByDirection(cell, Directions.DOWN);
                        }
                        else if (x != 0)
                        {
                            newCell = GetCellByDirection(cell, Directions.LEFT);
                        }
                        else if (y != Size.Y - 1)
                        {
                            newCell = GetCellByDirection(cell, Directions.UP);
                        }
                        else
                        {
                            newCell = GetCellByDirection(cell, Directions.RIGHT);
                        }
                    }
                    else
                    {
                        newCell = GetCellByDirection(cell, (Directions) colorCopyDirection);
                        colorCopyDirection = (colorCopyDirection + 1) % 4;
                    }

                    GameProvider.PlayColors newCellColor = GameField[newCell.Y, newCell.X].Color;
                    GameField[y, x].SetColor(newCellColor);
                }

                shouldBeReplaced = (shouldBeReplaced + 1) % 4;
            }
        }
    }

    private void FixBeginningCellsColors()
    {
        if (GameField[beginningCell1.Y, beginningCell1.X].Color == GameField[beginningCell2.Y, beginningCell2.X].Color)
        {
            int newBeginningCellColor = ((int) GameField[beginningCell1.Y, beginningCell1.X].Color + 1) % 6;
            GameField[beginningCell1.Y, beginningCell1.X].SetColor((GameProvider.PlayColors) newBeginningCellColor);

            newBeginningCellColor = ((int) GameField[beginningCell2.Y, beginningCell2.X].Color + 2) % 6;
            GameField[beginningCell2.Y, beginningCell2.X].SetColor((GameProvider.PlayColors) newBeginningCellColor);
        }
    }


    private GameProvider.PlayColors InvertPlayColor(GameProvider.PlayColors playColor)
    {
        return (GameProvider.PlayColors) ((int) GameProvider.PlayColors.MAGENTA - (int) playColor);
    }

    private void MakeSymmetric()
    {
        int xCenter = Size.X / 2;

        for (var y = 0; y < Size.Y; y++)
        {
            for (var x = 0; x <= xCenter; x++)
            {
                GameField[y, x].SetColor(InvertPlayColor(GameField[Size.Y - y - 1, Size.X - x - 1].Color));
            }
        }

        // Give central cell unique play color
        if (Size.Y % 2 == 1 && Size.X % 2 == 1)
        {
            (int X, int Y) cell = (Size.X / 2, Size.Y / 2);
            (int X, int Y) newCell;

            foreach (GameProvider.PlayColors currentColor in GameProvider.PlayColors.GetValues(typeof(GameProvider.PlayColors)))
            {
                if (cell.Y != 0)
                {
                    newCell = GetCellByDirection(cell, Directions.DOWN);
                    if (GameField[newCell.Y, newCell.X].Color == currentColor)
                    {
                        continue;
                    }
                }
                if (cell.X != 0)
                {
                    newCell = GetCellByDirection(cell, Directions.LEFT);
                    if (GameField[newCell.Y, newCell.X].Color == currentColor)
                    {
                        continue;
                    }
                }
                if (cell.Y != Size.Y - 1)
                {
                    newCell = GetCellByDirection(cell, Directions.UP);
                    if (GameField[newCell.Y, newCell.X].Color == currentColor)
                    {
                        continue;
                    }
                }
                if (cell.X != Size.X - 1)
                {
                    newCell = GetCellByDirection(cell, Directions.RIGHT);
                    if (GameField[newCell.Y, newCell.X].Color == currentColor)
                    {
                        continue;
                    }
                }

                GameField[cell.Y, cell.X].SetColor(currentColor);
                break;
            }
        }
    }

    private void CreateCell((int X, int Y) creatingCell, float cellSize)
    {
        Vector3 shift = new Vector3(creatingCell.X * cellSize, creatingCell.Y * cellSize, 0f);
        GridCell cell = GameField[creatingCell.Y, creatingCell.X] = Instantiate<GridCell>(CellPrefab);

        cell.InitializeTransform();
        cell.SetColor(GeneratePlayColor());
        cell.CellTransform.SetParent(transform, false);
        cell.CellTransform.localPosition = shift;
    }

    private void GenerateGrid()
    {
        GameField = new GridCell[Size.Y, Size.X];

        for (var y = 0; y < Size.Y; y++)
        {
            for (var x = 0; x < Size.X; x++)
            {
                CreateCell((x, y), CellSize);
            }
        }

        ExpandSizeOfColorGroups();
        FixBeginningCellsColors();

        if (isSymmetric)
        {
            MakeSymmetric();
        }
    }

    public void DrawGrid()
    {
        gameFieldMesh.DrawMesh();
    }


    private bool ShouldBeExpanded((int X, int Y) cell, GameProvider.PlayColors searchingColor, bool isFirstPlayerTurn)
    {
        bool A = GameField[cell.Y, cell.X].Color == searchingColor;
        bool B = GameField[cell.Y, cell.X].IsNeutralZone;
        bool C = GameField[cell.Y, cell.X].IsFirstPlayerZone == isFirstPlayerTurn;
        bool D = GameField[cell.Y, cell.X].IsChecked;

        // Unreduced expression: (A && (B || (!B && C)) || (!A && !B && C))
        return ((!B && C) || (A && B)) && !D;
    }


    private void CaptureCell((int X, int Y) cell, GameProvider.PlayColors capturingColor, bool isFirstPlayerTurn)
    {
        GameField[cell.Y, cell.X].SetColor(capturingColor);

        if (GameField[cell.Y, cell.X].IsNeutralZone)
        {
            (int X, int Y) newCell;
            GameObject line;

            GameField[cell.Y, cell.X].SetPlayerZone(isFirstPlayerTurn);
            gameFieldMesh.LightUpCapturedCell(cell);

            // Disable separating lines if it needs
            if (cell.Y != 0)
            {
                newCell = GetCellByDirection(cell, Directions.DOWN);
                if (!GameField[newCell.Y, newCell.X].IsNeutralZone && GameField[newCell.Y, newCell.X].IsFirstPlayerZone == isFirstPlayerTurn)
                {
                    line = GameField[cell.Y, cell.X].CellTransform.GetChild(0).gameObject;
                    line.SetActive(false);
                }
            }
            if (cell.X != 0)
            {
                newCell = GetCellByDirection(cell, Directions.LEFT);
                if (!GameField[newCell.Y, newCell.X].IsNeutralZone && GameField[newCell.Y, newCell.X].IsFirstPlayerZone == isFirstPlayerTurn)
                {
                    line = GameField[cell.Y, cell.X].CellTransform.GetChild(1).gameObject;
                    line.SetActive(false);
                }
            }
            if (cell.Y != Size.Y - 1)
            {
                newCell = GetCellByDirection(cell, Directions.UP);
                if (!GameField[newCell.Y, newCell.X].IsNeutralZone && GameField[newCell.Y, newCell.X].IsFirstPlayerZone == isFirstPlayerTurn)
                {
                    line = GameField[newCell.Y, newCell.X].CellTransform.GetChild(0).gameObject;
                    line.gameObject.SetActive(false);
                }
            }
            if (cell.X != Size.X - 1)
            {
                newCell = GetCellByDirection(cell, Directions.RIGHT);
                if (!GameField[newCell.Y, newCell.X].IsNeutralZone && GameField[newCell.Y, newCell.X].IsFirstPlayerZone == isFirstPlayerTurn)
                {
                    line = GameField[newCell.Y, newCell.X].CellTransform.GetChild(1).gameObject;
                    line.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ClearBFS()
    {
        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++)
            {
                GameField[y, x].SetUnchecked();
            }
        }
    }

    public int BFSByColor(GameProvider.PlayColors searchingColor, bool isFirstPlayerTurn, bool isComputerChoosing = false)
    {
        ClearBFS();

        int capturedCells = 0;
        (int X, int Y) beginningCell = (isFirstPlayerTurn) ? beginningCell1 : beginningCell2;
        Queue<(int, int)> cellsToCheck = new Queue<(int X, int Y)>();

        cellsToCheck.Enqueue(beginningCell);
        GameField[beginningCell.Y, beginningCell.X].SetChecked();


        while (cellsToCheck.Count > 0)
        {
            (int X, int Y) cell = cellsToCheck.Dequeue();
            (int X, int Y) newCell;

            capturedCells++;
            if (!isComputerChoosing)
            {
                CaptureCell(cell, searchingColor, isFirstPlayerTurn);
            }

            if (cell.Y != 0)
            {
                newCell = GetCellByDirection(cell, Directions.DOWN);
                if (ShouldBeExpanded(newCell, searchingColor, isFirstPlayerTurn))
                {
                    cellsToCheck.Enqueue(newCell);
                    GameField[newCell.Y, newCell.X].SetChecked();
                }
            }
            if (cell.X != 0)
            {
                newCell = GetCellByDirection(cell, Directions.LEFT);
                if (ShouldBeExpanded(newCell, searchingColor, isFirstPlayerTurn))
                {
                    cellsToCheck.Enqueue(newCell);
                    GameField[newCell.Y, newCell.X].SetChecked();
                }
            }
            if (cell.Y != Size.Y - 1)
            {
                newCell = GetCellByDirection(cell, Directions.UP);
                if (ShouldBeExpanded(newCell, searchingColor, isFirstPlayerTurn))
                {
                    cellsToCheck.Enqueue(newCell);
                    GameField[newCell.Y, newCell.X].SetChecked();
                }
            }
            if (cell.X != Size.X - 1)
            {
                newCell = GetCellByDirection(cell, Directions.RIGHT);
                if (ShouldBeExpanded(newCell, searchingColor, isFirstPlayerTurn))
                {
                    cellsToCheck.Enqueue(newCell);
                    GameField[newCell.Y, newCell.X].SetChecked();
                }
            }
        }

        return capturedCells;
    }


    public GameProvider.PlayColors MakeFirstStep(bool isFirstPlayerTurn)
    {
        (int X, int Y) beginningCell = (isFirstPlayerTurn) ? beginningCell1 : beginningCell2;
        GameProvider.PlayColors firstColor = GameField[beginningCell.Y, beginningCell.X].Color;
        
        BFSByColor(firstColor, isFirstPlayerTurn);
        return firstColor;
    }




    private GameObject MakeLine(GameObject linePrefab, Vector3 correctionVector, Transform cellTransform)
    {
        GameObject newLine = Instantiate<GameObject>(linePrefab);
        Transform lineTransform = newLine.GetComponent<Transform>();

        lineTransform.position += cellTransform.position + correctionVector;
        lineTransform.SetParent(cellTransform, true);

        return newLine;
    }

    public void MakeLineByCell((int X, int Y) cell, List<GameObject> linesList)
    { 
        GameProvider.PlayColors searchingColor = GameField[cell.Y, cell.X].Color;
        Transform cellTransform = GameField[cell.Y, cell.X].CellTransform;
        var correctionVector = new Vector3(0, 0, 0);
        (int X, int Y) newCell;
        GameObject newLine;


        newLine = MakeLine(RowLinePrefab, correctionVector, cellTransform);
        linesList.Add(newLine);
        newCell = GetCellByDirection(cell, Directions.DOWN);
        if (!(cell.Y != 0 && GameField[newCell.Y, newCell.X].Color != GameField[cell.Y, cell.X].Color))
        {
            newLine.SetActive(false);
        }

        newLine = MakeLine(ColumnLinePrefab, correctionVector, cellTransform);
        linesList.Add(newLine);
        newCell = GetCellByDirection(cell, Directions.LEFT);
        if (!(cell.X != 0 && GameField[newCell.Y, newCell.X].Color != GameField[cell.Y, cell.X].Color))
           {
            newLine.SetActive(false);
        }


        if (cell.Y == 0 || cell.Y == Size.Y - 1)
        {
            var correctionUpVector = new Vector3(0, 1, 0);
            var correctionDownVector = new Vector3(0, 0, 0);

            if (cell.Y == 0)
            {
                newLine = MakeLine(BorderRowLinePrefab, correctionDownVector, cellTransform);
            }
            else
            {
                newLine = MakeLine(BorderRowLinePrefab, correctionUpVector, cellTransform);
            }

            linesList.Add(newLine);
        }
        if (cell.X == 0 || cell.X == Size.X - 1)
        {
            var correctionRightVector = new Vector3(0, 0, 0);
            var correctionLeftVector = new Vector3(1, 0, 0);

            if (cell.X == 0)
            {
                newLine = MakeLine(BorderColumnLinePrefab, correctionRightVector, cellTransform);
            }
            else
            {
                newLine = MakeLine(BorderColumnLinePrefab, correctionLeftVector, cellTransform);
            }

            linesList.Add(newLine);
        }
    }



    public (int, int, int) GetStatistics()
    {
        int neutralCells = 0;
        int firstPlayerCells = 0;
        int secondPlayerCells = 0;

        for (var y = 0; y < Size.Y; y++)
        {
            for (var x = 0; x < Size.X; x++)
            {
                if (GameField[y, x].IsNeutralZone)
                {
                    neutralCells++;
                }
                else
                {
                    if (GameField[y, x].IsFirstPlayerZone)
                    {
                        firstPlayerCells++;
                    }
                    else
                    {
                        secondPlayerCells++;
                    }
                }
            }
        }

        return (neutralCells, firstPlayerCells, secondPlayerCells);
    }

}
