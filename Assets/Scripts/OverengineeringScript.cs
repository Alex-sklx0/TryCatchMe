using UnityEngine;

public class OverengineeringScript : MonoBehaviour
{
    public float velocidad ;
    public float radioAtaque;
    public float tiempoEntreAtaques = 2f;
    private float _dano = 1f;
    private float _salud = 8;
    private float tiempoAtaqueActual = 0f;
    private Transform objetivo;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        GameObject jugador = GameObject.FindWithTag("Player");

        if (jugador != null)
        {
            objetivo = jugador.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        // Movimiento hacia el jugador
        Vector2 direccion = (objetivo.position - transform.position).normalized;
        rb.MovePosition(rb.position + direccion * velocidad * Time.deltaTime);

        // Si está dentro del radio de ataque, intentar atacar
        if (distancia <= radioAtaque)
        {
            tiempoAtaqueActual -= Time.deltaTime;
            if (tiempoAtaqueActual <= 0f)
            {
                Atacar();
                tiempoAtaqueActual = tiempoEntreAtaques;
            }

            if (anim) anim.SetBool("atacando", true);
        }
        else
        {
            if (anim) anim.SetBool("atacando", false);
        }

        // Voltear al enemigo según la posición del jugador
        if (objetivo.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1); // mira a la derecha
        else
            transform.localScale = new Vector3(-1, 1, 1); // mira a la izquierda
    }

    void Atacar()
    {
        if (anim) anim.SetTrigger("atacar");

        float distancia = Vector2.Distance(transform.position, objetivo.position);
        if (distancia <= radioAtaque)
        {
            CristianMovimiento jugador = objetivo.GetComponent<CristianMovimiento>();
            if (jugador != null)
            {
                jugador.Golpe(_dano); // Asegúrate de que este método exista
            }
        }
    }
    void OnDrawGizmosSelected()
{
    // Color del radio de ataque (rojo transparente)
    Gizmos.color = new Color(1, 0, 0, 0.4f);
    Gizmos.DrawWireSphere(transform.position, radioAtaque);
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
