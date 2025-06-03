using UnityEngine;

public class DisparoPerseguidorScript : MonoBehaviour
{
    private Transform objetivo;
    private const float Velocidad = 0.44f;
    private const int TiempoVida = 5 ;
    private const float Dano = 0.5f;
    private CristianMovimiento _cristianScript;



    public void Iniciar(Transform jugador)
    {
        objetivo = jugador;
        DestruirDisparo(TiempoVida);
    }
  private void DestruirDisparo(int TiempoVida)
    {
        Destroy(gameObject, TiempoVida);
    }
    private void Update()
    {
        if (objetivo == null) return;
        MoverDisparo();
        
    }
    private void MoverDisparo()
    {
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * Velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _cristianScript))
        {
            _cristianScript.Golpe(Dano);
            DestruirDisparo(TiempoVida-TiempoVida);
        }
        else if (collision.CompareTag("Disparo"))
        {
            DestruirDisparo(TiempoVida-TiempoVida);
        }
    }
}

