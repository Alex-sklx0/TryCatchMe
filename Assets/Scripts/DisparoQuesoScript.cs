using UnityEngine;

public class DisparoQueso : MonoBehaviour
{
    //constantes
    private const float Velocidad = 0.3f;    
    private const float TiempoVida = 6f;
    private const string TagJugador = "Player";

    //variables privadas
    private Rigidbody2D _rb;

   
    private void ObtenerComponentes()
    {
        _rb = GetComponent<Rigidbody2D>();
       
    }
    public void LanzarDisparo(Vector3 posicionJugador)
    {
        ObtenerComponentes();
        RedirigirDisparo(posicionJugador);
        Autodestruir();
    }
    private void RedirigirDisparo(Vector3 posicionJugador)
    {
         Vector2 direccion = (posicionJugador - transform.position).normalized;
        _rb.linearVelocity = direccion * Velocidad;
    } 
      private void Autodestruir()
    {
        Destroy(gameObject, TiempoVida); //se destruye el objeto despues de un tiempo

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagJugador))
        {
            other.GetComponent<CristianMovimiento>().AplicarAtoramiento();
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}