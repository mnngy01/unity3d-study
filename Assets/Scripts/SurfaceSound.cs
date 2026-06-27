using UnityEngine;

public class SurfaceSound : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 걷는 중일 때만
        if (other.GetComponent<PlayerController>().IsMoving())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                SoundManager.Instance.MakeSound(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop(); 
        }
    }
}
