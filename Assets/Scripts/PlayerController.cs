using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEditor;
using TMPro;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    [SerializeField] private Vector2 _movementInput;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private GameObject _pointer;
    private PlayerInput _playerInput;
    public InputActionMap playerMap;
    public InputActionMap uiMap;
    public InputActionMap alwaysMap;
    [SerializeField] bool hasTriedToMove;

    [SerializeField] private Vector3 _playerPos;
    [SerializeField] private Vector3 _lastPlayerPos;
    public bool movedPointer;

    public Vector3 playerPos => _playerPos;
    public Vector3 lastPlayerPos => _lastPlayerPos;

    //[Header("Fog of War")]
    //public Tilemap fogOfWar;

    #region init
    private void Awake()
    {
        // setting up the PlayerInput, getting all the references
        _playerInput = GetComponent<PlayerInput>();
        playerMap = _playerInput.actions.FindActionMap("Player");
        uiMap = _playerInput.actions.FindActionMap("UI");
        alwaysMap = _playerInput.actions.FindActionMap("Always");


        this.transform.position = new Vector3(-332.33f, -157.89f, -4.2f);           // übergangsweise
        UpdatePlayerPosition();
        _pointer.GetComponent<PlayerPointer>().SetToPlayer();
    }

    //public void OnNewGame()
    //{
    //    this.transform.position = new Vector3(-332.33f, -157.89f, -4.2f);
    //}

    private void OnPause()
    {
        UiManager.Instance.CallPause();
    }

    private void OnEnable()
    {
        playerMap.Enable();
        uiMap.Disable();
        alwaysMap.Enable();
    }

    private void OnDisable()
    {
        uiMap.Enable();
        playerMap.Disable();
        alwaysMap.Enable();
    }

    /// <summary>
    /// The PlayerController checks for y-Input (up/down). If there is none, hasTriedToMove will be set to false, so the player will be able
    /// to move the next time Update() runs. If there is some while hasTriedToMove is true, a function to check the _direction the player should
    /// move int is called.
    /// </summary>
    void Update()
    {
        if(_movementInput.y == 0)
        {
            hasTriedToMove = false;
            //_direction = new Vector3(0, 0f);
        }
        else if (_movementInput.y != 0 && !hasTriedToMove)
        {
            hasTriedToMove = true;
           // _direction = new Vector3(0, 0f);

            GetMovementDirection();
            //UpdatePlayerPosition();
        }
    }

    #endregion

    #region Movement
    /// <summary>
    /// This function will give us the _direction the player should move in.
    /// It'll check if the y-Input was under 0, so the _direction will be going doing.
    /// The next stept is checking if it should go down&right, down&left or just down. This is checked via the x-Input.
    /// Same thing for up, up&righ, up&left.
    /// </summary>
    public void GetMovementDirection()
    {
        if (_movementInput.y < 0)
        {
            if (_movementInput.x > 0)
            {
                _direction = new Vector3(1.5f, -0.875f);
            }
            else if (_movementInput.x < 0)
            {
                _direction = new Vector3(-1.5f, -0.875f);
            }
            else
            {
                _direction = new Vector3(0, -1.75f, 0);
            }

            //transform.position += _direction;
            //UpdateFogOfWar();
        }
        else if (_movementInput.y > 0)
        {
            if (_movementInput.x > 0)
            {
                _direction = new Vector3(1.5f, 0.875f);
            }
            else if (_movementInput.x < 0)
            {
                _direction = new Vector3(-1.5f, 0.875f);
            }
            else
            {
                _direction = new Vector3(0, 1.75f, 0);
            }

            //transform.position += _direction;
            //UpdateFogOfWar(); 
        }
        _pointer.transform.position += _direction;
        MovePlayerToPointer();
       // movedPointer = true;
       // MovePlayerToPointer();
    }

    public void MovePlayerToPointer()
    {

        transform.position = _pointer.transform.position;
        UpdatePlayerPosition();

        //movedPointer = false;


        //if (_pointer.GetComponent<PlayerPointer>().obstacle == true)
        //{
        //    Debug.Log("You can't move here");
        //}
        //else
        //{
        //    transform.position += _direction;
        //    UpdatePlayerPosition();
        //}
        //_pointer.GetComponent<PlayerPointer>().SetToPlayer();
    }

    public void OnMove(InputValue value)
    {
       _movementInput = value.Get<Vector2>();
    }

    private void UpdatePlayerPosition()
    {
        _lastPlayerPos = _playerPos;
       _playerPos = this.gameObject.transform.position;

        Debug.Log(playerPos);
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
