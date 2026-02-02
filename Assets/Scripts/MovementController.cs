using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEditor;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector3 direction;

    //[SerializeField] private float fullTile;
    //[SerializeField] private float halfTileWidth;
    //[SerializeField] private float halfTileLength;

    public Tilemap fogOfWar;

    [SerializeField] bool hasMoved;

    void Update()
    {
        if(movementInput.y == 0)
        {
            hasMoved = false;
            //direction = new Vector3(0, 0f);
        }
        else if (movementInput.y != 0 && !hasMoved)
        {
            hasMoved = true;
           // direction = new Vector3(0, 0f);

            GetMovementDirection();
        }
    }

    //public void GetMovementDirection()
    //{
    //    if (movementInput.x < 0)
    //    {
    //        if (movementInput.y > 0)
    //        {
    //            direction = new Vector3(-0.5f, 0.5f);
    //        }
    //        else if (movementInput.y < 0)
    //        {
    //            direction = new Vector3(-0.5f, -0.5f);
    //        }
    //        else
    //        {
    //            direction = new Vector3(-1, 0, 0);
    //        }

    //        transform.position += direction;
    //        //UpdateFogOfWar();
    //    }
    //    else if (movementInput.x > 0)
    //    {
    //        if (movementInput.y > 0)
    //        {
    //            direction = new Vector3(0.5f, 0.5f);
    //        }
    //        else if (movementInput.y < 0)
    //        {
    //            direction = new Vector3(0.5f, -0.5f);
    //        }
    //        else
    //        {
    //            direction = new Vector3(1, 0, 0);
    //        }

    //        transform.position += direction;
    //        //UpdateFogOfWar(); 
    //    }
    //}

    public void GetMovementDirection()
    {
        // input y == -1 && x == 0
        // runter

        // input y == -1 && x == 1
        // schräg rechts runter

        // input y == -1 && x == -1
        // schräg links runter

        if (movementInput.y < 0)
        {
            if (movementInput.x > 0)
            {
                direction = new Vector3(1.5f, -0.875f);
            }
            else if (movementInput.x < 0)
            {
                direction = new Vector3(-1.5f, -0.875f);
            }
            else
            {
                direction = new Vector3(0, -1.75f, 0);
            }

            transform.position += direction;
            //UpdateFogOfWar();
        }
        // input y == 1 && x == 0
        // hoch

        // input y == 1 && x == -1
        // schräg rechts hoch

        // input y == 1 && x == 1
        // schräg links hoch
        else if (movementInput.y > 0)
        {
            if (movementInput.x > 0)
            {
                direction = new Vector3(1.5f, 0.875f);
            }
            else if (movementInput.x < 0)
            {
                direction = new Vector3(-1.5f, 0.875f);
            }
            else
            {
                direction = new Vector3(0, 1.75f, 0);
            }

            transform.position += direction;
            //UpdateFogOfWar(); 
        }
    }

    public void OnMove(InputValue value)
    {
       movementInput = value.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public int vision = 1;

    void UpdateFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(transform.position);

        //clear the surrounding tiles
        for(int x=-vision; x<= vision; x++)
        {
            for(int y=-vision; y<= vision; y++)
            {
                fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
            }
        }
    }

}
