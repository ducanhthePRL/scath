using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[UIPanelPrefabAttr("Popup/Tutorial/PopUpTutorial", "PriorityCanvas")]
public class PopupTutorial : BasePanel
{

    protected override void OnEnable()
    {
        base.OnEnable();
        AdsManager.instance.ShowBanner(false);
    }

    protected override void Start()
    {

    }
    public void PlayTutorial()
    {
        StopAllTutorial();
    }
    public void StopAllTutorial()
    {
       
    }
}
