using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour
{
    private Mesh fieldMesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private int[] triangles;
    private GameGrid gameGridComponent;
    private List<GameObject> linesList;
    private Dictionary<string, Color> screenColors;

    void Start()
    {
        gameGridComponent = GameObject.Find("GameField").GetComponent<GameGrid>();
        linesList = new List<GameObject>();

        // Initialize and fill dictionary of colors
        screenColors = new Dictionary<string, Color>();
        screenColors.Add("GRAY", new Color(0.2f, 0.2f, 0.2f, 1f));
        screenColors.Add("YELLOW", new Color(0.2f, 0.2f, 0f, 1f));
        screenColors.Add("RED", new Color(0.2f, 0f, 0f, 1f));
        screenColors.Add("BLUE", new Color(0f, 0f, 0.2f, 1f));
        screenColors.Add("GREEN", new Color(0f, 0.2f, 0f, 1f));
        screenColors.Add("MAGENTA", new Color(0.2f, 0f, 0.2f, 1f));

        screenColors.Add("DARK_GRAY", new Color(0.4f, 0.4f, 0.4f, 1f));
        screenColors.Add("DARK_YELLOW", new Color(0.5f, 0.5f, 0f, 1f));
        screenColors.Add("DARK_RED", new Color(0.75f, 0f, 0f, 1f));
        screenColors.Add("DARK_BLUE", new Color(0f, 0f, 0.75f, 1f));
        screenColors.Add("DARK_GREEN", new Color(0f, 0.5f, 0f, 1f));
        screenColors.Add("DARK_MAGENTA", new Color(0.5f, 0f, 0.5f, 1f));
    }

    public void MakeMesh()
    {
        GridCell[,] gamePlate = gameGridComponent.GameField;
        (int X, int Y) size = gameGridComponent.Size;
        float cellSize = gameGridComponent.CellSize;

        GetComponent<MeshFilter>().mesh = fieldMesh = new Mesh();
        fieldMesh.name = "Procedural Grid";
        vertices = new Vector3[size.X * size.Y * 4];
        normals = new Vector3[vertices.Length];
        triangles = new int[size.X * size.Y * 6];
        var normalsVector = new Vector3(0, 1, 3);

        // Creating and assigning arrays of vertices and normals for rendering mesh
        for (int i = 0, y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++, i += 4)
            {
                Transform currentTransform = gamePlate[y, x].CellTransform;
                float X = currentTransform.position.x;
                float Y = currentTransform.position.y;
                vertices[i] = new Vector3(X, Y);
                vertices[i + 1] = new Vector3(X + cellSize, Y);
                vertices[i + 2] = new Vector3(X, Y + cellSize);
                vertices[i + 3] = new Vector3(X + cellSize, Y + cellSize);
                normals[i] = normals[i + 1] = normals[i + 2] = normals[i + 3] = normalsVector;
            }
        }
        fieldMesh.vertices = vertices;
        fieldMesh.normals = normals;

        // Creating and assigning array of triagles for rendering mesh
        for (int tranglesIndex = 0, verticesIndex = 0, y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++, tranglesIndex += 6, verticesIndex += 4)
            {
                triangles[tranglesIndex] = verticesIndex;
                triangles[tranglesIndex + 1] = triangles[tranglesIndex + 4] = verticesIndex + 1;
                triangles[tranglesIndex + 3] = triangles[tranglesIndex + 2] = verticesIndex + 2;
                triangles[tranglesIndex + 5] = verticesIndex + 3;
            }
        }
        fieldMesh.triangles = triangles;

        // Makes borderds and lines separating cells with different colors
        for (var y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++)
            {
                gameGridComponent.MakeLineByCell((x, y), linesList);
            }
        }
    }


    private Color GetCellColor(GameProvider.PlayColors playColor, bool isNeutralCell)
    {
        if (isNeutralCell)
        {
            switch(playColor)
            {
                case GameProvider.PlayColors.GRAY:
                    return screenColors["GRAY"];

                case GameProvider.PlayColors.YELLOW:
                    return screenColors["YELLOW"];

                case GameProvider.PlayColors.RED:
                    return screenColors["RED"];

                case GameProvider.PlayColors.BLUE:
                    return screenColors["BLUE"];

                case GameProvider.PlayColors.GREEN:
                    return screenColors["GREEN"];

                default:
                    return screenColors["MAGENTA"];
            }
        }
        else
        {
            switch(playColor)
            {
                case GameProvider.PlayColors.GRAY:
                    return screenColors["DARK_GRAY"];

                case GameProvider.PlayColors.YELLOW:
                    return screenColors["DARK_YELLOW"];

                case GameProvider.PlayColors.RED:
                    return screenColors["DARK_RED"];

                case GameProvider.PlayColors.BLUE:
                    return screenColors["DARK_BLUE"];

                case GameProvider.PlayColors.GREEN:
                    return screenColors["DARK_GREEN"];

                default:
                    return screenColors["DARK_MAGENTA"];
            }
        }
    }

    public void DrawMesh()
    {
        GridCell[,] gameField = gameGridComponent.GameField;
        (int X, int Y) size = gameGridComponent.Size;

        // Creating and assigning array of colors to paint grid cells 
        Color[] colors = new Color[vertices.Length];
        for (int i = 0, y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++, i += 4)
            {
                colors[i] = colors[i + 1] = colors[i + 2] = colors[i + 3] = GetCellColor(gameField[y, x].Color, gameField[y, x].IsNeutralZone);
            }
        }
        fieldMesh.colors = colors;
    }

    public void LightUpCapturedCell((int X, int Y) cell)
    {
        int i = cell.Y * 4 + cell.X;
        normals[i] = normals[i + 1] = normals[i + 2] = normals[i + 3] = new Vector3(0,0,1);
    }
}
