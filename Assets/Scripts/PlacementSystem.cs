using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private ObjectDatabase database;

    private InputManager inputManager;
    private PreviewSystem previewSystem;
    private ObjectPlacer objectPlacer;

    private Grid grid;
    private GridData floorData;
    private GridData furnitureData;
    private Vector3Int lastDetectedPosition;

    private IBuildingState buildingState;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        previewSystem = GetComponent<PreviewSystem>();
        objectPlacer = GetComponent<ObjectPlacer>();

        grid = GetComponent<Grid>();
        floorData = new();
        furnitureData = new();
        lastDetectedPosition = Vector3Int.zero;
    }

    private void Start()
    {
        StopPlacement();
    }

    private void Update()
    {
        if (buildingState == null) return;

        var mouseWorldPosition = inputManager.GetSelectedMapPosition();
        var gridCellPosition = grid.WorldToCell(mouseWorldPosition);

        if (lastDetectedPosition == gridCellPosition) return;

        buildingState.UpdateState(gridCellPosition);
        lastDetectedPosition = gridCellPosition;
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(id,
                                           grid,
                                           previewSystem,
                                           objectPlacer,
                                           database,
                                           floorData,
                                           furnitureData);

        inputManager.OnClick += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RemovingState(grid,
                                          previewSystem,
                                          objectPlacer,
                                          floorData,
                                          furnitureData);

        inputManager.OnClick += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        var mouseWorldPosition = inputManager.GetSelectedMapPosition();
        var gridCellPosition = grid.WorldToCell(mouseWorldPosition);

        buildingState.OnAction(gridCellPosition);
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false);

        buildingState.EndState();

        inputManager.OnClick -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }
}
