using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum playerRole
{
    Warrior = 0,
    Magician = 1,
    Archer = 2
}

public class playerController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Tilemap collisionMap;

    [SerializeField] GameObject nextPlayer;

    [SerializeField] Camera camera;

    public playerRole role;

    public int movementPoints;
    private int currentMovementPoints;

    private selectController selectController;

    private int health;

    private gameLogic gameLogic;

    public void Awake()
    {
        selectController = GetComponentInParent<selectController>();
        gameLogic = GameObject.Find("Gamge").GetComponent<gameLogic>();
        health = gameLogic.health[(int)role];
        ResetMovePoints();
    }

    public void ResetMovePoints()
    {
        currentMovementPoints = movementPoints;
        Debug.Log("movepoints reset: " + name + " remaining: " + currentMovementPoints);
    }

    private void Update()
    {
        if (!SceneManager.GetSceneByName("Options").IsValid())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);
                Vector3Int movement = (interactionMap.WorldToCell(transform.position) - gridPosition) * -1;

                Debug.Log(interactionMap.WorldToCell(transform.position));
                Debug.Log(gridPosition);
                Debug.Log((interactionMap.WorldToCell(transform.position) - gridPosition) * -1);

                moveTileByTile(movement);

                transform.GetComponent<playerController>().enabled = false;

                /*
                Debug.Log(gridPosition);
                if (tilemap.HasTile(gridPosition))
                {
                    Debug.Log("tile existend");
                    Debug.Log(tilemap.GetTile(gridPosition).name);
                }
                */
            }
        }
    }


    private IEnumerator WalkTile(int walk, Vector2 direction)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for (int i = 0; i < walk; i++)
        {
            if (currentMovementPoints == 0)
            {
                break;
            }

            if (!Move(direction))
            {
                break;
            }

            currentMovementPoints--;
            yield return wait;
        }

        Debug.Log(role);
        
        List<GameObject> neighbour = selectController.checkForEnemy(interactionMap.WorldToCell(transform.position),
            gameLogic.range[(int)role]);

        Debug.Log("Player position " + interactionMap.WorldToCell(transform.position));
        
        Vector3Int position = interactionMap.WorldToCell(transform.position);
        
        if (interactionMap.GetSprite(position).name == "ChurchRed" || interactionMap.GetSprite(position).name == "ChurchBlue")
        {
            OnChurch();
        }
        
        if (neighbour != null)
        {
            if (neighbour.Count == 1)
            {
                neighbour.First().GetComponent<playerController>().health -= fightController.Attack(
                    this,
                    neighbour.First().GetComponent<playerController>(),
                    gameLogic);
                Debug.Log("Normal Attack -> New health: " + neighbour.First().GetComponent<playerController>().health);
            }
            else
            {
                foreach (var enemie in neighbour)
                {
                    health -= fightController.Attack(
                        enemie.GetComponent<playerController>(),
                        this,
                        gameLogic);
                }

                if (health <= 0)
                {
                    gameObject.SetActive(false);
                    if (selectController.AllAlliesDead())
                    {
                        if (nextPlayer.name == "Player1")
                            SceneManager.LoadScene("Player2Win");
                        else
                        {
                            SceneManager.LoadScene("Player1Win");
                        }

                        SceneManager.UnloadSceneAsync("SaraScene");
                    }
                }
            }

            if (neighbour.First().GetComponent<playerController>().health <= 0)
            {
                neighbour.First().SetActive(false);
                if (selectController.AllEnemiesDead())
                {
                    if (gameObject.transform.parent.name == "Player1")
                    {
                        SceneManager.LoadScene("Player1Win");
                    }
                    else
                    {
                        SceneManager.LoadScene("Player2Win");
                    }

                    SceneManager.UnloadSceneAsync("SaraScene");
                }
            }
        }
    }


    private void moveTileByTile(Vector3Int movement)
    {
        if (movement.x > 0)
        {
            StartCoroutine(WalkTile(movement.x, new Vector2(1, 0)));
        }
        else if (movement.x < 0)
        {
            StartCoroutine(WalkTile((movement.x) * -1, new Vector2(-1, 0)));
        }
        else if (movement.y > 0)
        {
            StartCoroutine(WalkTile(movement.y, new Vector2(0, 1)));
        }
        else if (movement.y < 0)
        {
            StartCoroutine(WalkTile((movement.y) * -1, new Vector2(0, -1)));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
    }

    private bool Move(Vector2 direction)
    {
        Debug.Log(direction);
        direction.y = direction.y * 0.16f;
        direction.x = direction.x * 0.16f;
        Vector3Int gridPosition = interactionMap.WorldToCell(transform.position + (Vector3)(direction));


        if (CanMove(gridPosition))
        {
            transform.position += (Vector3)direction;
            return true;
        }

        return false;
    }

    private bool CanMove(Vector3Int gridPosition)
    {
        if (!interactionMap.HasTile(gridPosition) || collisionMap.HasTile(gridPosition) ||
            selectController.checkForUnitNextPosition(gridPosition))
        {
            return false;
        }

        return true;
    }

    private void OnChurch()
    {
        Debug.Log("OnChurch");
        health += 5;
        if (health > gameLogic.health[(int)role])
        {
            health = gameLogic.health[(int)role];
        }
    }
}