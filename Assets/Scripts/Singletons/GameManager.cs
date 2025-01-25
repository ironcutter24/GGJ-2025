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
    }

    public void LoadNextLevel()
    {
        var nextIndex = sceneList
            .Where(t => t == SceneManager.GetActiveScene().name)
            .Select((v, i) => i + 1).First();
        
        var newIndex = SceneManager.GetActiveScene();
        if (nextIndex < sceneList.Length)
        {
            sceneIndex = nextIndex;
            //var nextScene = scene
            //SceneManager.LoadScene(nextScene.name, LoadSceneMode.Single);   
        }
    }
}
