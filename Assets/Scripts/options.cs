using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel; // 패널 오브젝트
    public Button optionsButton; // 옵션 버튼
    public Button closeButton; // 닫기 버튼

    void Start()
    {
        // 패널을 처음에 비활성화
        optionsPanel.SetActive(false);
        
        // 버튼 클릭 이벤트 추가
        optionsButton.onClick.AddListener(TogglePanel);
        closeButton.onClick.AddListener(HidePanel);
    }

    // 패널을 토글하는 메서드
    void TogglePanel()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    // 패널을 숨기는 메서드
    void HidePanel()
    {
        optionsPanel.SetActive(false);
    }
}
