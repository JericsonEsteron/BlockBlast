using UnityEngine;
using UnityEngine.Pool;

public class AudioController : Singleton<AudioController>
{
    [Header("References")]
    [SerializeField] private SFXController _sfxController;

    private IObjectPool<SFXController> _pool;

    protected override void Awake()
    {
        base.Awake();
        InitializeSFXPool();
    }

    private void InitializeSFXPool()
    {
        _pool = new ObjectPoolBuilder<SFXController>(_sfxController).WithInitialSize(5).WithMaxSize(10).Build();
    }

    public void PlaySFX(AudioClip clip)
    {
        var sfx = _pool.Get();
        sfx.transform.position = Vector3.zero;
        sfx.Play(clip);
    }
}
