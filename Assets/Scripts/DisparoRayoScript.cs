using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoRayoScript : MonoBehaviour
{
    private const float Velocidad = 0.3f;
    private const float Dano = 1f;
    private const float TiempoVida = 0.5f;
    private const string TagJugador = "Player";

    [SerializeField] private AudioClip sonido;
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
        ObtenerComponentes();


    }
    private void ObtenerComponentes()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(sonido);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _direccion * Velocidad;
    }

    private void Autodestruir(float TiempoVida)
    {
        Destroy(gameObject, TiempoVida); //se destruye el objeto despues de un tiempo

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag(TagJugador))//cuando colisiona con cristian, que tiene el tag "Player" 
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(Dano);//confirmar si se pueden acceder a los metodos y propiedades de cristian
            Destroy(gameObject);
        }
        Autodestruir(TiempoVida);
    }
}
