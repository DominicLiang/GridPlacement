using UnityEngine;

public class PlacementState : IBuildingState
{
    private readonly int selectedIndex = -1;

    private readonly Grid grid;
    private readonly PreviewSystem previewSystem;
    private readonly ObjectPlacer objectPlacer;
    private readonly ObjectDatabase database;
    private readonly GridData floorData;
    private readonly GridData furnitureData;

    public PlacementState(int id,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectPlacer objectPlacer,
                          ObjectDatabase database,
                          GridData floorData,
                          GridData furnitureData)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectPlacer = objectPlacer;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;

        selectedIndex = database.objectData.FindIndex(x => x.Id == id);
        if (selectedIndex < 0) return;
        previewSystem.StartShowingPlacementPreview(database.objectData[selectedIndex].Prefab,
                                                   database.objectData[selectedIndex].Size);
    }

    public void OnAction(Vector3Int gridCellPosition)
    {
        var placementValidity = CheckPlacementValidity(gridCellPosition, selectedIndex);
        if (!placementValidity) return;

        var index = objectPlacer.PlaceObject(database.objectData[selectedIndex].Prefab,
                                             grid.CellToWorld(gridCellPosition));

        var selectedData = database.objectData[selectedIndex].Id == 0 ? floorData : furnitureData;
        selectedData.AddObject(gridCellPosition,
                               database.objectData[selectedIndex].Size,
                               database.objectData[selectedIndex].Id,
                               index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridCellPosition), false);
    }

    public void UpdateState(Vector3Int gridCellPosition)
    {
        var placementValidity = CheckPlacementValidity(gridCellPosition, selectedIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridCellPosition), placementValidity);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValidity(Vector3Int gridCellPosition, int selectedIndex)
    {
        var selectedData = database.objectData[selectedIndex].Id == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObject(gridCellPosition, database.objectData[selectedIndex].Size);
    }
}