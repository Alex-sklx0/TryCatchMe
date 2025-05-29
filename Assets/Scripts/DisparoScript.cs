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
        NullieScript nullie = other.GetComponent<NullieScript>();
        SpaghettiCodeBoss spaghettiCodeBoss = other.GetComponent<SpaghettiCodeBoss>();

        OverengineeringScript overengineering = other.GetComponent<OverengineeringScript>();
        UnderengineeringScript underengineering = other.GetComponent <UnderengineeringScript>();
        InheritrixScript inheritrix = other.GetComponent<InheritrixScript>();
        GodObjectScript godObject = other.GetComponent<GodObjectScript>();
            if (other.CompareTag("Player")) return; // <-- Ignora al jugador
         if (other.CompareTag("Disparo"))
        {
            Destroy(gameObject);           // Destruye este disparo
            Destroy(other.gameObject);     // Destruye el otro disparo también (si quieres)
            return;                        // Sal del método para no seguir golpeando enemigos
        }

        if (getterGoblin != null)
        {
            getterGoblin.Golpe();
        }
        if (deadCodeScript != null)
        {
            deadCodeScript.Golpe();
        }
         if (nullie != null)
        {
            nullie.Golpe();
        }
       if (spaghettiCodeBoss != null)
        {
            spaghettiCodeBoss.Golpe();
        }
        if (overengineering != null)
        {
            overengineering .Golpe();
        }
        if (underengineering != null)
        {
            underengineering .Golpe();
        }
        if (inheritrix != null)
        {
            inheritrix.Golpe();
        }
        if (godObject != null)
        {
            godObject.Golpe();
        }

        DestruirDisparo();
    }
}
