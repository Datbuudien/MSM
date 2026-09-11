using UnityEngine;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSfx;

    private float nextPreviewTime;

    protected override void OnSetup()
    {
        if (sliderMusic != null) sliderMusic.onValueChanged.AddListener(SoundManager.Ins.SetMusicVolume);
        if (sliderSfx != null) sliderSfx.onValueChanged.AddListener(OnSfxChanged);
    }
    protected override void OnOpen()
    {
        if (sliderMusic != null) sliderMusic.SetValueWithoutNotify(SoundManager.Ins.MusicVolume);
        if (sliderSfx != null) sliderSfx.SetValueWithoutNotify(SoundManager.Ins.SfxVolume);
    }
    private void OnSfxChanged(float value)
    {
        SoundManager.Ins.SetSfxVolume(value);
        if (Time.unscaledTime < nextPreviewTime) return;
        nextPreviewTime = Time.unscaledTime + Constatnts.SOUND_PREVIEW_INTERVAL;
        SoundManager.Ins.PlaySfx(SfxType.ButtonClick);
    }
    public override void CloseDirectly()
    {
        SaveManager.Ins.Save();
        base.CloseDirectly();
    }
    public void OnClickClose()     => Close(0f);
    public override void BackKey() => OnClickClose();
}
