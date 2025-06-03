using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Lambdazap : MonoBehaviour, IDanable
{
    //constantes
    private const int anguloEntreRayos = 23; // Ángulo entre cada rayo
    private const float intervaloDisparo = 2; // Tiempo entre ráfagas
    private const int SaludMin = 0;
    public const float RadioDeteccion = 0.4f;


    private const float ConstanteEjeSprite = 1f; //constante para cuando se gira el srite en direccion y,z
    private const int AnguloCompleto = 360;
    private const int AnguloMinimo = 0;

    //serialize y variables publicas 
    [SerializeField] private GameObject _rayoPrefab; // Prefab del rayo que disparará
    [SerializeField] private Transform _cristianPosicion;
    //variables privadas
    private float _salud = 4f;
    private float _proximoDisparo;
    private bool _disparando = false;
    private Vector2 _direccionMovimiento;

    private void Update()
    {
        if (_cristianPosicion == null) return;

        ActualizarOrientacion();
        ControlarDisparo();


    }
    private void ActualizarOrientacion()
    {
        _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
        transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), ConstanteEjeSprite, ConstanteEjeSprite);
    }
    private bool JugadorEnRango()
    {
        if (_cristianPosicion == null) return false;

        float distancia = Vector2.Distance(transform.position, _cristianPosicion.position);
        return distancia <= RadioDeteccion;
    }

    private void ControlarDisparo()
    {
        if (!_disparando && Time.time >= _proximoDisparo && JugadorEnRango())
        {
            StartCoroutine(DispararRayosSecuencial());
            _proximoDisparo = Time.time + intervaloDisparo;
        }
    }
    private IEnumerator DispararRayosSecuencial()
    {
        _disparando = true;

        for (int angulo = AnguloMinimo; angulo < AnguloCompleto; angulo += anguloEntreRayos)
        {
            float radianes = angulo * Mathf.Deg2Rad;
            Vector2 direccion = new Vector2(Mathf.Cos(radianes), Mathf.Sin(radianes)).normalized;

            GameObject rayo = Instantiate(_rayoPrefab, transform.position, Quaternion.identity);
            DisparoRayoScript rayoScript = rayo.GetComponent<DisparoRayoScript>();
            if (rayoScript != null)
            {
                rayoScript.Direccion = direccion;
            }

            rayo.transform.rotation = Quaternion.Euler(0, 0, angulo);
            yield return new WaitForSeconds(0.1f);
        }

        _disparando = false;
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
  void OnDrawGizmosSelected()
    {
        // Color del radio de ataque (rojo transparente)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, RadioDeteccion);
    }
}

