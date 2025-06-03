using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoGetterGoblin : MonoBehaviour
{
    //constantes
    private const float Velocidad = 0.3f;
    private const float Dano = 1f;
    //serialize y variables publicas
    [SerializeField] private AudioClip sonido;

    //variables privadas
    private CristianMovimiento _cristianScript;

    private Rigidbody2D _rigidbody2D;
    private Vector3 _direccion;
    //propiedades
    public Vector3 Direccion
    {
        set
        {
            _direccion = value;

        }
        get
        {
            return _direccion;
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
        transform.position += Direccion * Velocidad * Time.deltaTime;
        }
            }



    public void DestruirDisparo()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _cristianScript))
        {
            _cristianScript.Golpe(Dano);
            DestruirDisparo();
        }
        else if (!collision.isTrigger || collision.CompareTag("Disparo"))
        {
            DestruirDisparo();
        }
    }
}
