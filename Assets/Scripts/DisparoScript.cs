using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoScript : MonoBehaviour
{
    private const float _velocidad = 0.5f;
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




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (DebeIgnorarColision(other)) return;

        if (EsColisionConOtroDisparo(other))
        {
            DestruirDisparo();
            return;
        }

        ProcesarImpacto(other);
        DestruirDisparo();


        // DeadCode deadCodeScript = other.GetComponent<DeadCode>();
        // GetterGoblin getterGoblin = other.GetComponent<GetterGoblin>();
        // Nullie nullie = other.GetComponent<Nullie>();
        // SpaghettiCodeBoss spaghettiCodeBoss = other.GetComponent<SpaghettiCodeBoss>();

        // OverengineeringScript overengineering = other.GetComponent<OverengineeringScript>();
        // UnderengineeringScript underengineering = other.GetComponent<UnderengineeringScript>();
        // InheritrixScript inheritrix = other.GetComponent<InheritrixScript>();
        // GodObjectScript godObject = other.GetComponent<GodObjectScript>();

        // LogicTrapScript logicTrap = other.GetComponent<LogicTrapScript>();
        // RecurseScript recurse = other.GetComponent<RecurseScript>();
        // LambdazapScript lambdazap = other.GetComponent<LambdazapScript>();
        // CrashtianScript crashtian = other.GetComponent<CrashtianScript>();

        // if (other.CompareTag("Disparo"))
        // {
        //     Destroy(gameObject);           // Destruye este disparo
        //     Destroy(other.gameObject);     // Destruye el otro disparo también (si quieres)
        //     return;                        // Sal del método para no seguir golpeando enemigos
        // }

        // //nivel 1
        // getterGoblin?.Golpe();
        // deadCodeScript?.Golpe();
        // nullie?.Golpe(); // Usa el método público  
        // spaghettiCodeBoss?.Golpe();

        // //nivel 2    
        // overengineering?.Golpe();
        // underengineering?.Golpe();
        // inheritrix?.Golpe();
        // godObject?.Golpe();

        // //nivel 3
        // logicTrap?.Golpe();
        // recurse?.Golpe();
        // lambdazap?.Golpe();
        // crashtian?.Golpe();

        // DestruirDisparo();
    }
    private bool DebeIgnorarColision(Collider2D other)
    {
        // Ignora colisiones con el jugador y ciertas capas
        return other.CompareTag("Player");
    }
    private bool EsColisionConOtroDisparo(Collider2D other)
    {
        return other.CompareTag("Disparo");
    }
    private void ProcesarImpacto(Collider2D other)
    {
        // Busca cualquier componente que implemente IDaniable
        IDanable enemigo = other.GetComponent<IDanable>();
        enemigo?.Golpe();
    }
    public void DestruirDisparo()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        DestruirDisparo();
    }
}
// Interface para objetos que pueden recibir daño
public interface IDanable
{
    void Golpe();
}
