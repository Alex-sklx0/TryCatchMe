using System.Collections;
using UnityEngine;

public class GodObjectScript : MonoBehaviour
{
    public Transform cristian; // El jugador
    public GameObject DisparoPerseguidorPrefab;
    public GameObject explosionSueloPrefab;
    public GameObject ataqueTechoPrefab;
    public GameObject indicadorPrefab; // asigna en el inspector

    public Transform[] puntosSuelo; // Objetos vacíos en el suelo
    public Transform[] puntosTecho; // Objetos vacíos en el techo

    private float tiempoEntreAtaques = 4f;
    private float proximoAtaque;
    private int _salud = 5;


    void Update()
    {
        if (Time.time >= proximoAtaque)
        {
            int ataque = Random.Range(1, 4); // 1, 2 o 3
            switch (ataque)
            {
                case 1:
                    LanzarPerseguidores();
                    break;
                case 2:
                    ExplosionDesdeSuelo();
                    break;
                case 3:
                    AtaqueDesdeTecho();
                    break;
            }

            proximoAtaque = Time.time + tiempoEntreAtaques;
        }
    }

    void LanzarPerseguidores()
    {

        GameObject p = Instantiate(DisparoPerseguidorPrefab, transform.position, Quaternion.identity);
        p.GetComponent<DisparoPerseguidorScript>().Iniciar(cristian);

    }

   
void ExplosionDesdeSuelo()
{
    foreach (Transform punto in puntosSuelo)
    {
        StartCoroutine(MostrarIndicadorYEjecutar(punto, explosionSueloPrefab));
    }
}

void AtaqueDesdeTecho()
{
    foreach (Transform punto in puntosTecho)
    {
        StartCoroutine(MostrarIndicadorYEjecutar(punto, ataqueTechoPrefab));
    }
}

IEnumerator MostrarIndicadorYEjecutar(Transform punto, GameObject prefabAtaque)
{
    GameObject indicador = Instantiate(indicadorPrefab, punto.position, Quaternion.identity);

    yield return new WaitForSeconds(1f); // tiempo para esquivar

    Destroy(indicador);
    Instantiate(prefabAtaque, punto.position, Quaternion.identity);
}
     public void Golpe()
    {
        _salud -= 1;
        if (_salud <= 0) Destroy(gameObject);
    }

}
