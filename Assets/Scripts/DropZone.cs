using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    public GameObject imagePrefab; // 드래그한 이미지가 없을 때 기본으로 추가할 Prefab

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedImage = eventData.pointerDrag; // 드래그한 이미지 가져오기

        if (droppedImage != null)
        {
            // 이미지가 Grid에 포함되지 않은 경우 복제하여 추가
            if (!droppedImage.transform.IsChildOf(transform))
            {
                GameObject newImage = Instantiate(imagePrefab, transform); // 새 이미지 생성
                newImage.GetComponent<Image>().sprite = droppedImage.GetComponent<Image>().sprite;
            }
            else
            {
                droppedImage.transform.SetParent(transform); // 기존 이미지 위치 이동
            }
        }
    }
}
