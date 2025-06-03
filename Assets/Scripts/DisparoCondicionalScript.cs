using UnityEngine;

public class DisparoCondicionalScript : MonoBehaviour
{
    private const float Dano = 1f;
    private const float DuracionDestruirInmediata = 0f;
    private const float DuracionDestruirGolpe = 3f;
    private const string TagJugador = "Player";

    // ─────────────────────────────────────────────
    // Públicos y serializados
    public float velocidad;
    public float duracionBloqueoSalto = 2f;
    public AudioClip sonido;

    // ─────────────────────────────────────────────
    // Privadas
    private Rigidbody2D _rigidbody2D;
    private Vector3 _direccion;

    public Vector3 Direccion
    {
        set { _direccion = value; }
    }

    private void Start()
    {
        try
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (Camera.main != null && Camera.main.GetComponent<AudioSource>() != null)
            {
                Camera.main.GetComponent<AudioSource>().PlayOneShot(sonido);
            }
            else
            {
                Debug.LogWarning("[DisparoCondicional] No se encontró AudioSource en la cámara.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCondicional] Error en Start: {e.Message}", this);
        }
    }

    private void FixedUpdate()
    {
        try
        {
            if (_rigidbody2D != null)
            {
                _rigidbody2D.linearVelocity = _direccion * velocidad;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCondicional] Error en FixedUpdate: {e.Message}", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag(TagJugador))
            {
                CristianMovimiento jugador = other.GetComponent<CristianMovimiento>();
                if (jugador != null)
                {
                    jugador.BloquearSalto(duracionBloqueoSalto);
                    jugador.Golpe(Dano);
                }

                DestruirDisparo(DuracionDestruirGolpe);
            }
            else
            {
                DestruirDisparo(DuracionDestruirInmediata);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCondicional] Error en colisión con {other.name}: {e.Message}", this);
        }
    }

    public void DestruirDisparo(float duracion)
    {
        try
        {
            Destroy(gameObject, duracion);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCondicional] Error al destruir disparo: {e.Message}", this);
        }
    }
}