using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,   // 좌우만 따라감
            transform.position.y,           // 높이 고정
            transform.position.z            // 거리 고정
        );
    }
}
