using UnityEngine;
using UnityEngine.Pool;

public class SFXController : MonoBehaviour, IPoolable<SFXController>
{
    [Header("References")]
    [SerializeField] private AudioSource _audioSource;
    public IObjectPool<SFXController> Pool { get; set; }

    public void Play(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
        Invoke(nameof(Return), audioClip.length);
    }

    private void Return()
    {
        ReturnToPool();
    }

    public void ReturnToPool(float delay = 0)
    {
        Pool.Release(this);
    }
}
