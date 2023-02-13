using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonRun : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Animator _animator;
    private Material thisMaterial;
    private void Start()
    {
        thisMaterial = GetComponent<Image>().material;
        StartCoroutine(shineEffect());
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.Play("RunUi");
        Observer.Instance.Notify(ObserverKey.ButtonDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animator.Play("IdleUI");
        Observer.Instance.Notify(ObserverKey.ButtonUp);
    }
    private int _HsvShift = 0;
    private float _ShineLocation = 0;

    private float timeBetween2Shine = 2f; //second
    private float shineTime = 1f; //second
    private IEnumerator shineEffect()
    {
        while (true)
        {
            if (thisMaterial.HasInt("_HsvShift") && thisMaterial.HasFloat("_ShineLocation"))
            {
                if (_HsvShift >= 360 || _ShineLocation >= 1)
                {
                    _HsvShift = 0;
                    _ShineLocation = 0;
                    yield return new WaitForSeconds(timeBetween2Shine);
                }
                else
                {
                    _HsvShift++;
                    _ShineLocation += 1 / 360f;
                }
                thisMaterial.SetInt("_HsvShift", _HsvShift);
                thisMaterial.SetFloat("_ShineLocation", _ShineLocation);
            }
            yield return new WaitForSeconds(shineTime / 360f);
        }
    }
}
