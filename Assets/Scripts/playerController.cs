using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;
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
        resetMovePoints();
    }

    public void resetMovePoints()
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
        Debug.Log((int)role);

        GameObject neighbour = selectController.checkForEnemy(interactionMap.WorldToCell(transform.position),
            gameLogic.range[(int)role]);

        Debug.Log("Player position " + interactionMap.WorldToCell(transform.position));
        if (neighbour != null)
        {
            neighbour.GetComponent<playerController>().health -= fightController.Attack(this,
                neighbour.GetComponent<playerController>(), gameLogic);
            Debug.Log("New health: " + neighbour.GetComponent<playerController>().health);
            if (neighbour.GetComponent<playerController>().health <= 0)
            {
                neighbour.SetActive(false);
                if (selectController.AllEnemiesDead())
                {
                    SceneManager.LoadScene("Win");
                    SceneManager.UnloadSceneAsync("SampleScene");
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
            Debug.Log("here");
            return false;
        }

        return true;
    }
}