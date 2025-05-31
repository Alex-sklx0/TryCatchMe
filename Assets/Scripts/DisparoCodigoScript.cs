using UnityEngine;

public class ChorroDisparoScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _dano = 1f;
    private float _tiempoVida = 4f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0.3f; // Aplica gravedad para que caiga como proyectil
        Destroy(gameObject, _tiempoVida);
    }

    public void Inicializar(Vector2 direccion, float fuerza)
    {
        _rb = _rb != null ? _rb : GetComponent<Rigidbody2D>();
        _rb.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("DisparoCrashtian")) return; // <-- Ignora al propio disparo 

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
