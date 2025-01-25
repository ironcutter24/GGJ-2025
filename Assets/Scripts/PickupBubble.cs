using UnityEngine;

public class PickupBubble : Bubble
{
    [SerializeField] private float maxAttractionForce = 10f;
    [SerializeField] private CircleCollider2D attractionTriggerCollider;
    [SerializeField] private AnimationCurve forceFalloff;
    private Rigidbody2D _rb;
    
    protected override void OnPopComplete()
    {
        // throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
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
