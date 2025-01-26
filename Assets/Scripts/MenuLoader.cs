using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.LoadMainMenu();
    }
    
}
