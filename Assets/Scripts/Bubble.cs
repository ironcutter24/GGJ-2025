using DG.Tweening;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    private const float DissolveTime = .3f;
    private const float PopupTime = 1.5f;
    private static readonly int DissolveAmount = Shader.PropertyToID("_alphaClipThreshold");
    
    private bool _wasPopped;
    private Material _rendMat;
    protected Rigidbody2D Rb;

    [SerializeField] private ParticleSystem popParticles;

    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        
        var rend = GetComponentInChildren<MeshRenderer>();
        if (rend)
        {
            rend.sharedMaterial = new Material(rend.sharedMaterial);
            _rendMat = rend.sharedMaterial;
        }
    }

    public void Pop()
    {
        if (_wasPopped) return;
        _wasPopped = true;
        
        Rb.linearVelocity = Vector2.zero;
        
        var popSequence = DOTween.Sequence();
        popSequence
            .Append(DOTween.To(() => 0f, SetDissolveAmount, 1f, DissolveTime))
            .SetEase(Ease.InQuad)
            .InsertCallback(0f, SpawnPopParticles)
            .OnComplete(OnPopComplete);
        
        AudioManager.Instance.PlayBubblePop();
    }
    
    protected abstract void OnPopComplete();

    private void SetDissolveAmount(float amount)
    {
        _rendMat?.SetFloat(DissolveAmount, amount);
    }
    
    private void SpawnPopParticles()
    {
        Instantiate(popParticles, transform.position, Quaternion.identity);
    }

    private void BubbleSpawnAnimation()
    {
        transform.localScale = Vector3.one * 0.01f;
        transform.DOScale(Vector3.one, PopupTime + Random.Range(0f, 1f)).SetEase(Ease.OutElastic);
    }
}
