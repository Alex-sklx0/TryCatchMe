using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetterGoblin : MonoBehaviour, IDanable
{
    //constantes
    private const float CooldownDisparo = 1.25f;
    private const float ConstanteDireccionDisparo = 0.0f; //constante para cuando genera un disparo en direccion y,z
    private const float ConstanteHorizontalDisparo = 0.12f; //constante para cuando genera un disparo en direccion y,z

    private const float ConstanteGiroSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const int SaludMin = 0;
    private const float RangoDisparo = 0.8f;
        
    //serialize y variables publicas
    [SerializeField] private Transform _cristianPosicion;
    [SerializeField] private GameObject _disparoPrefab;
    //variables privadas
    private float _ultimoTiro;
    private  int _salud = 3;
    private Vector2 _direccionMovimiento;

  private void Start()
    {

        if (_cristianPosicion == null)
        {
            Debug.LogError($"{name}: No se asignó la posición de Cristian.", this);
            return;
        }

        if (_disparoPrefab == null)
        {
            Debug.LogError($"{name}: No se asignó el prefab de disparo.", this);
        }
    }

    private void Update()
    {
        if (_cristianPosicion == null) return;

        ActualizarOrientacion();
        ControlarDisparo();
    }
       private void ActualizarOrientacion()
    {
            _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
            transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), ConstanteGiroSprite, ConstanteGiroSprite);
    }

    private void ControlarDisparo()
    {
        float distanciaAlJugador = Vector3.Distance(_cristianPosicion.position, transform.position);
        if (distanciaAlJugador <= RangoDisparo && Time.time > _ultimoTiro + CooldownDisparo)
        {
            Disparar();
            _ultimoTiro = Time.time;
        }
    }

    private void Disparar()
    {
        Vector3 direccion = new Vector3(transform.localScale.x, ConstanteDireccionDisparo, ConstanteDireccionDisparo);
        GameObject disparo = Instantiate(_disparoPrefab, transform.position + direccion * ConstanteHorizontalDisparo, Quaternion.identity);
        disparo.GetComponent<DisparoGetterGoblin>().Direccion = direccion;
    }

  private void RecibirDano()
    {
        _salud--;
        if (_salud <= SaludMin) Destroy(gameObject);//destruir con tiempo para aplciar la animacion } // Método público que llama al privado  
    }
    public void Golpe()
    {
        RecibirDano();

    }
    
}
