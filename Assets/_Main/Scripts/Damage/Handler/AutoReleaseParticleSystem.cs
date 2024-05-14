using System;
using UnityEngine;
using UnityEngine.Pool;

public class AutoReleaseParticleSystem : MonoBehaviour
{
    public ObjectPool<ParticleSystem> myPool;
    public float delayReleaseTime;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void AutoRelease()
    {
        Invoke(nameof(Release), delayReleaseTime);  
    } 
    
    private void Release()
    {
        myPool.Release(_particleSystem);
    }
}
