using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, 
                            IBeginDragHandler, 
                            IDragHandler, 
                            IEndDragHandler
{
    private RectTransform rectTransform; // 드래그할 객체의 RectTransform
    private CanvasGroup canvasGroup; // UI의 투명도를 조정할 수 있는 CanvasGroup

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // 드래그 중에는 투명도를 조정
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 다른 UI 요소와의 상호작용을 차단
    }

    // 드래그 중에 호출
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // 마우스 위치에 따라 위치 변경
    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // 드래그가 끝나면 투명도 복원
        canvasGroup.blocksRaycasts = true; // 다른 UI 요소와의 상호작용 복원
    }
}
