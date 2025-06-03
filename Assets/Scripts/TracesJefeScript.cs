using UnityEngine;

public class TracesJefe : MonoBehaviour
{
    private const string TagJugador = "Player";

    [SerializeField] private MonoBehaviour jefeGenerico; // cualquier script
    private IVerificadorTrace _jefe;

    private void Start()
    {
        _jefe = jefeGenerico as IVerificadorTrace;
        if (_jefe == null)
        {
            Debug.LogError("El objeto asignado no implementa IVerificadorTrace.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(TagJugador)) return;
        _jefe?.VerificarTrace(gameObject.name);
    }
}

public interface IVerificadorTrace
{
    void VerificarTrace(string idSeleccionado);
}
