using UnityEngine;

public class Map_RestEvents : MonoBehaviour
{
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject gameManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("You arrived at a rest point.");
        }
    }
}
