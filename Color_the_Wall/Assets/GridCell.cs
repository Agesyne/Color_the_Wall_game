using System.Collections;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Color color {get; private set;}
    public bool isNeutralZone {get; private set;}
    public bool isFirstPlayerZone {get; private set;}
    public Vector3[] vertices {get; private set;}

    public GridCell(int x, int y, float size)
    {
    	isNeutralZone = true;
    	isFirstPlayerZone = true;
    	vertices = new Vector3[4];
    	float X = x * size;
    	float Y = y * size;
    	vertices[0] = new Vector3(X, Y);
    	vertices[1] = new Vector3(X + size, Y);
    	vertices[2] = new Vector3(X, Y + size);
    	vertices[3] = new Vector3(X + size, Y + size);
    }

    /*public Vector3[] DrawCell(bool isFirst)
    {
    	return (isFirst) : new Vector3[]{vertices[0], vertices[1], vertices[2]}, {vertices[2], vertices[1], vertices[3]} };
    }*/

    public void SetPlayerZone(bool isFirstPlayerTurn)
    {
    	isNeutralZone = false;
    	isFirstPlayerZone = isFirstPlayerTurn;
    }

    public void SetColor(Color gameColor)
    {
    	color = gameColor;
    }
}
