using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoGetterGoblin : MonoBehaviour
{
    //constantes
    private const float _velocidad = 0.3f;
    private const float _dano = 1f;
    //serialize y variables publicas
    [SerializeField] private AudioClip sonido;

    //variables privadas
    private Rigidbody2D _rigidbody2D;
    private Vector3 _direccion;
    //propiedades
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
        if (_direccion != Vector3.zero)
        {
            _rigidbody2D.linearVelocity = _direccion * _velocidad;
        }
            }



    public void DestruirDisparo()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CristianMovimiento cristian = other.GetComponent<CristianMovimiento>();//ya que el disparo puede chocar con mas objetos que cristian
        
        if (cristian != null)
        {
            cristian.Golpe(_dano);
        }
        DestruirDisparo();
    }
}
