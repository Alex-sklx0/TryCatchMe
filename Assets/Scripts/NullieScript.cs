using UnityEngine;
using System.Collections;

public class NullieScript : MonoBehaviour
{
    [Header("Configuración Básica")]
    public Transform cristian;
    public float velocidad = 2.5f;
    
    [Header("Efecto de Daño")]
    public float distanciaActivacion = 2f;
    public float cooldownDaño = 1f;
    
    [Header("Efecto de Bloqueo")]
    public float duracionBloqueo = 3f;
    public float tiempoEntreBloqueos = 5f;
    public float tiempoMinimoEntreBloqueos = 2f; // Nuevo: tiempo mínimo antes de poder bloquear de nuevo
    
    [Header("Audio")]
    public AudioClip sonidoAdvertencia;
    public AudioClip sonidoBloqueo;
    public AudioClip sonidoDaño;
    
    private Rigidbody2D _rigidbody2D;
    private CristianMovimiento _cristianScript;
    private SpriteRenderer _spriteRenderer;
    private int _salud = 5;
    private float _ultimoDaño;
    private float _ultimoBloqueo;
    private bool _bloqueoActivo;
    private bool _puedeBloquear = true;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            cristian = playerObj.transform;
            _cristianScript = playerObj.GetComponent<CristianMovimiento>();
        }
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

        // Movimiento
        _rigidbody2D.linearVelocity = direccion.normalized * velocidad;

        // Verificar distancia para efectos
        if (direccion.magnitude <= distanciaActivacion)
        {
            AplicarDaño();
            
            // Solo intentar bloquear si no está ya bloqueando y ha pasado el tiempo mínimo
            if (_puedeBloquear && !_bloqueoActivo && 
                Time.time - _ultimoBloqueo >= tiempoMinimoEntreBloqueos)
            {
                StartCoroutine(CicloBloqueo());
            }
        }
    }

    void AplicarDaño()
    {
        if (Time.time - _ultimoDaño >= cooldownDaño)
        {
            _cristianScript.Golpe();
            _ultimoDaño = Time.time;
            
            _spriteRenderer.color = Color.red;
            Invoke("ResetColor", 0.2f);
            
            if (sonidoDaño != null) PlaySound(sonidoDaño);
        }
    }

    IEnumerator CicloBloqueo()
    {
        _puedeBloquear = false;
        _bloqueoActivo = true;
        _ultimoBloqueo = Time.time;

        // Fase de advertencia
        _spriteRenderer.color = Color.yellow;
        if (sonidoAdvertencia != null) PlaySound(sonidoAdvertencia);
        yield return new WaitForSeconds(0.5f);

        // Fase de bloqueo activo
        _cristianScript.BloquearDisparos(true);
        _spriteRenderer.color = Color.cyan;
        if (sonidoBloqueo != null) PlaySound(sonidoBloqueo);
        yield return new WaitForSeconds(duracionBloqueo);

        // Fin de bloqueo
        _cristianScript.BloquearDisparos(false);
        _bloqueoActivo = false;
        _spriteRenderer.color = Color.white;

        // Cooldown completo
        yield return new WaitForSeconds(tiempoEntreBloqueos);
        _puedeBloquear = true;
    }

    void ResetColor()
    {
        if (!_bloqueoActivo) // Solo resetear si no está en modo bloqueo
        {
            _spriteRenderer.color = Color.white;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud <= 0) Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanciaActivacion);
    }
}