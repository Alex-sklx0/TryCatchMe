using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void BtnJugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void BtnSalir()
    {
        Console.WriteLine("Saliendo del juego..");
        Application.Quit();
    }


}
