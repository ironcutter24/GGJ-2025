using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLoader : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        
        InputManager.Actions.Player.AnyKey.performed += LoadGame;
    }

    private void OnDestroy()
    {
        InputManager.Actions.Player.AnyKey.performed -= LoadGame;
    }

    private static void LoadGame(InputAction.CallbackContext context)
    {
        GameManager.Instance.RestartGame();
    }
}
