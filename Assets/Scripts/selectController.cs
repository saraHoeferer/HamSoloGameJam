using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Tilemaps;

public class selectController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Camera camera;

    [SerializeField] GameObject[] players;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            foreach (GameObject gameObjects in players) {
                gameObjects.GetComponent<playerController>().enabled = false;
            }

            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);

            Debug.Log(gridPosition);
            Debug.Log(interactionMap.WorldToCell(transform.position));
            
            foreach (GameObject gameObjects in players) {
                if (gridPosition == interactionMap.WorldToCell(gameObjects.transform.position)) {
                    gameObjects.transform.GetComponent<playerController>().enabled = true;
                    break;
                }
            }
        }
    }

    public GameObject checkForNeighbours(Vector3 poistion, int range) {
        foreach (GameObject gameObjects in players) {
            if (poistion + new Vector3(range,0,0) == interactionMap.WorldToCell(gameObjects.transform.position)) {
                return gameObjects;
            } else if (poistion + new Vector3(-range,0,0) == interactionMap.WorldToCell(gameObjects.transform.position)){
                return gameObjects;
            } else if (poistion + new Vector3(0,range,0) == interactionMap.WorldToCell(gameObjects.transform.position)){
                return gameObjects;
            } else if (poistion + new Vector3(0,-range,0) == interactionMap.WorldToCell(gameObjects.transform.position)){
                return gameObjects;
            }
        }
        return null;
    }

    public bool checkForNeighboursNextPosition(Vector3 position){
        foreach (GameObject gameObjects in players) {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position)) {
                return true;
            }
        }
        return false;
    }
}
