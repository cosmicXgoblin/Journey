using UnityEngine;
using UnityEngine.VFX;

public class Map_EnemyEvent : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject gameManager;

    [Header("Database")]
    public Enemy enemy;

    [SerializeField] private bool _fought = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!_fought)
        {
            if (col.gameObject.tag == "Player")
            {
                uiManager.GetComponent<TestUiManager>().Fight();
                gameManager.GetComponent<GameManager>().InitStartFight(enemy);
                //Destroy(this.gameObject);

                _fought = true;
            }
        }

    }

    public void LoadData(GameData data)
    {
        data.enemiesFought.TryGetValue(id, out _fought);
        if (_fought)
        {
            // visual.gameObject.SetActive(false);          // that's what they used in the tut, but ig visual is a reference
            gameObject.SetActive(false);                    // alternative, idk
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.enemiesFought.ContainsKey(id))
        {
            data.enemiesFought.Remove(id);
        }
        data.enemiesFought.Add(id, _fought);
    }
}
