using UnityEngine;
using Utilities;

public class AudioManager : Singleton<AudioManager>
{
    private FMOD.Studio.EventInstance _bubbleBounceInstance;

    [SerializeField] private FMODUnity.EventReference bubbleBounceEvent;
    [SerializeField] private FMODUnity.EventReference bubbleMergeEvent;
    [SerializeField] private FMODUnity.EventReference bubblePopEvent;

    private void Start()
    {
        _bubbleBounceInstance = FMODUnity.RuntimeManager.CreateInstance(bubbleBounceEvent);
    }

    public void PlayBubbleMerge() => PlaySound(bubbleMergeEvent);
    
    public void PlayBubblePop() => PlaySound(bubblePopEvent);

    public void PlayBubbleBounce(float pitch)
    {
        pitch = Mathf.Clamp01(pitch);
        _bubbleBounceInstance.setParameterByName("BouncePitch", pitch);
        _bubbleBounceInstance.start();
    }

    private static void PlaySound(FMODUnity.EventReference eventRef)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventRef);
    }
}
