using UnityEngine;

public class DisparoAlbondiga : MonoBehaviour
{
    // CONSTANTES
    private const float EscalaGravedadReducida = 0.3f;
    private const float TiempoVida = 5f;
    private const float Dano = 1f;
    private const string TagJugador = "Player";
    private const string TagSuelo = "Ground";
    private const float TiempoDestruccionInmediato = 0f;

    // PRIVADAS 
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        try
        {
            ObtenerComponentes();
            Autodestruir();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error en Start: {e.Message}", this);
        }
    }

    public void ObtenerComponentes()
    {
        try
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = EscalaGravedadReducida;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al obtener Rigidbody2D: {e.Message}", this);
        }
    }

    private void Autodestruir()
    {
        try
        {
            Destroy(gameObject, TiempoVida);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al programar autodestrucción: {e.Message}", this);
        }
    }

    public void LanzarDisparo(Vector2 direccion, float fuerza)
    {
        try
        {
            RedirigirDisparo(direccion, fuerza);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al lanzar disparo: {e.Message}", this);
        }
    }

    private void RedirigirDisparo(Vector2 direccion, float fuerza)
    {
        try
        {
            _rigidbody2D = _rigidbody2D != null ? _rigidbody2D : GetComponent<Rigidbody2D>();
            _rigidbody2D.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al redirigir disparo: {e.Message}", this);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag(TagJugador))
            {
                other.GetComponent<CristianMovimiento>()?.Golpe(Dano);
                Destroy(gameObject, TiempoDestruccionInmediato);
            }
            else if (other.CompareTag(TagSuelo))
            {
                Destroy(gameObject, TiempoDestruccionInmediato);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error en colisión con {other.name}: {e.Message}", this);
        }
    }
}
