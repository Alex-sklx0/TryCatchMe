using UnityEngine;

public class DisparoEspagueti : MonoBehaviour
{
    //constantes
    private const float Duracion = 3f;
    private const float VelocidadCrecimiento = 1.1f;
    private const float LongitudMaxima = 2f;
    private const float Dano = 1f;
    private const string TagJugador = "Player";
    private const float EscalaEjesSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const float EscalaInicialXSprite = 0.1f; //constante para cuando se gira el srite en direccion y,z

    private const float TiempoCicloParpaedo = 1f; //constante para cuando se gira el srite en direccion y,z
    private const float ParpadeoVelocidad = 10f;
    private const float UmbralVisibilidad = 0.5f;
    private const float CicloParpadeo = 1f;
    private const float TamanoMinimoParaActivarCollider = 1f;


    //variables privadas
    private float _tiempo;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

     private void Start()
    {
        ObtenerComponentes();
        ConfigurarEscalaInicial();
        DesactivarColision();
        Autodestruccion();
    }

    private void ObtenerComponentes()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ConfigurarEscalaInicial()
    {
        transform.localScale = new Vector3(EscalaInicialXSprite, EscalaEjesSprite, EscalaEjesSprite);
    }

    private void DesactivarColision()
    {
        if (_collider != null)
            _collider.enabled = false;
    }

    private void Autodestruccion()
    {
        Destroy(gameObject, Duracion);
    }

    // Lógica por frame
    private void Update()
    {
        _tiempo += Time.deltaTime;
        CrecerHorizontalmente();
        EjecutarParpadeoFinal();
    }

    private void CrecerHorizontalmente()
    {
        if (transform.localScale.x >= LongitudMaxima) return;

        float nuevaLongitud = Mathf.Min(
            transform.localScale.x + VelocidadCrecimiento * Time.deltaTime,
            LongitudMaxima
        );

        transform.localScale = new Vector3(nuevaLongitud, EscalaEjesSprite, EscalaEjesSprite);

        if (nuevaLongitud > TamanoMinimoParaActivarCollider && !_collider.enabled)
        {
            _collider.enabled = true;
        }
    }

    private void EjecutarParpadeoFinal()
    {
        if (_tiempo >= Duracion - TiempoCicloParpaedo && _spriteRenderer != null)
        {
            _spriteRenderer.enabled = Mathf.PingPong(_tiempo * ParpadeoVelocidad, CicloParpadeo) > UmbralVisibilidad;
        }
    }

    // Colisiones
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagJugador))
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(Dano);
        }
    }
}