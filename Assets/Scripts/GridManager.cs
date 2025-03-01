using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager
{
    private Game game;
    private Tilemap currState;
    private Tilemap nxtState;
    private Tile aliveTile;
    private int width;
    private int height;
    private int liveCellPercentage;
    private System.Random random;
    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> adjCells;

    public GridManager(Game game, Tilemap currState, Tilemap nxtState, Tile aliveTile,
                       int width, int height, int liveCellPercentage,
                       System.Random random, HashSet<Vector3Int> aliveCells, HashSet<Vector3Int> adjCells)
    {
        this.game = game;
        this.currState = currState;
        this.nxtState = nxtState;
        this.aliveTile = aliveTile;
        this.width = width;
        this.height = height;
        this.liveCellPercentage = liveCellPercentage;
        this.random = random;
        this.aliveCells = aliveCells;
        this.adjCells = adjCells;
    }

    public void ApplyTileColor()
    {
        if (PlayerPrefs.HasKey("TileColor") && aliveTile != null)
        {
            string colorName = PlayerPrefs.GetString("TileColor");
            Color tileColor = GetColorFromName(colorName);
            if (aliveTile.sprite != null)
            {
                aliveTile.color = tileColor;
            }
        }
    }

    public void SetRandomPattern()
    {
        Clear();
        float aliveProbability = liveCellPercentage / 100f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (random.NextDouble() < aliveProbability)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);
                    currState.SetTile(cell, aliveTile);
                    aliveCells.Add(cell);
                }
            }
        }
    }

    public void Clear()
    {
        currState.ClearAllTiles();
        nxtState.ClearAllTiles();
        aliveCells.Clear();
        adjCells.Clear();
    }

    public void UpdateGrid()
    {
        adjCells.Clear();
        HashSet<Vector3Int> newAliveCells = new HashSet<Vector3Int>();

        // Find the cells we need to check- reduces overall cells we check
        foreach (Vector3Int cell in aliveCells)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int adj = cell + new Vector3Int(i, j, 0);
                    adjCells.Add(adj);
                }
            }
        }

        // Process each cell
        foreach (Vector3Int cell in adjCells)
        {
            int adj = CountNeighbors(cell);
            bool living = IsAlive(cell);

            if (!living && adj == 3)
            {
                nxtState.SetTile(cell, aliveTile);
                newAliveCells.Add(cell);
            }
            else if (living && (adj < 2 || adj > 3))
            {
                nxtState.SetTile(cell, null);
            }
            else if (living)
            {
                nxtState.SetTile(cell, aliveTile);
                newAliveCells.Add(cell);
            }
        }

        // Update the alive cells collection
        aliveCells.Clear();
        foreach (Vector3Int cell in newAliveCells)
        {
            aliveCells.Add(cell);
        }

        // Swap the tilemaps
        Tilemap temp = currState;
        currState = nxtState;
        nxtState = temp;
        nxtState.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int counter = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector3Int adj = cell + new Vector3Int(i, j, 0);
                if (IsAlive(adj))
                {
                    counter++;
                }
            }
        }
        return counter;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currState.GetTile(cell) == aliveTile;
    }

    private Color GetColorFromName(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "black": return Color.black;
            case "white": return Color.white;
            case "red": return Color.red;
            case "green": return Color.green;
            case "blue": return Color.blue;
            case "yellow": return Color.yellow;
            default: return Color.white;
        }
    }
}