using DG.Tweening;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    private const float DissolveTime = 0.2f;
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    private bool _wasPopped;
    private Material _rendMat;

    [SerializeField] private ParticleSystem popParticles;

    private void Start()
    {
        var rend = GetComponent<MeshRenderer>();
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
        popParticles?.Play();
    }
}
