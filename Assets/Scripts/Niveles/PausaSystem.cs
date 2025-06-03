using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaSystem : MonoBehaviour
{
    //Privadas
    [SerializeField] private GameObject _btnPausa;
    [SerializeField] private GameObject _menuPausa;

    //Publicas
    public float EscalaDeTiempo = Time.timeScale;

    public void Pausar()
    {
        try
        {

            EscalaDeTiempo = 0f;
            _btnPausa.SetActive(false);
            _menuPausa.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al pausar el juego: {e.Message}");
        }
    }

    public void Reanudar()
    {
        try
        {
            if (_btnPausa == null || _menuPausa == null)
                throw new NullReferenceException("No existen _btnPausa o _menuPausa.");

            EscalaDeTiempo = 1f;
            _btnPausa.SetActive(true);
            _menuPausa.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al reanudar el juego: {e.Message}");
        }
    }

    public void Reiniciar()
    {
        try
        {
            EscalaDeTiempo = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al reiniciar: {e.Message}");

        }

    }

    public void Salir()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); //LLeva al menú pricipal
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir al Menú principal: {e.Message}");

        }
    }

}
