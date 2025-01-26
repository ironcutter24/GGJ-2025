using System;
using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private int _numberOfBubbles;
    private CharacterController _player;
    
    private IEnumerator Start()
    {
        _numberOfBubbles = GameObject.FindGameObjectsWithTag("Bubble").Length;
        
        _player = GameObject
            .FindWithTag("Player")
            .GetComponent<CharacterController>();

        _player.Merged += OnBubbleMerged;
        
        // Level start logic
        
        yield break;
    }

    private void OnDestroy()
    {
        _player.Merged -= OnBubbleMerged;
    }

    private void OnBubbleMerged()
    {
        _numberOfBubbles--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody?.CompareTag("Player") ?? false)
        {
            Debug.Log("Player entered LevelExit");

            if (_numberOfBubbles <= 0)
            {
                Debug.Log("Loading next level");
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}
