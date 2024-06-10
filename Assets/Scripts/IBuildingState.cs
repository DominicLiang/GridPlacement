using UnityEngine;

public interface IBuildingState
{
    void OnAction(Vector3Int gridCellPosition);
    void UpdateState(Vector3Int gridCellPosition);
    void EndState();
}