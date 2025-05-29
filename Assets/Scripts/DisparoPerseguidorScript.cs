using UnityEngine;

public class DisparoPerseguidorScript : MonoBehaviour
{
    private Transform objetivo;
    public float velocidad ;
    public float tiempoVida ;

    public void Iniciar(Transform jugador)
    {
        objetivo = jugador;
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (objetivo == null) return;

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) return; 

        if (other.CompareTag("Player"))
        {
            // Aplica daño
            CristianMovimiento jugador = other.GetComponent<CristianMovimiento>();
            if (jugador != null)
            {
                jugador.Golpe(0.5f); // o el daño que prefieras
            }

            Destroy(gameObject);
        }
    }
}

