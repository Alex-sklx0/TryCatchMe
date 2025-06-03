using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalJefe : MonoBehaviour
{
    [SerializeField] private string nombreEscenaSiguiente;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscenaSiguiente);
        }
    }
}
