using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField] private int levelSelect;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image levelImage;
    [SerializeField] private Button playButton;

    [SerializeField] private GameObject playGO, lockedGO;

    private void Start()
    {
        int curentLevel = UserDatas.user_Data.current_progress.level + 1;
        if (levelSelect <= curentLevel)
        {
            playGO?.SetActive(true);
            lockedGO?.SetActive(false);
        }
        else
        {
            playGO?.SetActive(false);
            lockedGO?.SetActive(true);
        }
        playButton.onClick.AddListener(OnPlayButton);
    }
    private void OnPlayButton()
    {
        Observer.Instance.Notify(ObserverKey.StartGame, levelSelect);
    }
}
