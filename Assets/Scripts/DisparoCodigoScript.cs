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

    // Variables privadas
    private Rigidbody2D _rigidbody2D;

 private void Start()
    {
        try
        {
            InicializarFisica();
            AutodestruirDespuesDeTiempo();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error en Start: {e.Message}", this);
        }
    }

    private void InicializarFisica()
    {
        try
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (_rigidbody2D != null)
            {
                _rigidbody2D.gravityScale = Gravedad;
            }
            else
            {
                Debug.LogWarning("[ChorroDisparo] Rigidbody2D no encontrado en el objeto.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error al inicializar físicas: {e.Message}", this);
        }
    }

    private void AutodestruirDespuesDeTiempo()
    {
        try
        {
            Destroy(gameObject, TiempoVida);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error al programar autodestrucción: {e.Message}", this);
        }
    }

    public void Inicializar(Vector2 direccion, float fuerza)
    {
        try
        {
            _rigidbody2D = _rigidbody2D != null ? _rigidbody2D : GetComponent<Rigidbody2D>();

            if (_rigidbody2D != null)
            {
                _rigidbody2D.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error al aplicar fuerza: {e.Message}", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag(TagIgnorar)) return;

            if (other.CompareTag(TagJugador))
            {
                CristianMovimiento cristian = other.GetComponent<CristianMovimiento>();
                cristian?.Golpe(Dano);
                DestruirDisparo();
            }
            else if (other.CompareTag(TagSuelo))
            {
                DestruirDisparo();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error en colisión con {other.name}: {e.Message}", this);
        }
    }

    private void DestruirDisparo()
    {
        try
        {
            Destroy(gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChorroDisparo] Error al destruir disparo: {e.Message}", this);
        }
    }
}