using UnityEngine;

public class TraceInteractivo : MonoBehaviour
{
    private const string TagJugador = "Player";

    [SerializeField] private SpaghettiCode jefe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jefe != null && other.CompareTag(TagJugador))
        {
            jefe.VerificarTrace(gameObject.name); // o puedes usar un ID numérico
        }
    }
}
