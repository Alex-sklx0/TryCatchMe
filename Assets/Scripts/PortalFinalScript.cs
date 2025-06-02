using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFinal : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJefe = "BossScene";
    [SerializeField] private GameObject mensajeAdvertenciaUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TraceRecolector recolector = other.GetComponent<TraceRecolector>();
            if (recolector != null && recolector.TieneTodosLosTraces())
            {
                SceneManager.LoadScene(nombreEscenaJefe);
            }
            else
            {
                if (mensajeAdvertenciaUI != null)
                {
                    mensajeAdvertenciaUI.SetActive(true);
                    Invoke("OcultarMensaje", 3f); // Lo esconde luego de 3s
                }
            }
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeAdvertenciaUI != null)
            mensajeAdvertenciaUI.SetActive(false);
    }
}
