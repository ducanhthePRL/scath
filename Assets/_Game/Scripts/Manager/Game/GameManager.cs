using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField] private List<GameObject> levelList;
    private int currentLevel;
    private GameObject currentLevelGO;
    private void Awake()
    {
       
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.PassLevel, PassLevel);
    }

    void Start()
    {
        UserDatas.current_scene_type = SceneType.Gameplay;
        AdsManager.instance.ShowBanner(false);
        StartCoroutine(IEInit(()=> {
            if (LevelSelect.level > 0)
            {
                currentLevel = LevelSelect.level - 1;
            }
            else
            {
                currentLevel = UserDatas.user_Data.current_progress.level;
            }
            CanvasManager.instance.SetTextLevel(currentLevel + 1);
            if (levelList?.Count != 0)
            {
                currentLevelGO = Instantiate(levelList[currentLevel]);
            }
            else
            {
                Debug.LogError("levelList null");
            }
        }));
        Observer.Instance.AddObserver(ObserverKey.PassLevel, PassLevel);
    }
    private void PassLevel(object data)
    {
        Ultis.ShowFade(true, delegate {
            LoadNextLevel();
        });
    }
    private void LoadNextLevel()
    {
        Destroy(currentLevelGO);
        if (currentLevel == UserDatas.user_Data.current_progress.level) {
            UserDatas.user_Data.current_progress.level++;
            UserDatas.Save();
        }
        currentLevel++;
        CanvasManager.instance.SetTextLevel(currentLevel + 1);
        if (currentLevel < levelList.Count)
        {
            currentLevelGO = Instantiate(levelList[currentLevel]);
        }
        else
        {
            ScenesManager.Instance.GetScene(AllSceneName.MainMenu, false);
        }
        Ultis.ShowFade(false, null);
    }
    private IEnumerator IEInit(UnityAction action)
    {
        yield return new WaitUntil(() => CanvasManager.instance != null);
        action?.Invoke();
    }
}