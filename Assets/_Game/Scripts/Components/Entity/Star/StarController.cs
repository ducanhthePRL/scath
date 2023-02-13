using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private Star star;
    private List<NonStar> listNonstar;
    private int currentNonstar = 0;
    void Start()
    {
        star = GetComponentInChildren<Star>();
        star.gameObject.SetActive(false);
        listNonstar = new List<NonStar>();
        for (int i = 0; i < transform.childCount; i++)
        {
            NonStar nonStar = transform.GetChild(i).GetComponent<NonStar>();
            if (nonStar != null)
            {
                listNonstar.Add(nonStar);
                nonStar.gameObject.SetActive(false);
            }
        }
        Observer.Instance.AddObserver(ObserverKey.CollideNonStar, ActionCollide);
        if (listNonstar.Count > 0)
        {
            listNonstar[currentNonstar].gameObject.SetActive(true);
        }
        else
        {
            star.gameObject.SetActive(true);
        }
    }
    private void ActionCollide(object data)
    {
        listNonstar[currentNonstar].gameObject.SetActive(false);
        currentNonstar++;
        if (currentNonstar >= listNonstar.Count)
        {
            star.gameObject.SetActive(true);
        }
        else
        {
            listNonstar[currentNonstar].gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.CollideNonStar, ActionCollide);
    }
}
