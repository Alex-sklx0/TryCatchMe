    using UnityEngine;

    public class AlbondigaScript : MonoBehaviour
    {
        [Header("Configuración")]
        private float _tiempoVida = 5f;
        private float _dano=1f;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _rb.gravityScale = 0.3f; // se modifica la gravedad para que caiga mas lento
            Destroy(gameObject, _tiempoVida); //se destruye el objeto despues de un tiempo
        }

    // Se llama desde el jefe con dirección ajustada hacia el jugador
    public void Inicializar(Vector2 direccion, float fuerza)
    {
        _rb = _rb != null ? _rb : GetComponent<Rigidbody2D>();
        _rb.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
    
    
}
//cuando colisiona con algo
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))//cuando colisiona con cristian, que tiene el tag "Player" 
            {
                other.GetComponent<CristianMovimiento>()?.Golpe(_dano);//confirmar si se pueden acceder a los metodos y propiedades de cristian
                Destroy(gameObject);
            }
            else if (other.CompareTag("Ground"))//cuando choca con el suelo se destruye
            {
                Destroy(gameObject);
            }
        }
    }
