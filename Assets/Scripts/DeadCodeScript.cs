using UnityEngine;

public class DeadCode : MonoBehaviour, IDanable
{
    //constantes
    private const float _velocidad = 0.3f;
    private const float _radioMovimiento = 0.43f;
    private const float _distanciaAutodestruccion = 0.2f;
    private const float _danoPorAutodestruccion = 0.5f;
    private const float _factorRalentizacion = 0.5f; //porcentaje que va a reducir la velocidad del jugador
    private const float _duracionRalentizacion = 3f; //3 segundos
    private const float _constanteGiroSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const int _saludMin = 0;


    //serializados y variables publicos 
    [SerializeField] private Transform _cristianPosicion; //SerializeField para que se pueda encapsular y aun así mostrar en el inspector de unity

    public AudioClip sonido;
    public AudioClip sonidoExplosion;

    //variables privados
    private CristianMovimiento _cristianScript;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direccionMovimiento;
    private int _salud = 3;
    private bool _jugadorDetectado = false;



    void Start()
    {
        // Obtener el Rigidbody2D del GameObject
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // Verificar si el Transform del jugador (Cristian) fue asignado en el Inspector
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


    void Update()
    {
        //verificar en cada frame si cristian existe en la escena
        if (_cristianPosicion == null) return;
        //distancia del jugador en cada frame
        float distanciaAlJugador = Vector2.Distance(transform.position, _cristianPosicion.position);

        //Activar movimiento si el jugador entra en el radio de deteccion
        _jugadorDetectado = distanciaAlJugador <= _radioMovimiento;

        if (_jugadorDetectado)
        {
            
            ActualizarOrientacion();
            // Verificar distancia para autodestrucción
            if (distanciaAlJugador <= _distanciaAutodestruccion)
            {
                //aplicar metodo realentizador a cristian y autodestruirse
                _cristianScript.AplicarRalentizacion(_factorRalentizacion, _duracionRalentizacion);
                Autodestruir();
            }
        }
        else _direccionMovimiento = Vector2.zero; // No moverse si no detecta a cristian

    }

    void FixedUpdate()
    {
        if (_cristianPosicion == null) return;
        //moverse hacia cristian
        _rigidbody2D.linearVelocity = _direccionMovimiento * _velocidad;
    }
    private void ActualizarOrientacion()
    {
        //direccion de movimiento en base a la posicion de cristian

        _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
        // Girar el sprite en direccion a cristian

        transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), _constanteGiroSprite, _constanteGiroSprite);
    }
    void Autodestruir()
    {
        _cristianScript.Golpe(_danoPorAutodestruccion);//aplicar dano a cristian
        Destroy(gameObject); //destruir con tiempo para aplciar la animacion
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
    void OnDrawGizmosSelected()
    {
        // Color del radio de ataque (rojo transparente)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, _radioMovimiento);
    }
}
