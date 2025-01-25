using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask groundMask;

    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        
        // var inputActions = InputManager.Actions;
    }

    private void FixedUpdate()
    {
        if (HasBlowInput(out var hit))
        {
            MoveAwayFrom(hit.point);
        }
    }

    private bool HasBlowInput(out RaycastHit hit)
    {
        if (InputManager.Actions.Player.Blow.IsPressed())
        {
            var pos = Mouse.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(pos, Camera.MonoOrStereoscopicEye.Mono);
            return Physics.Raycast(ray, out hit, groundMask);
        }
        else
        {
            hit = default;
            return false;
        }
    }

    private void MoveAwayFrom(Vector3 pos)
    {
        var dir = transform.position - pos;
        dir.z = 0f;
        transform.position += dir.normalized * (moveSpeed * Time.deltaTime);
    }
}
