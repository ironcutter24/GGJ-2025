using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameManager : Singleton<GameManager>
{
    [SerializeField, Scene] private string menuScene;
    [SerializeField, Scene] private string environmentScene;
    [Space]
    [SerializeField, ReadOnly] private int levelIndex;
    [SerializeField, Scene] private string[] levelList;
    
    private void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        
            levelIndex = 0;
        
            InputManager.Actions.Player.RestartLevel.performed += _ => LoadLevel(levelIndex);
            InputManager.Actions.Player.QuitGame.performed += _ => Application.Quit();
        }
    }

    public void RestartGame()
    {
        levelIndex = 0;
        ReloadCurrentLevel();
    }

    public void LoadMainMenu()
    {
        LoadLevel(menuScene);
    }
    
    public void LoadNextLevel()
    {
        var currentLevelName = SceneManager.GetSceneAt(1).name;
        Debug.Log($"Current: {currentLevelName}");

        var nextIndex = Array.IndexOf(levelList, currentLevelName) + 1;
        if (nextIndex < levelList.Length)
        {
            levelIndex = nextIndex;
            LoadLevel(levelIndex);
        }
        else
        {
            Debug.LogWarning("Level is out of range");
        }
    }

    public void ReloadCurrentLevel()
    {
        // TODO: Show game over UI
        
        LoadLevel(levelIndex);
    }

    private void LoadLevel(int index)
    {
        LoadLevel(levelList[index]);
    }

    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(environmentScene, LoadSceneMode.Single);
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }
}
