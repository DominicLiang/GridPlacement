using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.005f;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Material previewMaterialPrefab;

    private GameObject previewObject;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PreparePreview(GameObject previewObject)
    {
        var renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            var materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x <= 0 && size.y <= 0) return;
        cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
        cellIndicatorRenderer.material.mainTextureScale = size;
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject == null) return;
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
        if (previewObject == null) return;
        MovePreview(position);
        ApplyFeedbackToPreview(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        var color = validity ? Color.white : Color.red;
        color.a = 0.5f;
        previewMaterialInstance.color = color;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        var color = validity ? Color.white : Color.red;
        color.a = 0.5f;
        cellIndicatorRenderer.material.color = color;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = position + new Vector3(0, previewYOffset);
    }

    public void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}