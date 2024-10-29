using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MySQLManager : MonoBehaviour
{
    // MySQL 연결 문자열
    private string connectionString = "Server=giftnet.co.kr;Database=kustaf;User ID=kustaf;Password=pinics02!;";
    public InputField catalogNameInput; // 카탈로그 이름 입력 필드
    public InputField catalogIdInput; // 삭제할 카탈로그 ID 입력 필드
    public Dropdown catalogDropdown; // 카탈로그 드롭다운 UI
    public Dropdown chapterDropdown; // 챕터 드롭다운 UI

    public Dropdown pageDropdown; // 페이지 드롭다운 UI 추가

    public GameObject productPanel; // 상품 패널

    string regDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    // Start에서 카탈로그 데이터를 로드합니다.
    void Start()
    {
        productPanel.SetActive(false); // 상품 패널 비활성화
        LoadCatalogData(); // 카탈로그 데이터를 로드하여 드롭다운에 표시합니다.
    }

    // 카탈로그 추가 메서드
    public void AddCatalog()
    {
        string catalogName = catalogNameInput.text;

        if (!string.IsNullOrEmpty(catalogName))
        {
            InsertCatalogData(catalogName);
        }
        else
        {
            Debug.LogError("카탈로그 이름을 입력하세요.");
        }
    }

    private void InsertCatalogData(string catalogName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("MySQL 연결 성공!");

                // INSERT 쿼리 작성
                string query = "INSERT INTO T_CATALOG (CATALOG_NAME, REG_ADM, REG_DATE, USE_TF) " +
                               "VALUES (@catalogName, @regAdm, @regDate, 'Y')";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // 파라미터 바인딩
                    command.Parameters.AddWithValue("@catalogName", catalogName);
                    command.Parameters.AddWithValue("@regAdm", 1); // 예시로 관리자 ID를 1로 설정
                    command.Parameters.AddWithValue("@regDate", regDate); // 현재 날짜

                    // 쿼리 실행
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Debug.Log("카탈로그가 성공적으로 추가되었습니다.");
                        LoadCatalogData(); // 추가 후 드롭다운 업데이트
                    }
                    else
                    {
                        Debug.LogWarning("카탈로그 추가에 실패했습니다.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Debug.Log("MySQL 연결 종료");
            }
        }
    }

    // 카탈로그 삭제 메서드
    public void DeleteCatalog()
    {
        if (int.TryParse(catalogIdInput.text, out int catalogId)) // 입력한 ID를 정수로 변환
        {
            DeleteCatalogData(catalogId);
        }
        else
        {
            Debug.LogError("유효한 카탈로그 ID를 입력하세요.");
        }
    }

    private void DeleteCatalogData(int catalogId)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("MySQL 연결 성공!");

                // DELETE 쿼리 작성
                string query = "DELETE FROM T_CATALOG WHERE CATALOG_ID = @catalogId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // 파라미터 바인딩
                    command.Parameters.AddWithValue("@catalogId", catalogId);

                    // 쿼리 실행
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Debug.Log("카탈로그가 성공적으로 삭제되었습니다.");
                        LoadCatalogData(); // 삭제 후 드롭다운 업데이트
                    }
                    else
                    {
                        Debug.LogWarning("삭제할 카탈로그가 없습니다.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Debug.Log("MySQL 연결 종료");
            }
        }
    }

    // 카탈로그 업데이트 메서드
    public void UpdateCatalog()
    {
        if (int.TryParse(catalogIdInput.text, out int catalogId))
        {
            string newCatalogName = catalogNameInput.text.Trim();
            if (!string.IsNullOrEmpty(newCatalogName))
            {
                UpdateCatalogData(catalogId, newCatalogName);
            }
            else
            {
                Debug.LogError("새로운 카탈로그 이름을 입력하세요.");
            }
        }
        else
        {
            Debug.LogError("유효한 카탈로그 ID를 입력하세요.");
        }
    }

    private void UpdateCatalogData(int catalogId, string newCatalogName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("MySQL 연결 성공!");

                string query = "UPDATE T_CATALOG SET CATALOG_NAME = @newCatalogName WHERE CATALOG_ID = @catalogId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newCatalogName", newCatalogName);
                    command.Parameters.AddWithValue("@catalogId", catalogId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Debug.Log("카탈로그가 성공적으로 업데이트되었습니다.");
                        LoadCatalogData(); // 업데이트 후 드롭다운 업데이트
                    }
                    else
                    {
                        Debug.LogWarning("업데이트할 카탈로그가 없습니다.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Debug.Log("MySQL 연결 종료");
            }
        }
    }

    // 카탈로그 데이터를 로드하여 드롭다운에 추가하는 메서드
    private void LoadCatalogData()
    {
        List<string> catalogNames = new List<string>(); // 카탈로그 이름들을 저장할 리스트

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("MySQL 연결 성공!");

                string query = "SELECT CATALOG_ID, CATALOG_NAME FROM T_CATALOG WHERE USE_TF = 'Y'";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 카탈로그 이름을 리스트에 추가
                            catalogNames.Add(reader.GetString("CATALOG_NAME"));
                        }
                    }
                }

                // 드롭다운 UI 업데이트
                UpdateDropdown(catalogNames);
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Debug.Log("MySQL 연결 종료");
            }
        }
    }

    // 드롭다운 UI 업데이트
    private void UpdateDropdown(List<string> catalogNames)
    {
        catalogDropdown.ClearOptions(); // 기존 옵션 초기화
        catalogDropdown.AddOptions(catalogNames); // 새로운 옵션 추가
    }

    // 선택한 카탈로그의 챕터를 로드하는 메서드
    public void LoadChapters()
    {
        int selectedCatalogIndex = catalogDropdown.value; // 선택된 카탈로그의 인덱스
        // 선택된 카탈로그의 ID를 가져오는 쿼리 실행
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("MySQL 연결 성공!");

                // SELECT 쿼리 작성
                string query = "SELECT CHAPTER_NAME FROM T_CATALOG_CHAPTER WHERE CATALOG_ID = (SELECT CATALOG_ID FROM T_CATALOG WHERE CATALOG_NAME = @catalogName)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@catalogName", catalogDropdown.options[selectedCatalogIndex].text);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> chapterNames = new List<string>();
                        while (reader.Read())
                        {
                            // 챕터 이름을 리스트에 추가
                            chapterNames.Add(reader.GetString("CHAPTER_NAME"));
                        }
                        // 챕터 드롭다운 업데이트
                        UpdateChapterDropdown(chapterNames);
                        productPanel.SetActive(true); // 상품 패널 활성화
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL 오류: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Debug.Log("MySQL 연결 종료");
            }
        }
    }

    // 챕터 드롭다운 UI 업데이트
    private void UpdateChapterDropdown(List<string> chapterNames)
    {
        chapterDropdown.ClearOptions(); // 기존 옵션 초기화
        chapterDropdown.AddOptions(chapterNames); // 새로운 옵션 추가
    }

    // 선택한 챕터의 페이지를 로드하는 메서드
   public void LoadPages()
{
    // 선택한 챕터의 인덱스 가져오기
    int selectedChapterIndex = chapterDropdown.value;
    
    // 선택한 카탈로그 이름 가져오기
    string selectedCatalogName = catalogDropdown.options[catalogDropdown.value].text;

    // 선택한 챕터 이름 가져오기
    string selectedChapterName = chapterDropdown.options[selectedChapterIndex].text;

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = "SELECT PAGE_NAME FROM T_CATALOG_PAGE " +
                           "WHERE CHAPTER_ID = (SELECT CHAPTER_ID FROM T_CATALOG_CHAPTER WHERE CATALOG_ID = (SELECT CATALOG_ID FROM T_CATALOG WHERE CATALOG_NAME = @catalogName AND CHAPTER_NAME = @chapterName))";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@catalogName", selectedCatalogName);
                command.Parameters.AddWithValue("@chapterName", selectedChapterName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<string> pageNames = new List<string>();
                    while (reader.Read())
                    {
                        pageNames.Add(reader.GetString("PAGE_NAME"));
                    }

                    // 페이지 드롭다운 업데이트
                    UpdatePageDropdown(pageNames);
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("MySQL 오류: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }
}

private void UpdatePageDropdown(List<string> pageNames)
{
    pageDropdown.ClearOptions();
    pageDropdown.AddOptions(pageNames);
}

public Text productDescriptionText1; // 첫 번째 상품 설명
public Text productDescriptionText2; // 두 번째 상품 설명

public void LoadProducts()
{
    int selectedCatalogId = catalogDropdown.value;
    int selectedChapterId = chapterDropdown.value;
    int selectedPageId = pageDropdown.value;

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            Debug.Log("MySQL 연결 성공 - 상품 로드 시작");

            string query = @"SELECT DESCRIPTION1, DESCRIPTION2 
                             FROM T_CATALOG_GOODS 
                             WHERE CATALOG_ID = @catalogId 
                             AND CHAPTER_ID = @chapterId 
                             AND PAGE_ID = @pageId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@catalogId", selectedCatalogId);
                command.Parameters.AddWithValue("@chapterId", selectedChapterId);
                command.Parameters.AddWithValue("@pageId", selectedPageId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        productDescriptionText1.text = ""; // 초기화
                        productDescriptionText2.text = ""; // 초기화
                        
                        while (reader.Read())
                        {
                            // 첫 번째 설명과 두 번째 설명을 각각 다른 텍스트 UI에 표시
                            productDescriptionText1.text = reader["DESCRIPTION1"].ToString();
                            productDescriptionText2.text = reader["DESCRIPTION2"].ToString();
                        }
                        Debug.Log("상품 정보 로드 성공");
                    }
                    else
                    {
                        Debug.LogWarning("해당 페이지에 상품이 없습니다.");
                        productDescriptionText1.text = "해당 페이지에 상품이 없습니다.";
                        productDescriptionText2.text = "";
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("MySQL 오류: " + ex.Message);
        }
        finally
        {
            connection.Close();
            Debug.Log("MySQL 연결 종료");
        }
    }
}


    private void DisplayProductData(List<string> productDescriptions1, List<string> productDescriptions2)
{
    if (productDescriptions1.Count == 0 || productDescriptions2.Count == 0)
    {
        productDescriptionText1.text = "상품 설명이 없습니다."; // 상품 설명1이 없을 경우 표시
        productDescriptionText2.text = ""; // 상품 설명2이 없을 경우 표시 안 함
    }
    else
    {
        // 첫 번째와 두 번째 설명을 각각의 Text UI에 표시
        productDescriptionText1.text = string.Join("\n", productDescriptions1);
        productDescriptionText2.text = string.Join("\n", productDescriptions2);
    }

    // 상품 패널 활성화
    productPanel.SetActive(true);
  }
}
