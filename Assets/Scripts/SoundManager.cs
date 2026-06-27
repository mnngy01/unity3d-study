using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 현재 씬에서 하나만 존재하도록
    public static SoundManager Instance;

    // 현재 보스
    public LanternBossController boss;

    void Awake()
    {
        Instance = this;
    }

    // 소리 발생
    public void MakeSound(Vector3 soundPosition)
    {
        boss.HearSound(soundPosition);
    }
}
