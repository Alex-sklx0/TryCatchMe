using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFinal : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreEscenaJefe;
    [SerializeField] private GameObject mensajeAdvertenciaUI;

    private void Start()
    {
        // Asegurar que el mensaje esté oculto al iniciar
        if (mensajeAdvertenciaUI != null)
        {
            mensajeAdvertenciaUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("mensajeAdvertenciaUI no está asignado en el inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        TraceRecolector recolector = other.GetComponent<TraceRecolector>();
        if (recolector != null && recolector.TieneTodosLosTraces())
        {
            SceneManager.LoadScene(nombreEscenaJefe);
        }
        else
        {
            MostrarAdvertenciaTemporal();
        }
    }

    private void MostrarAdvertenciaTemporal()
    {
        if (mensajeAdvertenciaUI != null)
        {
            mensajeAdvertenciaUI.SetActive(true);
            CancelInvoke(nameof(OcultarMensaje)); // evita duplicar invocaciones
            Invoke(nameof(OcultarMensaje), 3f);
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeAdvertenciaUI != null)
        {
            mensajeAdvertenciaUI.SetActive(false);
        }
    }
}
