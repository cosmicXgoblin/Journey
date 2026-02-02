using UnityEngine;

public class Map_EnemyEvent : MonoBehaviour
{
    [SerializeField] private GameObject UiManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter2D");

        if (col.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");

            UiManager.GetComponent<TestUiManager>().Fight();
        }
    }
}
