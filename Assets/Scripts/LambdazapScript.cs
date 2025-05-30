using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LambdazapScript : MonoBehaviour
{
    public GameObject rayoPrefab; // Prefab del rayo que disparará
    public int anguloEntreRayos = 23; // Ángulo entre cada rayo
    public float fuerzaRayo = 5f; // Velocidad con la que se lanza el rayo
    public float intervaloDisparo = 2f; // Tiempo entre ráfagas
    private float _salud = 4f;
    private float _proximoDisparo;
    public Transform cristian;
    public float rangoDeteccion = 8f;
private bool disparando = false;


   private void Update()
{
     if (cristian == null) return;
        Vector3 direccion = cristian.position - transform.position;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        
    if (!disparando && Time.time >= _proximoDisparo && JugadorEnRango())
        {
            StartCoroutine(DispararRayosSecuencial());
            _proximoDisparo = Time.time + intervaloDisparo;
        }
}
    private bool JugadorEnRango()
{
    if (cristian == null) return false;

    float distancia = Vector2.Distance(transform.position, cristian.position);
    return distancia <= rangoDeteccion;
}


private IEnumerator DispararRayosSecuencial()
{
    disparando = true;

    for (int angulo = 0; angulo < 360; angulo += anguloEntreRayos)
    {
        float radianes = angulo * Mathf.Deg2Rad;
        Vector2 direccion = new Vector2(Mathf.Cos(radianes), Mathf.Sin(radianes)).normalized;

        GameObject rayo = Instantiate(rayoPrefab, transform.position, Quaternion.identity);
        DisparoRayoScript script = rayo.GetComponent<DisparoRayoScript>();
        if (script != null)
        {
            script.velocidad = fuerzaRayo;
            script.Direccion = direccion;
            script.DestruirDisparo(4f);
        }

        rayo.transform.rotation = Quaternion.Euler(0, 0, angulo);
        yield return new WaitForSeconds(0.1f);
    }

    disparando = false;
}

    public void Golpe()
    {
        _salud -= 1;
        if (_salud == 0) Destroy(gameObject);
    }
}
