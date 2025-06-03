using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGameover : MonoBehaviour
{
    
    [SerializeField] private GameObject _btnPausa;
    [SerializeField] private GameObject _vidaCristian;
    public GameObject gameOverPanel;
    //Pantalla Gameover


    public void MostrarGameOver()
    {
        StartCoroutine(MostrarGameOverConRetraso());
    }

    private IEnumerator MostrarGameOverConRetraso()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        try
        {

            gameOverPanel.SetActive(true);       // Ahora sí lo muestra

            //Oculta el boton Pausa y la Vida
            _btnPausa.SetActive(false);

            _vidaCristian.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar mostrar la pantalla Gameover despues de 5 seg: {e.Message}");

        }


    }


}
