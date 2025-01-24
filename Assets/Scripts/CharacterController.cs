using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask groundMask;
    
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        var pos = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(pos, Camera.MonoOrStereoscopicEye.Mono);
        
        if (Physics.Raycast(ray, out var hit, groundMask))
        {
            MoveAwayFrom(hit.point);
        }
    }

    private void MoveAwayFrom(Vector3 pos)
    {
        var dir = transform.position - pos;
        dir.y = 0f;
        transform.position += dir.normalized * (moveSpeed * Time.deltaTime);
    }
}
