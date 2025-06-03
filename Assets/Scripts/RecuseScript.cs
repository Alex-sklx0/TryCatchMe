using UnityEngine;

public class Recurse : MonoBehaviour
{
    public float radioDeteccion ;
    public float velocidad ;
    private int _salud = 3;
    public float radioExplosion = 2f;
    public GameObject acidoPrefab;
    public AudioClip sonidoExplosion;

    public Color colorAlerta = Color.red;
    public GameObject efectoExplosion;
private bool _yaExplotó = false;
    public Transform cristian;
    private SpriteRenderer _renderer;
    private Color _colorOriginal;
    private bool _enAlerta = false;

    private void Start()
    {
        cristian = GameObject.FindGameObjectWithTag("Player")?.transform;
        _renderer = GetComponent<SpriteRenderer>();
        _colorOriginal = _renderer.color;
    }

    private void Update()
    {
    if (cristian == null || _yaExplotó) return;
        float distancia = Vector2.Distance(transform.position, cristian.position);

       

        // Detección del jugador
        if (distancia <= radioDeteccion)
        {
            _enAlerta = true;
            _renderer.color = colorAlerta;
        //mirar al jugador
            Vector2 direccion = (cristian.position - transform.position).normalized;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            // Movimiento hacia el jugador
            transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
        }
        // Explosión automática si el jugador está muy cerca
if (distancia <= radioExplosion)
{
    Explotar();
}

        else if (_enAlerta)
        {
            _enAlerta = false;
            _renderer.color = _colorOriginal;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explotar();
        }
        else if (other.CompareTag("Disparo"))
        {
            Golpe();
        }
    }
private void Explotar()
{
    if (_yaExplotó) return;
    _yaExplotó = true;

    Debug.Log("¡Recurse explota!");

    // Instanciar ácido (solo una vez)
    if (acidoPrefab != null)
    {
Vector3 posicionAcido = transform.position + new Vector3(0f, -0.11f, 0f);
Instantiate(acidoPrefab, posicionAcido, Quaternion.identity);
    }

    // Efectos visuales/sonido
    if (efectoExplosion != null)
    {
        Instantiate(efectoExplosion, transform.position, Quaternion.identity);
    }

    if (sonidoExplosion != null && Camera.main != null)
    {
        AudioSource.PlayClipAtPoint(sonidoExplosion, Camera.main.transform.position);
    }

    // Desactivar render y colisiones
    _renderer.enabled = false;
    GetComponent<Collider2D>().enabled = false;

    // Destruir el objeto después de un delay
    Destroy(gameObject, 0.1f);
}
    public void Golpe()
    {
        _salud --;
        if (_salud <= 0)
        {
            Explotar();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
