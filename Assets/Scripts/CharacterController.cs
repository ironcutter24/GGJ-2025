using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CharacterController : Bubble
{
    [SerializeField] private float velocityPopThreshold = 7.5f;
    [SerializeField] private float radiusIncrement = 0.5f;
    [SerializeField] private float massIncrement = 0.1f;
    [SerializeField] private float maxPushForce = 10f;
    [SerializeField] private float pushRange = 5f;
    [SerializeField] private AnimationCurve forceFalloff;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform modelChildTransform;

    private Camera _mainCamera;
    private Rigidbody2D _rb;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        // var inputActions = InputManager.Actions;
    }

    protected override void OnPopComplete()
    {
        GameManager.Instance.ReloadCurrentLevel();
    }

    private void FixedUpdate()
    {
        if (HasBlowInput(out var hit))
        {
            ApplyForce(hit.point);
        }
        //velocity = _rb.linearVelocity.magnitude;
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
        dir.z = 0f;
        var normalizedDistance = (pushRange - Mathf.Clamp(dir.magnitude, 0f, pushRange))/pushRange;
        var falloff = forceFalloff.Evaluate(normalizedDistance);
        _rb.AddForce(dir.normalized * (falloff * maxPushForce));
    }

    private void Grow()
    {
        modelChildTransform.DOScale(transform.localScale + radiusIncrement * Vector3.one, .4f).SetEase(Ease.OutBounce);
        // modelChildTransform.localScale += radiusIncrement * Vector3.one;
        _rb.mass += massIncrement;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Bubble") && collision.gameObject.layer == 10)
        {
            Grow();
            Destroy(collision.attachedRigidbody.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0 && collision.relativeVelocity.magnitude >= velocityPopThreshold)
        {
            Pop();
        }
    }
}
