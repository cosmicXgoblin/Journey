using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerPointer : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private bool _obstacle;
    private bool _movedPointer;
    public bool obstacle => _obstacle;

    private void Awake()
    {
        //this.gameObject.transform.position = _player.GetComponent<PlayerController>().playerPos;
        _obstacle = false;
        _movedPointer = _player.GetComponent<PlayerController>().movedPointer;
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //if (col.gameObject.layer == 10)
    //{
    //    Debug.Log("Enter the Obstacle");
    //    _obstacle = true;
    //    SetToPlayer();
    //}
    //else 
    //{
    //    Debug.Log("Still walkable");
    //    _obstacle = false;
    //    _player.GetComponent<PlayerController>().MovePlayerToPointer();
    //}

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    if (col.gameObject.layer == 9)
    //    {
    //        if (!_movedPointer)
    //            return;

    //        _obstacle = false;
    //        _player.GetComponent<PlayerController>().MovePlayerToPointer();

    //    }
    //}

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            _obstacle = true;
            Debug.Log("Cant walk here!");
            SetToBefore();
            _player.GetComponent<PlayerController>().MovePlayerToPointer();

        }
    }

    //void OnTriggerExit2D(Collider2D col)
    //{
    //    Debug.Log("Exit");
    //    if (col.gameObject.layer != 10)
    //    {
    //        _obstacle = false;
    //    }
    //    _player.GetComponent<PlayerController>().MovePlayerToPointer();
    //}

    public void SetToPlayer()
    {
        this.gameObject.transform.position = _player.GetComponent<PlayerController>().playerPos;
    }

    public void SetToBefore()
    {
        this.gameObject.transform.position = _player.GetComponent<PlayerController>().lastPlayerPos;
    }
}
