using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEditor;
using TMPro;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject _uiManager;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector3 direction;

    private PlayerInput _playerInput;
    public InputActionMap playerMap;
    public InputActionMap uiMap;


    //public Tilemap fogOfWar;

    [SerializeField] bool hasMoved;

    #region init
    private void Awake()
    {
        // setting up the PlayerInput
        _playerInput = GetComponent<PlayerInput>();
        playerMap = _playerInput.actions.FindActionMap("Player");
        uiMap = _playerInput.actions.FindActionMap("UI");
    }

    private void OnPause()
    {
        _uiManager.GetComponent<TestUiManager>().CallPause();

        Debug.Log("Pause was triggered");
    }

    private void OnEnable()
    {
        playerMap.Enable();
        uiMap.Disable();
    }

    private void OnDisable()
    {
        playerMap.Disable();
        uiMap.Enable();
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    #endregion

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

    #region Pausemenü


    #endregion

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

}
