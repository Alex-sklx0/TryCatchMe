using UnityEngine;

public class DisparoAlbondiga : MonoBehaviour
{
    //constantes
    private const float EscalaGravedadReducida = 0.3f;
    private const float TiempoVida = 5f;
    private const float Dano = 1f;
    private const string TagJugador = "Player";
    private const string TagSuelo = "Ground";

    //variables privadas
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        ObtenerComponentes();
        Autodestruir();
    }
      public void ObtenerComponentes()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = EscalaGravedadReducida; // se modifica la gravedad para que caiga mas lento    }
    }
    private void Autodestruir()
    {
        Destroy(gameObject, TiempoVida); //se destruye el objeto despues de un tiempo

    }
    public void LanzarDisparo(Vector2 direccion, float fuerza)
    {
        RedirigirDisparo(direccion, fuerza);
    }
    private void RedirigirDisparo(Vector2 direccion, float fuerza)
    {
        _rigidbody2D = _rigidbody2D != null ? _rigidbody2D : GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
    }
    // Se llama desde el jefe con dirección ajustada hacia el jugador

  
    //cuando colisiona con algo
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagJugador))//cuando colisiona con cristian, que tiene el tag "Player" 
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(Dano);//confirmar si se pueden acceder a los metodos y propiedades de cristian
            Destroy(gameObject);
        }
        else if (other.CompareTag(TagSuelo))//cuando choca con el suelo se destruye
        {
            Destroy(gameObject);
        }
    }
}

