using UnityEngine;

public class Map_EnemyEvent : MonoBehaviour
{
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject gameManager;

    [Header("Database")]
    public Enemy enemy;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            uiManager.GetComponent<TestUiManager>().Fight();
            gameManager.GetComponent<TestFight>().OnClickStartFight(enemy);
        }
    }
}
