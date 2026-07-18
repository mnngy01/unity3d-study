using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleSymbol : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float targetAngle = 270; // 문양을 맞춰야 하는 위치

    [SerializeField] private float snapAngle = 2f;

    // 문양이 맞게 위치했는지 판정
    public bool IsMatched()
    {
        float current = transform.localEulerAngles.z;
        
        // 현재 각도를 snap 기준으로 반올림
        current = Mathf.Round(current / snapAngle) * snapAngle;

        return Mathf.Abs(current - targetAngle) < 10f;
    }
}
