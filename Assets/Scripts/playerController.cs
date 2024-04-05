using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public class playerController : MonoBehaviour
{
    [SerializeField] Tilemap interactionMap;
    [SerializeField] Tilemap collisionMap;

    [SerializeField] Camera camera;

    public int movementPoints;

    private PlayerInputs controls;

    public void Awake()
    {
        controls = new PlayerInputs();
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
            Vector2 direction = new Vector2(movement.x, movement.y);
            //Move(direction);
            moveTileByTile(movement);
            
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
            Move(direction);
            movement--;
            yield return wait;
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

    private void Move(Vector2 direction)
    {
        Debug.Log(direction);
        direction.y = direction.y * 0.64f;
        direction.x = direction.x * 0.64f;
        if (CanMove(direction)) 
        {
            transform.position += (Vector3)direction;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPostition = interactionMap.WorldToCell(transform.position + (Vector3)(direction));
        Debug.Log(gridPostition);
        if (!interactionMap.HasTile(gridPostition) || collisionMap.HasTile(gridPostition))
        {
            return false;
        }
        return true;    
    }
}
