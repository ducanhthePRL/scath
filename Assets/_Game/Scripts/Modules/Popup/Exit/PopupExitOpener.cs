using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupExitOpener : BaseOpenPanel
{
    protected override void Open()
    {
        PopUpNotice popUpNotice = PanelManager.Show<PopUpNotice>();
        popUpNotice.OnSetTextTwoButtonCustom("", "Back to Main menu ?", delegate {
            Ultis.ShowFade(true, delegate {
                AdsManager.instance.ShowInter(delegate
                {
                    Ultis.ShowFade(false, null);
                    ScenesManager.Instance.GetScene(AllSceneName.MainMenu, false);
                });
            });
        });
    }
}
