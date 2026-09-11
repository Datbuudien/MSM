using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSfx : MonoBehaviour
{
    [SerializeField] private SfxType type = SfxType.ButtonClick;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }
    private void Play() => SoundManager.Ins.PlaySfx(type);
}
