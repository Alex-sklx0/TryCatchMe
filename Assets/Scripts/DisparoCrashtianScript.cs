using UnityEngine;

public class DisparoCrashtianScript : MonoBehaviour
{
    // Constantes
    private const string TagJugador = "Player";
    private const string TagCreador = "Crashtian";
    private const float DuracionVidaDisparo = 5f;
    private const float Velocidad = 0.3f;
    private const float DuracionVida = 5f;
    private const float Dano = 2f;
    private const float ConstanteGiroSprite = 1f;

    
    // Serialized (editable desde el inspector)
    [SerializeField] private AudioClip sonidoDisparo;

    // Variables privadas
    private Rigidbody2D _rigidbody;

     private void Start()
    {
        try
        {
            InicializarComponentes();
            BuscarYDirigirAlJugador();
            ReproducirSonidoDisparo();
            ProgramarAutodestruccion();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCrashtian] Error en Start: {e.Message}", this);
        }
    }

    private void InicializarComponentes()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogWarning("[DisparoCrashtian] Rigidbody2D no encontrado.");
        }
    }

    private void BuscarYDirigirAlJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag(TagJugador);
        if (jugador == null)
        {
            Debug.LogWarning("[DisparoCrashtian] No se encontró al jugador.");
            return;
        }

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;

        if (_rigidbody != null)
            _rigidbody.linearVelocity = direccion * Velocidad;

        transform.localScale = new Vector3(
            direccion.x >= 0f ? 1f : -1f,
            ConstanteGiroSprite,
            ConstanteGiroSprite
        );
    }

    private void ReproducirSonidoDisparo()
    {
        try
        {
            if (sonidoDisparo != null && Camera.main != null)
            {
                AudioSource audio = Camera.main.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.PlayOneShot(sonidoDisparo);
                }
                else
                {
                    Debug.LogWarning("[DisparoCrashtian] No se encontró AudioSource en la cámara.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCrashtian] Error al reproducir sonido: {e.Message}", this);
        }
    }

    private void ProgramarAutodestruccion()
    {
        Destroy(gameObject, DuracionVidaDisparo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag(TagCreador)) return;

            if (other.CompareTag(TagJugador))
            {
                CristianMovimiento cristian = other.GetComponent<CristianMovimiento>();
                if (cristian != null)
                {
                    cristian.Golpe(Dano);
                }
            }

            Destroy(gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DisparoCrashtian] Error al colisionar con {other.name}: {e.Message}", this);
        }
    }
}