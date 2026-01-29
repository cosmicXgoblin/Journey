using UnityEngine;

public class TestUiManager : MonoBehaviour
{
    [SerializeField] private GameObject testFight;
    [SerializeField] private GameObject testChooseCharacter;

    public void OnClickGoFight()
    {
        testChooseCharacter.SetActive(false);
        testFight.SetActive(true);
    }

    public void OnClickGoChooseCharacter()
    {
        testChooseCharacter.SetActive(true);
        testFight.SetActive(false);
    }


}
