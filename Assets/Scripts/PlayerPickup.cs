using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 2f;

    private GameObject nearbyObject;
    private GameObject heldObject;

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
        if (other.CompareTag("Pickable"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            if (nearbyObject == other.gameObject)
            {
                nearbyObject = null;
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        heldObject.transform.SetParent(null);

        heldObject = null;
    }
}