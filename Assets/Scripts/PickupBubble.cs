using UnityEngine;

public class PickupBubble : Bubble
{
    private Rigidbody2D _rb;
    
    [SerializeField] private float maxAttractionForce = 10f;
    [SerializeField] private CircleCollider2D attractionTriggerCollider;
    [SerializeField] private AnimationCurve forceFalloff;

    protected override void OnPopComplete()
    {
        Destroy(gameObject);
    }
    
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        
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
            _rb.AddForce(dir.normalized * falloff * maxAttractionForce);
        }
    }
}
