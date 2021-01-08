using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour
{
    private Mesh FieldMesh;
    private Vector3[] Vertices;
    private Vector3[] Normals;
    private int[] Triangles;
    private GameGrid GameGridComponent;
    private List<GameObject> LinesList;
    private Dictionary<string, Color> ScreenColors;

    void Start()
    {
    	GameGridComponent = GameObject.Find("GameField").GetComponent<GameGrid>();
    	LinesList = new List<GameObject>();

    	// Initialize and fill dictionary of colors
    	ScreenColors = new Dictionary<string, Color>();
    	ScreenColors.Add("GRAY", new Color(0.2f, 0.2f, 0.2f, 1f));
    	ScreenColors.Add("YELLOW", new Color(0.2f, 0.2f, 0f, 1f));
    	ScreenColors.Add("RED", new Color(0.2f, 0f, 0f, 1f));
    	ScreenColors.Add("BLUE", new Color(0f, 0f, 0.2f, 1f));
    	ScreenColors.Add("GREEN", new Color(0f, 0.2f, 0f, 1f));
    	ScreenColors.Add("MAGENTA", new Color(0.2f, 0f, 0.2f, 1f));

    	ScreenColors.Add("DARK_GRAY", new Color(0.4f, 0.4f, 0.4f, 1f));
    	ScreenColors.Add("DARK_YELLOW", new Color(0.5f, 0.5f, 0f, 1f));
    	ScreenColors.Add("DARK_RED", new Color(0.75f, 0f, 0f, 1f));
    	ScreenColors.Add("DARK_BLUE", new Color(0f, 0f, 0.75f, 1f));
    	ScreenColors.Add("DARK_GREEN", new Color(0f, 0.5f, 0f, 1f));
    	ScreenColors.Add("DARK_MAGENTA", new Color(0.5f, 0f, 0.5f, 1f));
    }

    public void MakeMesh()
    {
    	GridCell[,] gamePlate = GameGridComponent.GameField;
    	(int X, int Y) size = GameGridComponent.Size;
    	float cellSize = GameGridComponent.CellSize;

    	GetComponent<MeshFilter>().mesh = FieldMesh = new Mesh();
		FieldMesh.name = "Procedural Grid";
    	Vertices = new Vector3[size.X * size.Y * 4];
		Normals = new Vector3[Vertices.Length];
    	Triangles = new int[size.X * size.Y * 6];
    	var normalsVector = new Vector3(0, 1, 3);

    	// Creating and assigning arrays of vertices and normals for rendering mesh
		for (int i = 0, y = 0; y < size.Y; y++)
		{
			for (var x = 0; x < size.X; x++, i += 4)
			{
				Transform currentTransform = gamePlate[y, x].CellTransform;
				float X = currentTransform.position.x;
		    	float Y = currentTransform.position.y;
		    	Vertices[i] = new Vector3(X, Y);
		    	Vertices[i + 1] = new Vector3(X + cellSize, Y);
		    	Vertices[i + 2] = new Vector3(X, Y + cellSize);
		    	Vertices[i + 3] = new Vector3(X + cellSize, Y + cellSize);
		    	Normals[i] = Normals[i + 1] = Normals[i + 2] = Normals[i + 3] = normalsVector;
			}
		}
		FieldMesh.vertices = Vertices;
		FieldMesh.normals = Normals;

		// Creating and assigning array of triagles for rendering mesh
		for (int tranglesIndex = 0, verticesIndex = 0, y = 0; y < size.Y; y++)
		{
			for (var x = 0; x < size.X; x++, tranglesIndex += 6, verticesIndex += 4)
			{
				Triangles[tranglesIndex] = verticesIndex;
				Triangles[tranglesIndex + 1] = Triangles[tranglesIndex + 4] = verticesIndex + 1;
				Triangles[tranglesIndex + 3] = Triangles[tranglesIndex + 2] = verticesIndex + 2;
				Triangles[tranglesIndex + 5] = verticesIndex + 3;
			}
		}
		FieldMesh.triangles = Triangles;

		// Makes borderds and lines separating cells with different colors
		for (var y = 0; y < size.Y; y++)
		{
			for (var x = 0; x < size.X; x++)
			{
				GameGridComponent.MakeLineByCell((x, y), LinesList);
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
					return ScreenColors["GRAY"];

				case GameProvider.PlayColors.YELLOW:
					return ScreenColors["YELLOW"];

				case GameProvider.PlayColors.RED:
					return ScreenColors["RED"];

				case GameProvider.PlayColors.BLUE:
					return ScreenColors["BLUE"];

				case GameProvider.PlayColors.GREEN:
					return ScreenColors["GREEN"];

				default:
					return ScreenColors["MAGENTA"];
			}
    	}
    	else
    	{
    		switch(playColor)
			{
				case GameProvider.PlayColors.GRAY:
					return ScreenColors["DARK_GRAY"];

				case GameProvider.PlayColors.YELLOW:
					return ScreenColors["DARK_YELLOW"];

				case GameProvider.PlayColors.RED:
					return ScreenColors["DARK_RED"];

				case GameProvider.PlayColors.BLUE:
					return ScreenColors["DARK_BLUE"];

				case GameProvider.PlayColors.GREEN:
					return ScreenColors["DARK_GREEN"];

				default:
					return ScreenColors["DARK_MAGENTA"];
			}
    	}
    }

	public void DrawMesh()
	{
		GridCell[,] gameField = GameGridComponent.GameField;
		(int X, int Y) size = GameGridComponent.Size;

    	// Creating and assigning array of colors to paint grid cells 
		Color[] colors = new Color[Vertices.Length];
		for (int i = 0, y = 0; y < size.Y; y++)
		{
			for (var x = 0; x < size.X; x++, i += 4)
			{
				colors[i] = colors[i + 1] = colors[i + 2] = colors[i + 3] = GetCellColor(gameField[y, x].Color, gameField[y, x].IsNeutralZone);
			}
		}
		FieldMesh.colors = colors;
	}

	public void LightUpCapturedCell((int X, int Y) cell)
	{
		int i = cell.Y * 4 + cell.X;
		Normals[i] = Normals[i + 1] = Normals[i + 2] = Normals[i + 3] = new Vector3(0,0,1);
	}
}
