using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameManager : Singleton<GameManager>
{
    [SerializeField, Scene] private string menuScene;
    [SerializeField, Scene] private string environmentScene;
    [Space]
    [SerializeField, Min(0)] private int levelIndex;
    [SerializeField, Scene] private string[] levelList;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        InputManager.Actions.Player.RestartLevel.performed += _ => LoadLevel(levelIndex);
        InputManager.Actions.Player.QuitGame.performed += _ => Application.Quit();
    }

    public void RestartGame()
    {
        levelIndex = 0;
        ReloadCurrentLevel();
    }
    
    public void LoadNextLevel()
    {
        var nextIndex = levelList
            .Where(t => t == SceneManager.GetSceneAt(1).name)
            .Select((_, i) => i + 1).First();
        
        if (nextIndex < levelList.Length)
        {
            levelIndex = nextIndex;
            LoadLevel(levelIndex);
        }
    }

    public void ReloadCurrentLevel()
    {
        // TODO: Show game over UI
        
        LoadLevel(levelIndex);
    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadScene(environmentScene, LoadSceneMode.Single);
        SceneManager.LoadScene(levelList[index], LoadSceneMode.Additive);
    }
}
