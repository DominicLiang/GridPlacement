using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    private readonly Dictionary<Vector3Int, PlacementData> placedObject;

    public GridData()
    {
        placedObject = new();
    }

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int id, Guid guid)
    {
        var occupiedPosition = CalculatePosition(gridPosition, objectSize);
        var data = new PlacementData(occupiedPosition, id, guid);
        foreach (var pos in occupiedPosition)
        {
            if (placedObject.ContainsKey(pos)) continue;
            placedObject.Add(pos, data);
        }
    }

    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objectSize)
    {
        var occupiedPosition = CalculatePosition(gridPosition, objectSize);
        return !occupiedPosition.Any(x => placedObject.ContainsKey(x));
    }

    public Guid GetRepresentationIndex(Vector3Int gridCellPosition)
    {
        if (!placedObject.ContainsKey(gridCellPosition)) return Guid.Empty;
        return placedObject[gridCellPosition].Guid;
    }

    public void RemoveObject(Vector3Int gridCellPosition)
    {
        foreach (var pos in placedObject[gridCellPosition].OccupiedPositions)
        {
            placedObject.Remove(pos);
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int objectSize)
    {
        var returnValue = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnValue.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnValue;
    }
}

public class PlacementData
{
    public List<Vector3Int> OccupiedPositions { get; private set; }
    public int Id { get; private set; }
    public Guid Guid { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int id, Guid guid)
    {
        OccupiedPositions = occupiedPositions;
        Id = id;
        Guid = guid;
    }
}