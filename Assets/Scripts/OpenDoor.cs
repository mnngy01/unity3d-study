using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public float openAngle = -90f;
    public float openSpeed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    void Start()
    {
        closedRotation = transform.rotation;

        openedRotation = Quaternion.Euler(
            transform.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }

    void Update()
    {
        Quaternion targetRotation = isOpen
            ? openedRotation
            : closedRotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}