using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private BoxCollider2D boxCollider2D
    {
        get
        {
            if (_boxCollider2D == null) _boxCollider2D = GetComponent<BoxCollider2D>();
            return _boxCollider2D;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Observer.Instance.Notify(ObserverKey.CollideTrap);
    }
    public void ActiveBoxCollider()
    {
        boxCollider2D.enabled = true;
    }
    public void DeactiveBoxCollider()
    {
        boxCollider2D.enabled = false;
    } 
}
