using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class selectController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Camera camera;

    [SerializeField] GameObject[] allies;
    [SerializeField] GameObject[] enemies;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            foreach (GameObject gameObjects in allies) {
                gameObjects.GetComponent<playerController>().enabled = false;
            }

            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);

            Debug.Log(gridPosition);
            
            foreach (GameObject gameObjects in allies) {
                Debug.Log("Korrdinaten von den Units "+ interactionMap.WorldToCell(gameObjects.transform.position));
                if (gridPosition == interactionMap.WorldToCell(gameObjects.transform.position)) {
                   
                    Debug.Log("Figur von Spieler");
                    gameObjects.transform.GetComponent<playerController>().enabled = true;
                    break;
                }
            }
        }
    }

    public GameObject checkForEnemy(Vector3 poistion, int range) {
        foreach (GameObject gameObjects in enemies) {
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

    public bool checkForUnitNextPosition(Vector3 position){
        foreach (GameObject gameObjects in enemies) {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position)) {
                return true;
            }
        }
        foreach (GameObject gameObjects in allies) {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position)) {
                return true;
            }
        }
        return false;
    }
}
