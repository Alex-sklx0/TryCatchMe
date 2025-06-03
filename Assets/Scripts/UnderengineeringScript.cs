using UnityEngine;

public class UnderengineeringScript : MonoBehaviour, IDanable
{
    private const float Velocidad = 0.5f;
    private const float RadioAtaque = 0.12f;  //Radio para activar el movimiento
    private const float RadioMovimiento = 0.6f;

    private const float TiempoEntreAtaques = 1f; // Tiempo que tarda entre un ataque y otro
    private const int TiempoMinimoAtaque = 0;
    private const float Dano = 1f;  // Daño ejercido
    private const float ConstanteGiroSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const int SaludMin = 0;
    

    private float _salud = 4;
private float _tiempoAtaqueActual = 0f;
    private Transform _cristianPosicion;
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    private Vector2 _direccionMovimiento;



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
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_cristianPosicion == null) return;

        float distancia = Vector2.Distance(transform.position, _cristianPosicion.position);

        // Movimiento y orientación solo si está dentro del rango de movimiento
        if (distancia <= RadioMovimiento)
        {
            ActualizarOrientacion();
        }

        // Si está dentro del rango de ataque
        if (distancia <= RadioAtaque)
        {
            _tiempoAtaqueActual -= Time.deltaTime;

            if (_tiempoAtaqueActual <= TiempoMinimoAtaque)
            {
                Atacar();
                _tiempoAtaqueActual = TiempoEntreAtaques;
            }
        }
    }
        private void FixedUpdate()
    {
        if (_cristianPosicion == null) return;
        //moverse hacia cristian
        _rigidbody2D.linearVelocity = _direccionMovimiento * Velocidad;
    }
    

    private void ActualizarOrientacion()
    {
        _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
        float direccionX = Mathf.Sign(_direccionMovimiento.x);
        transform.localScale = new Vector3(direccionX, ConstanteGiroSprite, ConstanteGiroSprite);
    }

    private void Atacar()
    {
        if (_cristianPosicion.TryGetComponent(out CristianMovimiento cristianScript))
        {
            cristianScript.Golpe(Dano);
        }
    }


    private void RecibirDano()
    {
        _salud--;
        if (_salud <= SaludMin) Destroy(gameObject);//destruir con tiempo para aplciar la animacion } // Método público que llama al privado  
    }
    public void Golpe()
    {
        RecibirDano();

    }
    private void OnDrawGizmosSelected()
    {
        // Color del radio de ataque (rojo transparente)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, RadioAtaque);
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, RadioMovimiento);
        
    }
}