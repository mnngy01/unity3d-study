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
            player.position.x + offset.x,       // 좌우 따라감
            player.position.y + offset.y,       // 높이 따라감
            transform.position.z                // 거리 고정
        );
    }
}
