using UnityEngine;

public class DisparoCrashtianScript : MonoBehaviour
{
    // Constantes
    private const string TagJugador = "Player";
    private const string TagCreador = "Crashtian";
private const float velocidad = 0.3f;
    private const float duracionVida = 5f;
    private const float dano = 2f;
    
    // Serialized (editable desde el inspector)
    [SerializeField] private AudioClip sonidoDisparo;

    // Variables privadas
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        InicializarComponentes();
        BuscarYDirigirAlJugador();
        ReproducirSonidoDisparo();
        ProgramarAutodestruccion();
    }

    private void InicializarComponentes()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void BuscarYDirigirAlJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag(TagJugador);
        if (jugador == null) return;

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;
        _rigidbody.linearVelocity = direccion * velocidad;

        // Orientación visual del disparo
        transform.localScale = new Vector3(
            direccion.x >= 0f ? 1f : -1f,
            1f,
            1f
        );
    }

    private void ReproducirSonidoDisparo()
    {
        if (sonidoDisparo != null && Camera.main != null)
        {
            AudioSource audio = Camera.main.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.PlayOneShot(sonidoDisparo);
            }
        }
    }

    private void ProgramarAutodestruccion()
    {
        Destroy(gameObject, duracionVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagCreador)) return;

        if (other.CompareTag(TagJugador))
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(dano);
        }

        Destroy(gameObject);
    }
}
