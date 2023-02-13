using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class CanvasManager : SingletonMono<CanvasManager>
{
    [SerializeField]
    private Camera _canvasCamera;
    public Camera canvasCamera => _canvasCamera;

    [SerializeField] private TMP_Text textLevel;
    [SerializeField] private TMP_Text textDiamond;
    [SerializeField] private TMP_Text textEnergy;
    [SerializeField] private TMP_Text textDarkEnergy;

    
    protected override void Awake()
    {
        base.Awake();
        EnvironmentType environment = EnvironmentConfig.currentEnvironmentEnum;

        SetDiamond(UserDatas.user_Data.currency.diamond);
        SetEnergy(UserDatas.user_Data.currency.energy);
    }

    public void SetDiamond(int diamond)
    {
        textDiamond.text = $"{diamond}";
    }

    public void SetEnergy(int energy)
    {
        textEnergy.text = $"{energy}";
    }

    public void SetTextLevel(int level)
    {
        textLevel.text = $"Level {level}";
    }

    protected override void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
}
