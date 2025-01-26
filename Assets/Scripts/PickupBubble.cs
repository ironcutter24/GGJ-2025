using UnityEngine;

public class PickupBubble : Bubble
{
    [SerializeField] private float maxAttractionForce = 10f;
    [SerializeField] private CircleCollider2D attractionTriggerCollider;
    [SerializeField] private AnimationCurve forceFalloff;

    protected override void OnPopComplete()
    {
        GameManager.Instance.ReloadCurrentLevel();
    }
    
    protected override void Start()
    {
        base.Start();
        
        var pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            var dir = collision.transform.position - transform.position;
            dir.z = 0f;
            var maxDistance = attractionTriggerCollider.radius;
            var normalizedDistance = (maxDistance - Mathf.Clamp(dir.magnitude, 0f, maxDistance)) / maxDistance;
            var falloff = forceFalloff.Evaluate(normalizedDistance);
            Rb.AddForce(dir.normalized * falloff * maxAttractionForce);
        }
    }
}
