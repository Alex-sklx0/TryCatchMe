using UnityEngine;

public class ChorroDisparo : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // Constantes
    private const float Dano = 1f;
    private const float TiempoVida = 4f;
    private const float Gravedad = 0.3f;
    private const string TagJugador = "Player";
    private const string TagSuelo = "Ground";
    private const string TagIgnorar = "DisparoCrashtian";

    // ─────────────────────────────────────────────
    // Variables privadas
    private Rigidbody2D _rigidbody2D;

    // ─────────────────────────────────────────────
    private void Start()
    {
        InicializarFisica();
        AutodestruirDespuesDeTiempo();
    }

    private void InicializarFisica()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D != null)
        {
            _rigidbody2D.gravityScale = Gravedad;
        }
        else
        {
            Debug.LogWarning("No se encontró Rigidbody2D en el objeto.");
        }
    }

    private void AutodestruirDespuesDeTiempo()
    {
        Destroy(gameObject, TiempoVida);
    }

    public void Inicializar(Vector2 direccion, float fuerza)
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_rigidbody2D != null)
        {
            _rigidbody2D.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagIgnorar)) return;

        if (other.CompareTag(TagJugador))
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(Dano);
            DestruirDisparo();
        }
        else if (other.CompareTag(TagSuelo))
        {
            DestruirDisparo();
        }
    }

    private void DestruirDisparo()
    {
        Destroy(gameObject);
    }
}
