using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (Input.GetKeyDown(KeyCode.Escape) && !SceneManager.GetSceneByName("Options").IsValid())
        {
            SceneManager.LoadScene("Options", LoadSceneMode.Additive);
        }

        if(!SceneManager.GetSceneByName("Options").IsValid())
        {
            if (Input.GetMouseButtonDown(1))
            {
                foreach (GameObject gameObjects in allies)
                {
                    gameObjects.GetComponent<playerController>().enabled = false;
                }

                Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);

                Debug.Log(gridPosition);


                foreach (GameObject gameObjects in allies)
                {
                    Debug.Log("Korrdinaten von den Units " +
                              interactionMap.WorldToCell(gameObjects.transform.position));
                    if (gridPosition == interactionMap.WorldToCell(gameObjects.transform.position))
                    {
                        Debug.Log("Figur von Spieler");
                        gameObjects.transform.GetComponent<playerController>().enabled = true;
                        break;
                    }
                }
            }
        }
    }

    public bool AllEnemiesDead()
    {
        foreach (var enemie in enemies)
        {
            if (enemie.activeInHierarchy)
                return false;
        }

        return true;
    }

    public GameObject checkForEnemy(Vector3 position, int range)
    {
        foreach (GameObject gameObject in enemies)
        {
            Vector3 enemyPosition = interactionMap.WorldToCell(gameObject.transform.position);
            float distance = Vector3.Distance(position, enemyPosition);
            if (distance <= range)
            {
                return gameObject;
            }
        }

        return null;
    }

    public bool checkForUnitNextPosition(Vector3 position)
    {
        foreach (GameObject gameObjects in enemies)
        {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position))
            {
                return true;
            }
        }

        foreach (GameObject gameObjects in allies)
        {
            if (position == interactionMap.WorldToCell(gameObjects.transform.position))
            {
                return true;
            }
        }

        return false;
    }
}