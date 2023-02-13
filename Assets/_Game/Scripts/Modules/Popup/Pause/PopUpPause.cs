using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[UIPanelPrefabAttr("Popup/Setting/PopupPause", "PriorityCanvas")]
public class PopUpPause : BasePanel
{
    [SerializeField]
    private ButtonCustom btClose;
    [SerializeField]
    private ButtonCustom btContinue;
    [SerializeField]
    private ButtonCustom btMainmenu;
    protected override void Awake()
    {
        base.Awake();
        SetBtEvents(btClose, HidePanel);
        SetBtEvents(btContinue, HidePanel);
        SetBtEvents(btMainmenu, ClickBackMainMenu);
    }
    private void ClickBackMainMenu()
    {
        PopUpNotice popUpNotice = PanelManager.Show<PopUpNotice>();
        popUpNotice.OnSetTextTwoButtonCustom("", "Back to Main menu ?", delegate {
            Ultis.ShowFade(true, delegate {
                AdsManager.instance.ShowInter(delegate
                {
                    ScenesManager.Instance.GetScene(AllSceneName.MainMenu, false);
                });
            });
        });
        HidePanel();
    }
    private void SetBtEvents(ButtonCustom bt, UnityAction action)
    {
        if (bt != null)
            bt.onClick = action;
    }
}
