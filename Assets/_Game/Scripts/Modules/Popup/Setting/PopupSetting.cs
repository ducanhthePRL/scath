using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[UIPanelPrefabAttr("Popup/Setting/PopupSetting", "PriorityCanvas")]
public class PopupSetting : BasePanel
{
    [SerializeField]
    private ButtonCustom btMusicOn, btMusicOff, btFxOn, btFxOff;
    [SerializeField]
    private ButtonCustom btClose;
    [SerializeField]
    private GameObject obMusicOn, obMusicOff;
    [SerializeField]
    private GameObject obFxOn, obFxOff;

    protected override void Awake()
    {
        base.Awake();
        SetBtEvents(btMusicOn, ClickMusic);
        SetBtEvents(btMusicOff, ClickMusic);
        SetBtEvents(btFxOn, ClickFx);
        SetBtEvents(btFxOff, ClickFx);
        SetBtEvents(btClose, HidePanel);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bool music_mute = TPRLSoundManager.Instance.isMusicMute;
        SetActive(obMusicOn, !music_mute);
        SetActive(obMusicOff, music_mute);
        bool fx_mute = TPRLSoundManager.Instance.isFxMuted;
        SetActive(obFxOn, !fx_mute);
        SetActive(obFxOff, fx_mute);
        bool theme_open = ObscuredPrefs.Get<bool>("ThemeOpen", false);
    }

    private void SetBtEvents(ButtonCustom bt, UnityAction action)
    {
        if (bt != null)
            bt.onClick = action;
    }

    private void ClickMusic()
    {
        bool active = obMusicOn.activeSelf;
        active = !active;
        SetActive(obMusicOn, active);
        SetActive(obMusicOff, !active);
        TPRLSoundManager.Instance.SetMusicMute(!active);
    }

    private void ClickFx()
    {
        bool active = obFxOn.activeSelf;
        active = !active;
        SetActive(obFxOn, active);
        SetActive(obFxOff, !active);
        TPRLSoundManager.Instance.SetSoundMute(!active);
    }

    private void SetActive(GameObject ob, bool active)
    {
        if (ob != null)
            ob.SetActive(active);
    }
 }
