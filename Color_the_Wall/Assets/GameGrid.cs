using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GameGrid : MonoBehaviour
{
	public int xSize;/*{get; private set;}*/
	public int ySize; /*{get; private set;}*/
	public float CellSize; /*{get; private set;}*/
	public GridCell[,] GamePlate;
	private System.Random Randomizer;
	private enum PlayColors
	{
		GRAY = 0,
		YELLOW,
		RED,
		BLUE,
		GREEN,
		MAGENTA
	}


	private Color GetColor(PlayColors colorName)
	{
		switch(colorName)
		{
			case PlayColors.GRAY:
				return Color.grey;

			case PlayColors.YELLOW:
				return Color.yellow;

			case PlayColors.RED:
				return Color.red;

			case PlayColors.BLUE:
				return Color.blue;

			case PlayColors.GREEN:
				return Color.green;

			default: /*case PlayColors.MAGENTA:*/
				return Color.magenta;
		};
	}

	private PlayColors GeneratePlayColor()
	{
		if (Randomizer == null)
		{
			Randomizer = new System.Random();
		}

		return (PlayColors)Randomizer.Next(6);
	}


    private void Awake ()
    {
    	Generate();
    	Draw();
    }

    private Vector3[] vertices;
    private Mesh mesh;
    private Vector2[] uv;

    private void Generate()
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


		vertices = new Vector3[xSize * ySize * 4];
		uv = new Vector2[vertices.Length];
		Debug.Log(vertices.Length);
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

    private void Draw()
    {
    	GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
		//Debug.Log(vertices.Length);
		mesh.vertices = vertices;
		mesh.uv = uv;


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
