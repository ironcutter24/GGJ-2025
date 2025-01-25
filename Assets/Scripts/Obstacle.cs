using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag is "Player" or "Bubble")
        {
            other.gameObject.GetComponent<Bubble>().Pop();
        }
    }
}
