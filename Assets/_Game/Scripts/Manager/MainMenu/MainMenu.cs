using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text textDiamond;
    [SerializeField] private TMP_Text textEnergy;

    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.StartGame, ClickPlay);
        SetDiamond(UserDatas.user_Data.currency.diamond);
        SetEnergy(UserDatas.user_Data.currency.energy);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.StartGame, ClickPlay);
    }
    public void ClickPlay(object data)
    {
        LevelSelect.level = (int)data;
        Ultis.ShowFade(true, LoadSceneGame);
    }

    private void LoadSceneGame()
    {
        ScenesManager.Instance.GetScene(AllSceneName.GamePlayMain, false);
        ScenesManager.Instance.GetScene(AllSceneName.GamePlayCanvas, true);
        Ultis.ShowFade(false, null);
    }
    public void SetDiamond(int diamond)
    {
        textDiamond.text = $"{diamond}";
    }
    public void SetEnergy(int energy)
    {
        textEnergy.text = $"{energy}";
    }
}
