using UnityEngine;

/// <summary>
/// checks out if a tile you want to move to is walkable or not
/// </summary>
public class PlayerPointer : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private bool _obstacle;
    private bool _movedPointer;
    public bool obstacle => _obstacle;

    private void Awake()
    {
        _obstacle = false;
        _movedPointer = _player.GetComponent<PlayerController>().movedPointer;
    }

    /// <summary>
    /// if it collides with an obstacle (layer9(, the _obstale bool will be set to true and the PlayerPointer (and therefor the PlayerPosition) will be reset
    /// </summary>
    /// <param name="col"></param>
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

    public void SetToPlayer()
    {
        this.gameObject.transform.position = _player.GetComponent<PlayerController>().playerPos;
    }

    public void SetToBefore()
    {
        this.gameObject.transform.position = _player.GetComponent<PlayerController>().lastPlayerPos;
    }
}
