using UnityEngine;
using System.Collections;

public class Nullie : MonoBehaviour, IDanable
{
    //constantes
    private const float _velocidad = 0.3f;
    private const float _radioMovimiento = 0.43f; // Nuevo radio para activar el movimiento

    private const float _distanciaActivacion = 0.15f;
    private const float _cooldownDano = 1f;

    private const float _tiempoEsperaCiclo = 0.5f; 
    private const float _duracionBloqueo = 4f; // Tiempo que dura el bloqueo activo.
    private const float _cooldownBloqueo = 4f; // Tiempo total entre bloqueos   
    private const float _constanteGiroSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const int _saludMin = 0;     

    //serializados y variables publicos 
    [SerializeField] private Transform _cristianPosicion;

    public AudioClip sonidoAdvertencia;
    public AudioClip sonidoBloqueo;
    public AudioClip sonidoDaño;
    //variables privadas
    private Rigidbody2D _rigidbody2D;
    private CristianMovimiento _cristianScript;
    private SpriteRenderer _spriteRenderer;
    private float _dano = 1.25f;
    private int _salud = 5;
    private float _ultimoDano;
    private float _ultimoBloqueo;
    private bool _bloqueoActivo;
    private bool _puedeBloquear = true;
    private Vector2 _direccionMovimiento;
    private bool _jugadorDetectado = false;



    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_cristianPosicion == null)
        {
            Debug.LogError("No se asignó la posición de Cristian (Transform).");
            return;
        }

        // Usar operador de coalescencia nula para intentar obtener el componente y reportar errores
        _cristianScript = _cristianPosicion.GetComponent<CristianMovimiento>();

        if (_cristianScript == null)
        {
            Debug.LogError($"No se encontró el componente CristianMovimiento en el objeto {_cristianPosicion.name}.");
        }
    }

    public void Update()
    {


        //verificar en cada frame si cristian existe en la escena
        if (_cristianPosicion == null) return;
        //magnitud de distancia entre nullie y jugador
        float distanciaAlJugador = Vector2.Distance(_cristianPosicion.position, transform.position);
        //validar si está lo suficientemente cerca
        _jugadorDetectado = distanciaAlJugador <= _radioMovimiento;

        if (_jugadorDetectado)
        {
            //establecer la direccion
            ActualizarOrientacion();
            if (distanciaAlJugador <= _distanciaActivacion)
            {
                AplicarDanoACristian();

                //aplicar efecto de cancelacion de disparo

                if (_puedeBloquear && !_bloqueoActivo && Time.time - _ultimoBloqueo >= _cooldownBloqueo)
                {
                    StartCoroutine(CicloBloqueo());
                }
            }
        }
        else
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
    public void FixedUpdate()
    {
        if (_cristianPosicion == null) return;
        //moverse hacia cristian
        _rigidbody2D.linearVelocity = _direccionMovimiento * _velocidad;
    }
           private void ActualizarOrientacion()
    {
            _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
            transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), _constanteGiroSprite, _constanteGiroSprite);
    }
    private void AplicarDanoACristian()
    {
        if (Time.time - _ultimoDano >= _cooldownDano)
        {
            _cristianScript.Golpe(_dano);
            _ultimoDano = Time.time;


            if (sonidoDaño != null) PlaySound(sonidoDaño);
        }
    }

    private IEnumerator CicloBloqueo()
    {
        _puedeBloquear = false;
        _bloqueoActivo = true;
        _ultimoBloqueo = Time.time;

        
        yield return new WaitForSeconds(_tiempoEsperaCiclo);

        _cristianScript.BloquearDisparos(true);
        
        yield return new WaitForSeconds(_duracionBloqueo);

        _cristianScript.BloquearDisparos(false);
        _bloqueoActivo = false;

        // Espera el cooldown restante (total - duración del bloqueo)
        yield return new WaitForSeconds(_cooldownBloqueo - _duracionBloqueo);
        _puedeBloquear = true;
    }

    void PlaySound(AudioClip clip)
    {
        if (Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    private void RecibirDano()
    {
        _salud--;
        if (_salud <= _saludMin) Destroy(gameObject);//destruir con tiempo para aplciar la animacion } // Método público que llama al privado  
    }
    public void Golpe()
    {
        RecibirDano();

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _distanciaActivacion);

        Gizmos.color = Color.cyan; // Para el nuevo radio de movimiento
        Gizmos.DrawWireSphere(transform.position, _radioMovimiento);
    }
}
