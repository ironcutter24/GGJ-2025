using DG.Tweening;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    private const float DissolveTime = .2f;
    private static readonly int DissolveAmount = Shader.PropertyToID("_alphaClipThreshold");
    
    private bool _wasPopped;
    private Material _rendMat;

    [SerializeField] private ParticleSystem popParticles;

    protected virtual void Start()
    {
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
        AudioManager.Instance.PlayBubblePop();
        var popSequence = DOTween.Sequence();
        popSequence
            .Append(DOTween.To(() => 0f, SetDissolveAmount, 1f, DissolveTime))
            .SetEase(Ease.InQuad)
            .InsertCallback(0f, SpawnPopParticles)
            .OnComplete(OnPopComplete);
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
}
