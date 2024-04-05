using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public enum playerRole {Warrior = 0, Magician = 1, Archer = 2}
public class playerController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Tilemap collisionMap;

    [SerializeField] GameObject nextPlayer;

    [SerializeField] Camera camera;
    
    public playerRole role;

    public int movementPoints;

    public int[] range = {1, 3, 2};

    public int health;

    public int attack;

    private PlayerInputs controls;

    private selectController selectController;

    public void Awake()
    {
        controls = new PlayerInputs();
        selectController = GameObject.Find("GameLogic").GetComponent<selectController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = interactionMap.WorldToCell(mousePosition);
            Vector3Int movement = (interactionMap.WorldToCell(transform.position) - gridPosition) * -1;

            Debug.Log(interactionMap.WorldToCell(transform.position));
            Debug.Log(gridPosition);
            Debug.Log((interactionMap.WorldToCell(transform.position) - gridPosition)*-1);

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
     

    private IEnumerator WalkTile(int walk, Vector2 direction)
    {
        int movement = movementPoints;
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for (int i = 0; i < walk; i++)
        {
            if (movement == 0)
            {
                break;
            }
            if (!Move(direction)) {
                break;
            }
            movement--;
            yield return wait;
        }
        Debug.Log(role);
        Debug.Log((int)role);
        GameObject neighbour = selectController.checkForNeighbours(interactionMap.WorldToCell(transform.position), range[(int)role]);

            Debug.Log("Player position " + interactionMap.WorldToCell(transform.position));
            if (neighbour != null) {
                Debug.Log("ANGRIFF!");
            }
    }


    private void moveTileByTile(Vector3Int movement)
    {
        if (movement.x > 0)
        {
           StartCoroutine(WalkTile(movement.x, new Vector2(1, 0)));
        } else if (movement.x < 0)
        {
           StartCoroutine(WalkTile((movement.x)*-1, new Vector2(-1, 0)));
        } else if (movement.y > 0)
        {
            StartCoroutine(WalkTile(movement.y, new Vector2(0, 1)));
        } else if (movement.y < 0)
        {
            StartCoroutine(WalkTile((movement.y)*-1, new Vector2(0, -1)));
        }
    }

    public void OnEnable()
    {
        controls.Enable();
    }

    public void OnDisable()
    {
        controls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private bool Move(Vector2 direction)
    {
        Debug.Log(direction);
        direction.y = direction.y * 0.64f;
        direction.x = direction.x * 0.64f;
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
        if (!interactionMap.HasTile(gridPosition) || collisionMap.HasTile(gridPosition) || selectController.checkForNeighboursNextPosition(gridPosition))
        {
            Debug.Log("here");
            return false;
        }
        return true;    
    }
}