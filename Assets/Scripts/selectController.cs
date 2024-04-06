using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class selectController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Camera camera;

    [SerializeField] private Transform opponent;

    private List<GameObject> allies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();


    void Start()
    {
        foreach (Transform ally in transform)
        {
            allies.Add(ally.gameObject);
        }
        foreach (Transform enemy in opponent)
        {
            enemies.Add(enemy.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SceneManager.GetSceneByName("Options").IsValid())
        {
            SceneManager.LoadScene("Options", LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByName("Options").IsValid())
        {
            if (Input.GetMouseButtonDown(1))
            {
                foreach (GameObject gameObjects in allies)
                {
                    gameObjects.GetComponent<playerController>().enabled = false;
                }

                Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);

                foreach (GameObject gameObjects in allies)
                {

                    if (gridPosition == interactionMap.WorldToCell(gameObjects.transform.position) && gameObjects.activeSelf)
                    {
                        gameObjects.transform.GetComponent<playerController>().enabled = true;
                        gameObjects.transform.GetComponent<playerController>().enableFlag();
                        break;
                    }
                }
            }
        }
    }

    public bool AllEnemiesDead()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.activeInHierarchy)
                return false;
        }

        return true;
    }
    
    public bool AllAlliesDead()
    {
        foreach (var ally in allies)
        {
            if (ally.activeInHierarchy)
                return false;
        }

        return true;
    }

    public List<GameObject> checkForEnemy(Vector3 position, int range)
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach (GameObject gameObject in enemies)
        {
            Vector3 enemyPosition = interactionMap.WorldToCell(gameObject.transform.position);
            float distance = Vector3.Distance(position, enemyPosition);
            if (distance <= range)
            {
                enemies.Add(gameObject);
            }
        }

        return null;
    }

    public bool checkForUnitNextPosition(Vector3 position)
    {
        foreach (GameObject gameObjects in enemies)
        {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position) && gameObjects.activeSelf)
            {
                return true;
            }
        }

        foreach (GameObject gameObjects in allies)
        {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position) && gameObjects.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}