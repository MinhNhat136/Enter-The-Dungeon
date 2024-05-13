using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SelfPoolRelease : MonoBehaviour
{
    public float timeDelay;

    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(timeDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Release<T>(ObjectPool<T> pool, T objectRelease) where T : Component
    {
        StartCoroutine(ReleaseCoroutine(pool, objectRelease));
    }
    
    private IEnumerator ReleaseCoroutine<T>(ObjectPool<T> pool, T objectRelease) where T : Component
    {
        yield return _waitForSeconds;
        objectRelease.transform.position = Vector3.zero;
        pool.Release(objectRelease);
    }
}
