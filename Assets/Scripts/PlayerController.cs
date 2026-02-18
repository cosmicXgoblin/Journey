using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEditor;
using TMPro;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Manager")]
    [SerializeField] private GameObject _uiManager;

    [Header("Movement")]
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector3 direction;
    private PlayerInput _playerInput;
    public InputActionMap playerMap;
    public InputActionMap uiMap;
    [SerializeField] bool hasMoved;

    //[Header("Fog of War")]
    //public Tilemap fogOfWar;



    #region init
    private void Awake()
    {
        // setting up the PlayerInput, getting all the references
        _playerInput = GetComponent<PlayerInput>();
        playerMap = _playerInput.actions.FindActionMap("Player");
        uiMap = _playerInput.actions.FindActionMap("UI");

        this.transform.position = new Vector3(-332.33f, -157.89f, -4.2f);           // übergangsweise
    }

    //public void OnNewGame()
    //{
    //    this.transform.position = new Vector3(-332.33f, -157.89f, -4.2f);
    //}

    private void OnPause()
    {
        _uiManager.GetComponent<UiManager>().CallPause();
    }

    private void OnEnable()
    {
        playerMap.Enable();
        uiMap.Disable();
    }

    private void OnDisable()
    {
        uiMap.Enable();
        playerMap.Disable();
    }

    /// <summary>
    /// The PlayerController checks for y-Input (up/down). If there is none, hasMoved will be set to false, so the player will be able
    /// to move the next time Update() runs. If there is some while hasMoved is true, a function to check the direction the player should
    /// move int is called.
    /// </summary>
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

    #endregion

    #region Movement
    /// <summary>
    /// This function will give us the direction the player should move in.
    /// It'll check if the y-Input was under 0, so the direction will be going doing.
    /// The next stept is checking if it should go down&right, down&left or just down. This is checked via the x-Input.
    /// Same thing for up, up&righ, up&left.
    /// </summary>
    public void GetMovementDirection()
    {
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //}

    #endregion

    #region Fog of War
    //public int vision = 1;

    //void UpdateFogOfWar()
    //{
    //    Vector3Int currentPlayerTile = fogOfWar.WorldToCell(transform.position);

    //    //clear the surrounding tiles
    //    for(int x=-vision; x<= vision; x++)
    //    {
    //        for(int y=-vision; y<= vision; y++)
    //        {
    //            fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
    //        }
    //    }
    //}
    #endregion

    #region IDataPersistence
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    #endregion
}
