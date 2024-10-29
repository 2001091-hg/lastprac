using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class MariaDBTest : MonoBehaviour{
    private void Start()
    {
        Debug.Log("Connection Test:" + ConnectionTest());

    }
   public bool ConnectionTest()
{
    String conStr = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", "giftnet.co.kr", "kustaf", "kustaf", "pinics02!");

    try
    {
        using (MySqlConnection conn = new MySqlConnection(conStr))
        {
            conn.Open();
            Debug.Log("연결 성공 ~!!");
        }
        return true;    
    }
    catch (Exception e)
    {
        Debug.Log("Error: " + e.Message);
        Debug.Log("Stack Trace: " + e.StackTrace); // 스택 트레이스도 로그에 출력
        return false; // false를 반환하도록 수정
    }
  }
}