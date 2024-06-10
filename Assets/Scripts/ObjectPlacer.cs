using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Dictionary<Guid, GameObject> placedGameObject;

    private void Awake()
    {
        placedGameObject = new();
    }

    public Guid PlaceObject(GameObject prefab, Vector3 pos)
    {
        var newObject = Instantiate(prefab);
        newObject.transform.position = pos;
        var id = Guid.NewGuid();
        placedGameObject.Add(id, newObject);
        return id;
    }

    public void RemoveObject(Guid id)
    {
        if (!placedGameObject.ContainsKey(id)) return;
        Destroy(placedGameObject[id]);
        placedGameObject.Remove(id);
    }
}