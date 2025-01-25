using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxPushForce = 10f;
    [SerializeField] private float pushRange = 5f;
    [SerializeField] private AnimationCurve forceFalloff;
    [SerializeField] private LayerMask groundMask;

    private Camera _mainCamera;
    private Rigidbody2D rb;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        // var inputActions = InputManager.Actions;
    }

    private void FixedUpdate()
    {
        if (HasBlowInput(out var hit))
        {
            ApplyForce(hit.point);
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

    private void ApplyForce(Vector3 from)
    {
        var dir = transform.position - from;
        var pushForce = maxPushForce;
        dir.z = 0f;
        var normalizedDistance = (pushRange - Mathf.Clamp(dir.magnitude, 0f, pushRange))/pushRange;
        var falloff = forceFalloff.Evaluate(normalizedDistance);
        rb.AddForce(dir.normalized * falloff * maxPushForce);
    }

    /*
    private void MoveAwayFrom(Vector3 pos)
    {
        var dir = transform.position - pos;
        dir.z = 0f;
        transform.position += dir.normalized * (moveSpeed * Time.deltaTime);
    }
    */
}
