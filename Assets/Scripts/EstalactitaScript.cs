using UnityEngine;

public class EstalactitaScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _dano = 1.5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f; // Empieza suspendida
    }

    public void CaerLuegoDe(float segundos)
    {
        Invoke(nameof(ActivarCaida), segundos);
    }

    private void ActivarCaida()
    {
        _rb.gravityScale = 0.3f; // Activa gravedad para que caiga
    }

    private void OnTriggerEnter2D(Collider2D other)
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
