using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private Guid guid = Guid.Empty;

    private readonly Grid grid;
    private readonly PreviewSystem previewSystem;
    private readonly ObjectPlacer objectPlacer;
    private readonly GridData floorData;
    private readonly GridData furnitureData;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         ObjectPlacer objectPlacer,
                         GridData floorData,
                         GridData furnitureData)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectPlacer = objectPlacer;
        this.floorData = floorData;
        this.furnitureData = furnitureData;

        previewSystem.StartShowingRemovePreview();
    }

    public void OnAction(Vector3Int gridCellPosition)
    {
        var isFurniture = !furnitureData.CanPlaceObject(gridCellPosition, Vector2Int.one);
        var isFloor = !floorData.CanPlaceObject(gridCellPosition, Vector2Int.one);
        var selectedData = isFurniture ? furnitureData : isFloor ? floorData : null;
        if (selectedData == null) return;
        guid = selectedData.GetRepresentationIndex(gridCellPosition);
        if (guid == Guid.Empty) return;
        selectedData.RemoveObject(gridCellPosition);
        objectPlacer.RemoveObject(guid);
        var cellPos = grid.CellToWorld(gridCellPosition);
        previewSystem.UpdatePosition(cellPos, CheckSelectionIsValid(gridCellPosition));
    }

    public void UpdateState(Vector3Int gridCellPosition)
    {
        var validity = CheckSelectionIsValid(gridCellPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridCellPosition), validity);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckSelectionIsValid(Vector3Int gridCellPosition)
    {
        var isFurniture = !furnitureData.CanPlaceObject(gridCellPosition, Vector2Int.one);
        var isFloorValid = floorData.CanPlaceObject(gridCellPosition, Vector2Int.one);
        return isFurniture && isFloorValid;
    }
}