using UnityEngine;

public class TraceInteractivo : MonoBehaviour
{
    [SerializeField] private SpaghettiCode jefe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jefe != null && other.CompareTag("Player"))
        {
            jefe.VerificarTrace(gameObject.name); // o puedes usar un ID numérico
        }
    }
}
