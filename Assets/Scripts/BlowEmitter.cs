using UnityEngine;

public class BlowEmitter : MonoBehaviour
{
    [SerializeField] private Transform meshRoot;
    [SerializeField] private ParticleSystem partSystem;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
    }
    
    public void SetDistance(float distance, float maxDistance)
    {
        if (distance > maxDistance)
            distance = maxDistance;
        
        var scale = meshRoot.transform.localScale;
        scale.z = distance * .5f;
        meshRoot.transform.localScale = scale;

        var time = distance / partSystem.main.startSpeedMultiplier;
        var main = partSystem.main;
        main.startLifetime = time;
    }
}
