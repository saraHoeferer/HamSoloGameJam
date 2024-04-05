using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mousePosition);
            Debug.Log(gridPosition);
            if (tilemap.HasTile(gridPosition)) {
                Debug.Log("tile existend");
                Debug.Log(tilemap.GetTile(gridPosition).name);
            }
        }
    }
}
