using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.Landing,PlayLandingEffect);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.Landing, PlayLandingEffect);
    }
    private void PlayLandingEffect(object data)
    {
        Vector3 position = (Vector3)data;
        particleSystem.transform.position = position;
        particleSystem?.Play();
    }
    void Update()
    {
        
    }
}
