using UnityEngine;

public class DeadCodeScript : MonoBehaviour
{
    public Transform cristian;
    private CristianMovimiento _cristianScript;
    public float velocidad;
    public float distanciaAutodestruccion ; 
    public int danoPorAutodestruccion ;
    private const float _factorRalentizacion=0.5f; 
    private const float _duracionRalentizacion = 3f; // Tiempo en segundos
    public AudioClip sonido;
    public AudioClip sonidoExplosion;
    
    private int _salud = 3;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direccionMovimiento;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
                GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            cristian = jugador.transform;
            _cristianScript = jugador.GetComponent<CristianMovimiento>();
            
            if (_cristianScript == null)
            {
                Debug.LogError("No se encontró CristianMovimiento en el jugador");
            }
        }
        else
        {
            Debug.LogError("No se encontró objeto con tag 'Player'");
        }
        // if (sonido != null && Camera.main != null)
        // {
        //     Camera.main.GetComponent<AudioSource>().PlayOneShot(sonido);
        // }
    }

    void Update()
    {
        if (cristian == null || _cristianScript == null) return;
        Vector2 direccion = cristian.position - transform.position;

        // Flip sprite
        transform.localScale = new Vector3(
            direccion.x >= 0 ? 1f : -1f,
            1f,
            1f
        );

        _direccionMovimiento = direccion.normalized;

        // Verificar distancia para autodestrucción
         if (direccion.magnitude <= distanciaAutodestruccion)
        {
            _cristianScript.AplicarRalentizacion(_factorRalentizacion, _duracionRalentizacion);
            Autodestruir();
        }
    }
    void FixedUpdate()
    {
        if (cristian == null) return;
        _rigidbody2D.linearVelocity = _direccionMovimiento * velocidad;
    }

    void Autodestruir()
    {
        // Aplicar daño a Cristian
        CristianMovimiento cristianScript = cristian.GetComponent<CristianMovimiento>();
        if (cristianScript != null)
        {
            for (int i = 0; i < danoPorAutodestruccion; i++)
            {
                cristianScript.Golpe();
            }
        }

        // Sonido de explosión
        // if (sonidoExplosion != null && Camera.main != null)
        // {
        //     Camera.main.GetComponent<AudioSource>().PlayOneShot(sonidoExplosion);
        // }

        // Efecto visual opcional (si quieres agregar una explosión)
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud <= 0) 
        {
            Destroy(gameObject);
        }
    }
}