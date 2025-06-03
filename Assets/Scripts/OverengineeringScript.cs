using UnityEngine;

public class OverengineeringScript : MonoBehaviour, IDanable
{
    // Constantes
    private const float Velocidad = 0.3f;
    private const float RadioAtaque = 0.25f;
    private const float RadioMovimiento = 0.8f;
    private const float TiempoEntreAtaques = 2f;
    private const float Dano = 1f;
    private const float ConstanteGiroSprite = 1f;
    private const int SaludInicial = 8;
    private const int SaludMinima = 0;

    // Componentes y referencias
    private Transform _cristianPosicion;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    // Variables privadas
    private float _salud = SaludInicial;
    private float _tiempoAtaqueActual;
    private Vector2 _direccionMovimiento;
    private bool _jugadorEnRango = false;

    private void Start()
    {
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            _cristianPosicion = jugador.transform;
        }
        else
        {
            Debug.LogError("No se encontró al jugador en la escena.");
        }

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_cristianPosicion == null) return;

        float distancia = Vector2.Distance(transform.position, _cristianPosicion.position);
        _jugadorEnRango = distancia <= RadioMovimiento;

        if (_jugadorEnRango)
        {
            ActualizarOrientacion();
        }

        if (distancia <= RadioAtaque)
        {
            _tiempoAtaqueActual -= Time.deltaTime;

            if (_tiempoAtaqueActual <= 0f)
            {
                Atacar();
                _tiempoAtaqueActual = TiempoEntreAtaques;
            }

            _animator?.SetBool("atacando", true);
        }
        else
        {
            _animator?.SetBool("atacando", false);
        }
    }

    private void FixedUpdate()
    {
        if (_cristianPosicion == null || !_jugadorEnRango) return;

        _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direccionMovimiento * Velocidad * Time.fixedDeltaTime);
    }

    private void ActualizarOrientacion()
    {
        float direccionX = Mathf.Sign(_cristianPosicion.position.x - transform.position.x);
        transform.localScale = new Vector3(-direccionX, ConstanteGiroSprite, ConstanteGiroSprite);
    }

    private void Atacar()
    {

        if (_cristianPosicion.TryGetComponent(out CristianMovimiento cristianScript))
        {
            cristianScript.Golpe(Dano);
        }
    }

    public void Golpe()
    {
        _salud--;
        if (_salud <= SaludMinima)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // Radio de ataque
        Gizmos.DrawWireSphere(transform.position, RadioAtaque);

        Gizmos.color = new Color(0f, 1f, 0f, 0.4f); // Radio de movimiento
        Gizmos.DrawWireSphere(transform.position, RadioMovimiento);
    }
}
