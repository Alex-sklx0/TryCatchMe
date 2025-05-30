using UnityEngine;

public class DisparoCondicionalScript : MonoBehaviour
{
    public float velocidad;

    public float duracionBloqueoSalto = 2f;
    private float _dano = 1f;
    public AudioClip sonido;

    private Rigidbody2D _rigidbody2D;
    private Vector3 _direccion;
    public Vector3 Direccion
    {
        set
        {
            _direccion = value;

        }
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(sonido);
        
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _direccion * velocidad;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        float duracion1 = 0f;
        float duracion2 = 3f;
        if (other.CompareTag("Player"))
        {
            CristianMovimiento jugador = other.GetComponent<CristianMovimiento>();
            if (jugador != null)
            {
                jugador.BloquearSalto(duracionBloqueoSalto);
                jugador.Golpe(_dano);
            }
            DestruirDisparo(duracion2);
        }
        
            DestruirDisparo(duracion1);
        
    }
     public void DestruirDisparo(float duracion)
    {
        Destroy(gameObject, duracion);
    }
}
