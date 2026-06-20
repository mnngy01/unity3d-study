using UnityEngine;
using TMPro;

public class PasswordManager : MonoBehaviour
{
    public TMP_InputField inputField;   // 입력받는 값
    public TMP_Text resultText; // UI 출력용

    public string correctPassword = "1234"; // 정답 비밀번호
    public OpenDoor door;   // 정답시 열릴 문

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CheckPassword()
    {
        if (inputField.text == correctPassword)
        {
            Debug.Log("correct");
            door.ToggleDoor();  // 문 열기
            ClosePanel();
        }
        else
        {
            Debug.Log("incorrect");
            inputField.text = "";
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
