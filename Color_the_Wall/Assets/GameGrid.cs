using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GameGrid : MonoBehaviour
{
	private GameObject screen;
	public int xSize {get; private set;}
	public int ySize {get; private set;}
	public float CellSize {get; private set;}
	public GridCell[,] GamePlate {get; private set;}
	private bool isSymmetric;
	private System.Random Randomizer;
	private enum Directions
	{
		UP = 0,
		RIGHT,
		DOWN,
		LEFT
	}

	public GameGrid(int newXSize, int newYSize, bool newIsSymmetric, float newCellSize = 0.1f)
	{
		xSize = newXSize;
		ySize = newYSize;
		CellSize = newCellSize;
		isSymmetric = newIsSymmetric;
		Debug.Log("Created Grid");
		GenerateGrid();
		Debug.Log("Generated Grid");
		DrawGrid();
		Debug.Log("Drawn Grid");


	}

	void Awake()
	{	
		screen = new GameObject("Screen");
		screen.AddComponent<GameGrid>();
		screen.AddComponent<MeshFilter>();
		screen.AddComponent<MeshRenderer>();
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
				default: //Directions.LEFT:
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
				default: //Directions.LEFT:
					return 0;
			}
		}
	}

	private Color GetColor(GameProvider.PlayColors colorName)
	{
		switch(colorName)
		{
			case GameProvider.PlayColors.GRAY:
				return Color.grey;

			case GameProvider.PlayColors.YELLOW:
				return Color.yellow;

			case GameProvider.PlayColors.RED:
				return Color.red;

			case GameProvider.PlayColors.BLUE:
				return Color.blue;

			case GameProvider.PlayColors.GREEN:
				return Color.green;

			default: /*case PlayColors.MAGENTA:*/
				return Color.magenta;
		};
	}

	private GameProvider.PlayColors GeneratePlayColor()
	{
		if (Randomizer == null)
		{
			Randomizer = new System.Random();
		}

		return (GameProvider.PlayColors)Randomizer.Next(6);
	}

	/*
    private void Awake ()
    {
    	Generate();
    	Draw();
    }
    */

    private Vector3[] vertices;
    private Mesh mesh;
    private Vector2[] uv;


    private bool IfOnlyNearColor(int y, int x)
    {
    	if (y != 0)
    	{
    		if (GamePlate[y, x].color == GamePlate[y - 1, x].color)
    		{
    			return false;
    		}
    	}
    	if (x != 0)
    	{
    		if (GamePlate[y, x].color == GamePlate[y, x - 1].color)
    		{
    			return false;
    		}
    	}
    	if (y != ySize)
    	{
    		if (GamePlate[y, x].color == GamePlate[y + 1, x].color)
    		{
    			return false;
    		}
    	}
    	if (x != xSize)
    	{
    		if (GamePlate[y, x].color == GamePlate[y, x + 1].color)
    		{
    			return false;
    		}
    	}
    	return true;
    }

    private void FixColors()
    {
    	int shouldBeReplaced = 0;
    	int colorCopyDirection = 0;
    	for (int y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				if (IfOnlyNearColor(y, x) && (shouldBeReplaced == 0))
				{
					Color newCellColor = Color.grey;
					if (y == 0 || x == 0 || y == ySize || x == xSize)
					{
						if (y != 0)
				    	{
				    		newCellColor = GamePlate[y - 1, x].color;
				    	}
				    	else if (x != 0)
				    	{
				    		newCellColor = GamePlate[y, x - 1].color;
				    	}
				    	else if (y != ySize)
				    	{
				    		newCellColor = GamePlate[y + 1, x].color;
				    	}
				    	else if (x != xSize)
				    	{
				    		newCellColor = GamePlate[y, x + 1].color;
				    	}
				    }
				    else
				    {
				    	newCellColor = GamePlate[y + GetDirection(false, (Directions)colorCopyDirection), x + GetDirection(true, (Directions)colorCopyDirection)].color;
				    	colorCopyDirection = (colorCopyDirection + 1) % 4;
				    }

					GamePlate[y, x].SetColor(newCellColor);
				}
				shouldBeReplaced = (shouldBeReplaced + 1) % 4;
			}
		}
    }


    private void MakeSymmetric()
    {
    	int yCenter = ySize / 2;
    	int xCenter = xSize / 2;
    	for (int y = 0; y <= yCenter; y++)
		{
			for (int x = 0; x <= xCenter / 2; x++)
			{
				GamePlate[y, x].SetColor(GamePlate[ySize - y, xSize - x].color);
			}
		}
    }

    private void GenerateGrid()
    {
    	GamePlate = new GridCell[ySize + 1, xSize + 1];
		for (int y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				GamePlate[y, x] = new GridCell(x, y, CellSize);
				GamePlate[y, x].SetColor(GetColor(GeneratePlayColor()));
			}
		}
		FixColors();
		if (isSymmetric)
		{
			MakeSymmetric();
		}


		vertices = new Vector3[xSize * ySize * 4];
		uv = new Vector2[vertices.Length];
		//Debug.Log(vertices.Length);
		for (int i = 0, y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, i += 4)
			{
				float X = x * CellSize;
		    	float Y = y * CellSize;
		    	vertices[i] = new Vector3(X, Y);
		    	vertices[i + 1] = new Vector3(X + CellSize, Y);
		    	vertices[i + 2] = new Vector3(X, Y + CellSize);
		    	vertices[i + 3] = new Vector3(X + CellSize, Y + CellSize);
		    	uv[i] = new Vector2(X / xSize, Y / ySize);
		    	uv[i + 1] = new Vector2((X + CellSize) / xSize, Y / ySize);
		    	uv[i + 2] = new Vector2(X / xSize, Y / (ySize + CellSize));
		    	uv[i + 3] = new Vector2((X + CellSize) / xSize, Y / (ySize + CellSize));
			}
		}
    }

    public void DrawGrid()
    {
		//GetComponent<Transform>();
		//var link = GameObject.FindWithTag("Player");
		//link.AddComponent()
		//Debug.Log($"{link.name}");
		screen.GetComponent<MeshFilter>().mesh = new Mesh();
		return;
		mesh.name = "Procedural Grid";
		Debug.Log(vertices.Length);
		mesh.vertices = vertices;
		mesh.uv = uv;
		return;


		int[] triangles = new int[xSize * ySize * 6];

		/*for (int y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				triangles[i] = GamePlate[y, x].Draw()
			}
		}*/
		Color[] colors = new Color[vertices.Length];
		for (int i = 0, y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, i += 4)
			{
				colors[i] = colors[i + 1] = colors[i + 2] = colors[i + 3] = GamePlate[y, x].color;
			}
		}
		mesh.colors = colors;

		for (int ti = 0, vi = 0, y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, ti += 6, vi += 4)
			{
				triangles[ti] = vi;
				triangles[ti + 1] = triangles[ti + 4] = vi + 1;
				triangles[ti + 3] = triangles[ti + 2] = vi + 2;
				triangles[ti + 5] = vi + 3;
				//Debug.Log($"t/v: {ti}, {vi},   y/x: {y}, {x}");
			}
		}
		/*for (int i = 0; i < triangles.Length; i += 3)
    	{
    	    Debug.Log($"{triangles[i]}, {triangles[i + 1]}, {triangles[i + 2]}");
    	}*/
		mesh.triangles = triangles;
		//mesh.RecalculateNormals();
    }


    private bool computerCalculatingWFS(int x, int y, Color searchingColor, bool isFirstPlayerTurn)
    {
    	return ((GamePlate[y, x].isNeutralZone && GamePlate[y, x].color == searchingColor) || GamePlate[y, x].isFirstPlayerZone == isFirstPlayerTurn);
    }

    private bool expandingCellsWFS(int x, int y, Color searchingColor, bool isFirstPlayerTurn)
    {
    	bool result = ((GamePlate[y, x].isNeutralZone && GamePlate[y, x].color == searchingColor) || GamePlate[y, x].isFirstPlayerZone == isFirstPlayerTurn);
    	if (GamePlate[y, x].isNeutralZone)
    	{
    		GamePlate[y, x].SetPlayerZone(isFirstPlayerTurn);
    	}
    	if (result)
    	{
    		GamePlate[y, x].SetColor(searchingColor);
    	}
    	return result;
    }

    public int WFSByColor(GameProvider.PlayColors playColor, bool isFirstPlayerTurn, bool isComputerChoosing = false)
    {
    	int result = 0;
    	Func<int, int, Color, bool, bool> operatingFunction;
    	if (isComputerChoosing)
    	{
    		operatingFunction = computerCalculatingWFS;
    	}
    	else
    	{
    		operatingFunction = expandingCellsWFS;
    	}

    	Color searchingColor = GetColor(playColor);
    	(int X, int Y) beginningCell = (0, 0);
    	beginningCell.Y = (isFirstPlayerTurn) ? 0 : ySize;
    	beginningCell.X = (isFirstPlayerTurn) ? 0 : xSize;
    	Queue<(int, int)> nextCells = new Queue<(int X, int Y)>();
    	nextCells.Enqueue(beginningCell);

    	while (nextCells.Count > 0)
    	{
    		(int X, int Y) cell = nextCells.Dequeue();
    		if (cell.Y != 0)
    		{
    			(int X, int Y) newCell = (cell.X + GetDirection(true, Directions.DOWN), cell.Y + GetDirection(false, Directions.DOWN));
    			if (operatingFunction(newCell.X, newCell.Y, searchingColor, isFirstPlayerTurn))
    			{
    				nextCells.Enqueue(newCell);
    				result++;
    			}
    		}
    		if (cell.X != 0)
    		{
    			(int X, int Y) newCell = (cell.X + GetDirection(true, Directions.LEFT), cell.Y + GetDirection(false, Directions.LEFT));
    			if (operatingFunction(newCell.X, newCell.Y, searchingColor, isFirstPlayerTurn))
    			{
    				nextCells.Enqueue(newCell);
    				result++;
    			}
    		}
    		if (cell.Y != ySize)
    		{
    			(int X, int Y) newCell = (cell.X + GetDirection(true, Directions.UP), cell.Y + GetDirection(false, Directions.UP));
    			if (operatingFunction(newCell.X, newCell.Y, searchingColor, isFirstPlayerTurn))
    			{
    				nextCells.Enqueue(newCell);
    				result++;
    			}
    		}
    		if (cell.X != xSize)
    		{
    			(int X, int Y) newCell = (cell.X + GetDirection(true, Directions.RIGHT), cell.Y + GetDirection(false, Directions.RIGHT));
    			if (operatingFunction(newCell.X, newCell.Y, searchingColor, isFirstPlayerTurn))
    			{
    				nextCells.Enqueue(newCell);
    				result++;
    			}
    		}

    	}
    	return result;
    }

    public (int, int, int) GetStatistics()
    {
    	int neutralCells = 0;
    	int firstPlayerCells = 0;
    	int secondPlayerCells = 0;
    	for (int y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				if (GamePlate[y, x].isNeutralZone)
				{
					neutralCells++;
				}
				else if (GamePlate[y, x].isFirstPlayerZone)
				{
					firstPlayerCells++;
				}
				else
				{
					secondPlayerCells++;
				}
			}
		}
		return (neutralCells, firstPlayerCells, secondPlayerCells);
    }
    /*private void OnDrawGizmos()
    {
    	if (vertices == null)
    	{
    		return;
    	}

    	Gizmos.color = Color.black;
    	for (int i = 0; i < vertices.Length; i++)
    	{
    		Gizmos.DrawSphere(vertices[i], 0.1f);
    	}
    }*/
}
