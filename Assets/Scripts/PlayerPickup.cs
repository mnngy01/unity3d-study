using NUnit.Framework;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;     // 물건 드는 위치
    public float pickupRange = 2f;  // 물건 집는 범위
    private LighterController lighterController; // 라이터 스크립트 호출

    private GameObject nearbyObject;
    private GameObject heldObject;
    private bool hasKey = false;    // key 주웠는지


    void Start()
    {
        lighterController = GetComponent<LighterController>();
    }

    void Update()
    {
        // E 키 입력
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 이미 들고 있으면 내려놓기
            if (heldObject != null)
            {
                DropObject();
            }
            // 근처 물건 줍기
            else if (nearbyObject != null)
            {
                PickupObject(nearbyObject);
            }
        }
    }

    // 근처 물체 감지
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") || other.CompareTag("Key"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") || other.CompareTag("Key"))
        {
            if (nearbyObject == other.gameObject)
            {
                nearbyObject = null;
            }
        }
    }

    // 물건 줍기
    void PickupObject(GameObject obj)
    {
        lighterController.TurnOffLighter(); // 라이터 끄기

        // Key인 경우 변수 변경
        if (obj.CompareTag("Key"))
        {
            hasKey = true;
        }    

        heldObject = obj;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        // 중력 끄고 키네마틱 키기
        rb.useGravity = false;
        rb.isKinematic = true;

        // 부모오브젝트 설정, 포지션과 회전여부 고정
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    // 물건 내려놓기
    void DropObject()
    {
        // Key인 경우 변수 변경
        if (heldObject.CompareTag("Key"))
        {
            hasKey = false;
        }

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        heldObject.transform.SetParent(null);

        heldObject = null;
    }

    // hasKey 상태 출력
    public bool HasKey()
    {
        return hasKey;
    }
}