using UnityEngine;
using Utilities;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
