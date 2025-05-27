using UnityEngine;

public class QuesoScript : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad ;    
    public float tiempoVida = 4f;
    
    private Rigidbody2D _rb;

    public void Inicializar(Vector3 posicionJugador)
    {
        _rb = GetComponent<Rigidbody2D>();
        Vector2 direccion = (posicionJugador - transform.position).normalized;
        _rb.linearVelocity = direccion * velocidad;
        
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CristianMovimiento>().AplicarStun();
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}