using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Observer.Instance.Notify(ObserverKey.CollideNonStar);
        }
    }
}
