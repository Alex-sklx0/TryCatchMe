    using UnityEngine;

    public class AlbondigaScript : MonoBehaviour
    {
        [Header("Configuración")]
        public float fuerza ;
        public float tiempoVida;
        private float _dano;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _rb.gravityScale = 0.3f; // Permite que la albóndiga caiga en arco
            Destroy(gameObject, tiempoVida);
        }

        // Se llama desde el jefe con dirección ajustada hacia el jugador
       public void Inicializar(Vector2 direccion, float fuerza)
{
    if (_rb == null) _rb = GetComponent<Rigidbody2D>();

    if (_rb != null)
    {
        _rb.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
    }
}

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<CristianMovimiento>()?.Golpe(_dano);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
