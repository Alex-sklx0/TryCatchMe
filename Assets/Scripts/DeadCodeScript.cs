using UnityEngine;

public class DeadCode : MonoBehaviour, IDanable
{
    // CONSTANTES 
    private const float Velocidad = 0.3f;
    private const float RadioMovimiento = 0.43f;
    private const float DistanciaAutodestruccion = 0.2f;
    private const float DanoPorAutodestruccion = 0.5f;
    private const float FactorRalentizacion = 0.5f;
    private const float DuracionRalentizacion = 3f;
    private const float ConstanteGiroSprite = 1f;
    private const int SaludMin = 0;
    private static readonly Color ColorRojoTransparente = new Color(1f, 0f, 0f, 0.4f);
    private const float TiempoDestruccionInmediato = 0f;

    //  SERIALIZED y publicos
    [SerializeField] private Transform _cristianPosicion;
    public AudioClip sonido;
    public AudioClip sonidoExplosion;

    // PRIVADAS 
    private CristianMovimiento _cristianScript;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direccionMovimiento;
    private int _salud = 3;
    private bool _jugadorDetectado = false;

    private void Start()
    {
        try
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al obtener Rigidbody2D: {e.Message}", this);
        }

        if (_cristianPosicion == null)
        {
            Debug.LogError("No se asignó la posición de Cristian (Transform).", this);
            return;
        }

        try
        {
            _cristianScript = _cristianPosicion.GetComponent<CristianMovimiento>();
            if (_cristianScript == null)
                Debug.LogError($"No se encontró CristianMovimiento en {_cristianPosicion.name}", this);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al obtener CristianMovimiento: {e.Message}", this);
        }
    }

    private void Update()
    {
        if (_cristianPosicion == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, _cristianPosicion.position);
        _jugadorDetectado = distanciaAlJugador <= RadioMovimiento;

        if (_jugadorDetectado)
        {
            try
            {
                ActualizarOrientacion();

                if (distanciaAlJugador <= DistanciaAutodestruccion)
                {
                    _cristianScript?.AplicarRalentizacion(FactorRalentizacion, DuracionRalentizacion);
                    Autodestruir();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error durante detección o orientación: {e.Message}", this);
            }
        }
        else
        {
            _direccionMovimiento = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_cristianPosicion == null) return;

        try
        {
            _rigidbody2D.linearVelocity = _direccionMovimiento * Velocidad;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error durante movimiento: {e.Message}", this);
        }
    }

    private void ActualizarOrientacion()
    {
        try
        {
            _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
            float direccionX = Mathf.Sign(_direccionMovimiento.x);
            transform.localScale = new Vector3(direccionX, ConstanteGiroSprite, ConstanteGiroSprite);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al actualizar orientación: {e.Message}", this);
        }
    }

    private void Autodestruir()
    {
        try
        {
            _cristianScript?.Golpe(DanoPorAutodestruccion);
            Destroy(gameObject, TiempoDestruccionInmediato);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al autodestruir: {e.Message}", this);
        }
    }

    private void RecibirDano()
    {
        _salud--;

        if (_salud <= SaludMin)
        {
            try
            {
                Destroy(gameObject);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al destruir objeto por daño: {e.Message}", this);
            }
        }
    }

    public void Golpe()
    {
        try
        {
            RecibirDano();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al recibir daño: {e.Message}", this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = ColorRojoTransparente;
        Gizmos.DrawWireSphere(transform.position, RadioMovimiento);
    }
}
