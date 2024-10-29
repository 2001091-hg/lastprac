using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public class ChapterImageManager : MonoBehaviour
{
    public Dropdown chapterDropdown; // 챕터 드롭다운 UI
    public Image chapterImage; // 이미지를 표시할 UI Image
    public Sprite defaultSprite; // 기본 이미지 스프라이트 (챕터 선택 전 표시될 기본 이미지)

    // MySQL 연결 문자열
    private string connectionString = "Server=giftnet.co.kr;Database=kustaf;User ID=kustaf;Password=pinics02!;";
    
    void Start()
    {
        LoadChapterData(); // 드롭다운에 챕터 목록을 로드
        chapterDropdown.onValueChanged.AddListener(OnChapterSelected); // 드롭다운 변경 시 호출될 메서드 추가
        chapterImage.sprite = defaultSprite; // 기본 이미지로 설정
    }

    // 챕터 데이터를 로드하여 드롭다운에 추가
    private void LoadChapterData()
    {
        List<string> chapterNames = new List<string>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT CHAPTER_NAME FROM T_CATALOG_CHAPTER WHERE CATALOG_ID = @catalogId"; 
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // 카탈로그 ID는 필요한 경우 파라미터로 전달되거나, 선택된 카탈로그 ID를 바인딩해야 합니다.
                    command.Parameters.AddWithValue("@catalogId", 1); // 예시로 카탈로그 ID 1번을 사용
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string chapterName = reader.GetString("CHAPTER_NAME");
                            chapterNames.Add(chapterName);
                        }
                    }
                }

                chapterDropdown.AddOptions(chapterNames); // 드롭다운 옵션 추가
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
        }
    }

    // 드롭다운에서 챕터 선택 시 호출되는 메서드
    private void OnChapterSelected(int index)
    {
        if (index >= 0)
        {
            // 여기에 SQL에서 해당 챕터와 연결된 이미지를 로드하는 코드를 추가
            // 현재는 기본 동작으로 UI에 이미지를 업데이트하는 예시만 구현

            // 실제로는 SQL 쿼리로 이미지를 가져오거나 경로를 가져와 로드해야 함
            // 이미지 경로를 가져왔다고 가정하고 로드
            LoadChapterImage(index); // 인덱스에 맞는 이미지를 로드
        }
    }

    // 챕터에 맞는 이미지를 로드 (현재는 기본 이미지만 교체하는 로직)
    private void LoadChapterImage(int chapterIndex)
    {
        // 예를 들어 chapterIndex에 맞는 이미지를 변경
        // 현재는 단순히 기본 이미지를 표시하는 예제

        // 기본 예시로 이미지를 임의로 교체
        chapterImage.sprite = defaultSprite; // 선택된 챕터에 맞는 스프라이트로 설정

        // 실제로는 데이터베이스에서 이미지 경로나 데이터를 가져와 동적으로 이미지를 설정해야 함
    }
}
