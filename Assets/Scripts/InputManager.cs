using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public event Action OnClick;
    public event Action OnExit;

    [SerializeField] private LayerMask placementLayer;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick?.Invoke();
        if (Input.GetMouseButtonDown(1))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetSelectedMapPosition()
    {
        var lastPosition = new Vector3(0, -100, 0);
        Vector3 mousePos = Input.mousePosition;
        var ray = mainCamera.ScreenPointToRay(mousePos);
        if (!Physics.Raycast(ray, out var hit, 100, placementLayer)) return lastPosition;
        lastPosition = hit.point;
        return lastPosition;
    }
}
