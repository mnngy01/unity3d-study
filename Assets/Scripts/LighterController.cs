using UnityEngine;

public class LighterController : MonoBehaviour
{
    public GameObject lighter;  // 라이터 오브젝트
    public Light lighterLight;  // 라이터 광원
    
    private bool isUsingLighter = false;    // 현재 라이터 사용 중인지

    void Start()
    {
        // 게임 시작 시 라이터와 불빛 끄기
        lighter.SetActive(false);
        lighterLight.enabled = false;
    }

    void Update()
    {
        // F키 누르면 라이트 켜기/끄기
        if (Input.GetKeyDown(KeyCode.F)) {
            ToggleLighter();
        }
    }

    // 라이터 켜기/끄기
    void ToggleLighter()
    {
        isUsingLighter = !isUsingLighter;   // 사용 여부 상태 바꾸기
        lighter.SetActive(isUsingLighter);  // 라이터 오브젝트 활성화 상태 바꾸기 (보이기/숨기기)
        lighterLight.enabled = isUsingLighter;  // 광원 켜기/끄기
    }

    // 라이터 끄기
    public void TurnOffLighter()
    {
        isUsingLighter = false; // 사용 상태 off
        lighter.SetActive(false);
        lighterLight.enabled = false;
    }
}
