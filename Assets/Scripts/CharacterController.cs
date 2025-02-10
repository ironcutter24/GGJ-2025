using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using DG.Tweening;
using UnityEngine.Serialization;

public class CharacterController : Bubble
{
    private Camera _mainCamera;
    
    [SerializeField, Min(0f)] private float velocityPopThreshold = 7.5f;
    [SerializeField, Min(0f)] private float velocityBumpThreshold = 0.5f;
    [SerializeField, Min(0f)] private float radiusIncrement = 0.5f;
    [SerializeField, Min(0f)] private float massIncrement = 0.1f;
    [SerializeField, Min(0f)] private float maxPushForce = 10f;
    [SerializeField, Min(0f)] private float pushRange = 5f;
    [SerializeField, Min(0f)] private float pickupTriggerRadiusOffset = .2f;
    [SerializeField] private AnimationCurve forceFalloff;
    [SerializeField] private LayerMask groundMask;
    [Space]
    [SerializeField] private Transform modelRootTrs;
    [SerializeField] private Transform modelTrs;
    [SerializeField] private CircleCollider2D pickupTrigger;
    [SerializeField] private BlowEmitter blowEmitter;
    
    private float Radius => modelTrs.localScale.x * .5f;

    public event System.Action Merged;
    
    
    protected override void Start()
    {
        base.Start();
        
        _mainCamera = Camera.main;
        // var inputActions = InputManager.Actions;
        
        pickupTrigger.radius = Radius - pickupTriggerRadiusOffset;
        blowEmitter.gameObject.SetActive(false);
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
            UpdateBlowVFX(hit.point);
        }
        else
        {
            blowEmitter.gameObject.SetActive(false);
        }
    }

    private void UpdateBlowVFX(Vector3 point)
    {
        blowEmitter.gameObject.SetActive(true);
        blowEmitter.SetPosition(point);

        var dist = (point - transform.position).magnitude;
        blowEmitter.SetDistance(dist - Radius, pushRange - Radius);
        blowEmitter.LookAt(transform.position);
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
        Rb.AddForce(dir.normalized * (falloff * maxPushForce));
    }

    private void Grow()
    {
        modelTrs.DOScale(modelTrs.localScale + radiusIncrement * Vector3.one, .4f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => pickupTrigger.radius = Radius - pickupTriggerRadiusOffset);
        Rb.mass += massIncrement;
        AudioManager.Instance.PlayBubbleMerge();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.attachedRigidbody.CompareTag("Bubble"))
        {
            Grow();
            Destroy(collision.attachedRigidbody.gameObject);
            Merged?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            if (collision.relativeVelocity.magnitude >= velocityPopThreshold)
            {
                Pop();
            }
            else if(collision.relativeVelocity.magnitude > velocityBumpThreshold)
            {
                Bump(collision.relativeVelocity.magnitude);
            }
        }
    }

    private void EvaluateSolidCollision()
    {
        
    }

    private void Bump(float velocityMagnitude)
    {
        var impactVelocityRemapped = math.remap(velocityBumpThreshold, velocityPopThreshold, 0f, 1f, velocityMagnitude);
        var bumpDelta = impactVelocityRemapped * 0.15f + 0.1f;
        var bumpSquash = 1f - bumpDelta;
        var bumpStretch = 1f + bumpDelta;
        var bumpTime = 0.1f + 0.2f * impactVelocityRemapped;
        var bump = DOTween.Sequence();
        bump.Append(modelRootTrs.DOScale(new Vector3(bumpSquash, bumpStretch, bumpSquash), bumpTime))
            .Append(modelRootTrs.DOScale(new Vector3(bumpStretch, bumpSquash, bumpStretch), bumpTime))
            .Append(modelRootTrs.DOScale(Vector3.one, bumpTime));
        
        AudioManager.Instance.PlayBubbleBounce(1 - impactVelocityRemapped);
    }
}
