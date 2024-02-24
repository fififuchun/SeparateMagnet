using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CustomButton : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public System.Action onClickCallback;

    [SerializeField] private CanvasGroup _canvasGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
        // Debug.Log("Click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);
        // Debug.Log("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
        // Debug.Log("Up");
    }
}
