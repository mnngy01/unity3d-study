using UnityEngine;

public class OpenBox : MonoBehaviour
{
    public float openAngle = -90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openedRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openedRot = Quaternion.Euler( transform.localEulerAngles + new Vector3(0, 0, openAngle) );
    }

    void Update()
    {
        Quaternion target = isOpen ? openedRot : closedRot;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            target,
            Time.deltaTime * openSpeed
        );
    }

    public void Open()
    {
        isOpen = true;
    }
}
