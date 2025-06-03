using UnityEngine;
using System;

public class LogicTrap : MonoBehaviour, IDanable
{
    [Header("Configuración Básica")]
    //Constantes
    public const float TiempoEntreAtaques= 2;
    public const float RadioDeteccion = 0.5f;
    private const string TagJugador = "Player";
    private const float DesplazamientoDisparo = 0.12f;
    private const float ConstanteEjeSprite = 1f; //constante para cuando se gira el srite en direccion y,z


    //serialize y variables Publicas
    [SerializeField] private GameObject _disparoPrefab;
    [SerializeField] private Transform _cristianPosicion;

//variables Privadas
    private int _salud = 3;
    private float _tiempoUltimoAtaque;
    private Vector2 _direccionMovimiento;

    
    private void Start()
    {
        try
        {
            _cristianPosicion = GameObject.FindGameObjectWithTag(TagJugador)?.transform;
           
        }
        catch (Exception e)
        {

            Debug.LogError("Error en Start: " + e.Message);
        }

    }

    private void Update()
    {
    try
        {
        if (_cristianPosicion == null) return;
        ActualizarOrientacion();
        ControlarDisparo();
            
        }
        catch (Exception e)
        {

            Debug.LogError("Error al momento de atacar:" + e.Message);
        }


    }
    
 private void ActualizarOrientacion()
    {
        _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
            transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), ConstanteEjeSprite, ConstanteEjeSprite);
    }
    private void ControlarDisparo()
    {
        // Verificar distancia y tiempo para atacar
            if (PuedeAtacar())
            {
                Disparo();
                _tiempoUltimoAtaque = Time.time;
            }

    }
      private bool PuedeAtacar()
    {
        float distancia = Vector2.Distance(transform.position, _cristianPosicion.position);
        return distancia <= RadioDeteccion && Time.time >= _tiempoUltimoAtaque + TiempoEntreAtaques;
    }
    private void Disparo()
    {
        Vector3 direccion = new Vector3(transform.localScale.x, 0.0f, 0.0f);

        if (_disparoPrefab == null) return;
        try
        {
            GameObject disparo = Instantiate(_disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);

            disparo.GetComponent<DisparoCondicionalScript>().Direccion = direccion;
        }
        catch (Exception e)
        {

            Debug.LogError("Error al disparar:" + e.Message);
        }
        // Configurar dirección al jugador
    }
    

    public void Golpe()
    {
        try
        {
            RecibirDano();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al recibir daño: " + e.Message);
        }

    }

    private void RecibirDano()
    {
        _salud --;

        if (_salud <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Color del radio de ataque (rojo transparente)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, RadioDeteccion);
    }
}