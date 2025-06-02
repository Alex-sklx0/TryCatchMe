using UnityEngine;

public class TraceNivelScript : MonoBehaviour
{
    private const string TagJugador = "Player";
    private bool recogido = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!recogido && other.CompareTag(TagJugador))
        {
            TraceRecolector recolector = other.GetComponent<TraceRecolector>();
            if (recolector != null)
            {
                recolector.RecogerTrace();
                recogido = true;
                gameObject.SetActive(false); // Oculta o destruye el trace
            }
        }
    }
}
