using UnityEngine;

public class EspaguetiScript : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion ;
    public float velocidadCrecimiento ;
    public float longitudMaxima ;
    private float _dano = 1f;
    private float _tiempo;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Comenzar con escala mínima en X (horizontal)
        transform.localScale = new Vector3(0.01f, 1f, 1f);
        _collider.enabled = false;

        Destroy(gameObject, duracion);
    }

    void Update()
    {
        _tiempo += Time.deltaTime;

        // Crecimiento horizontal
        if (transform.localScale.x < longitudMaxima)
        {
            float nuevaLongitud = Mathf.Min(
                transform.localScale.x + velocidadCrecimiento * Time.deltaTime,
                longitudMaxima
            );

            transform.localScale = new Vector3(
                nuevaLongitud,
                1f, // Escala vertical constante
                1f
            );

            // Activar colisión al alcanzar cierto tamaño
            if (nuevaLongitud > 1f && !_collider.enabled)
            {
                _collider.enabled = true;
            }
        }

        // Parpadeo visual en el último segundo
        if (_tiempo >= duracion - 1f && _spriteRenderer != null)
        {
            _spriteRenderer.enabled = Mathf.PingPong(_tiempo * 10f, 1f) > 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(_dano);
        }
    }
}
