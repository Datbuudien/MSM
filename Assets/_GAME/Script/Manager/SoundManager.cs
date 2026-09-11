using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Nhac nen - keo DUNG THU TU BgmType")]
    [SerializeField] private AudioClip[] bgmClips;
    [Header("Tieng dong - keo DUNG THU TU SfxType")]
    [SerializeField] private AudioClip[] sfxClips;
    [Header("Tran am luong (can bang, khong phai thu nguoi choi chinh)")]
    [SerializeField, Range(0f, 1f)] private float bgmMaxVolume = .4f;
    [SerializeField, Range(0f, 1f)] private float sfxMaxVolume = 1f;

    private BgmType currentBgm = BgmType.None;

    public float MusicVolume => SaveManager.Ins.Data.MusicVolume;
    public float SfxVolume => SaveManager.Ins.Data.SfxVolume;

    protected override void Awake()
    {
        base.Awake();
        if (IsDuplicate) return;
        if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        ApplyVolume();
    }
    public void PlayBgm(BgmType type)
    {
        if (currentBgm == type) return;
        currentBgm = type;
        AudioClip clip = GetClip(bgmClips, (int)type);
        if (clip == null)
        {
            bgmSource.Stop();
            return;
        }
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    public void StopBgm()
    {
        currentBgm = BgmType.None;
        bgmSource.Stop();
    }

    public void PlaySfx(SfxType type) => PlaySfx(type, 1f);
    public void PlaySfx(SfxType type, float volumeScale)
    {
        float volume = SfxVolume * sfxMaxVolume * volumeScale;
        if (volume <= 0f) return;
        AudioClip clip = GetClip(sfxClips, (int)type);
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMusicVolume(float value)
    {
        SaveManager.Ins.Data.MusicVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }
    public void SetSfxVolume(float value)
    {
        SaveManager.Ins.Data.SfxVolume = Mathf.Clamp01(value);
    }
    private void ApplyVolume()
    {
        bgmSource.volume = MusicVolume * bgmMaxVolume;
    }
    private static AudioClip GetClip(AudioClip[] clips, int index)
    {
        if (clips == null || index < 0 || index >= clips.Length) return null;
        return clips[index];
    }
}
