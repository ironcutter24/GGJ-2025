using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameManager : Singleton<GameManager>
{
    [SerializeField, Scene] private string menuScene;
    [Space]
    [SerializeField, Min(0)] private int sceneIndex;
    [SerializeField, Scene] private string[] sceneList;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        InputManager.Actions.Player.QuitGame.performed += _ => Application.Quit();
    }

    public void LoadNextLevel()
    {
        var nextIndex = sceneList
            .Where(t => t == SceneManager.GetActiveScene().name)
            .Select((_, i) => i + 1).First();
        
        if (nextIndex < sceneList.Length)
        {
            sceneIndex = nextIndex;
            LoadLevel(sceneIndex);
        }
    }

    public void ReloadCurrentLevel()
    {
        // TODO: Show game over UI
        
        LoadLevel(sceneIndex);
    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadScene(sceneList[index], LoadSceneMode.Single);
    }
}
