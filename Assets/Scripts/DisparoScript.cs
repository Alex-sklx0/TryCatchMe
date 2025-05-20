using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoScript : MonoBehaviour
{
    public float velocidad;
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



    public void DestruirDisparo()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DeadCodeScript deadCodeScript = other.GetComponent<DeadCodeScript>();
        GetterGoblinScript getterGoblin = other.GetComponent<GetterGoblinScript>();
        if (getterGoblin != null)
        {
            getterGoblin.Golpe();
        }
        if (deadCodeScript != null)
        {
            deadCodeScript.Golpe();
        }
       
        DestruirDisparo();
    }
}
