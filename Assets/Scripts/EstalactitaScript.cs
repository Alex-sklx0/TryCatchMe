using UnityEngine;

public class Estalactita : MonoBehaviour
{
    // Constantes
    private const float GravedadInicial = 0f;
    private const float GravedadActiva = 0.3f;
    private const float Dano = 1.5f;
    private const string TagJugador = "Player";
    private const string TagSuelo = "Ground";

    // Variables privadas
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        InicializarEstalactita();
    }

    private void InicializarEstalactita()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = GravedadInicial; // Suspende al inicio
    }

   
    public void CaerLuegoDe(float segundos)
    {
        Invoke(nameof(ActivarCaida), segundos);
    }

    private void ActivarCaida()
    {
        _rigidbody.gravityScale = GravedadActiva;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagJugador))
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(Dano);
            DestruirEstalactita();
        }
        else if (other.CompareTag(TagSuelo))
        {
            DestruirEstalactita();
        }
    }

    private void DestruirEstalactita()
    {
        Destroy(gameObject);
    }
}
