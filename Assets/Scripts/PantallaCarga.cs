using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaCarga : MonoBehaviour
{
    // Tiempo de espera en segundos antes de cargar la escena principal
    public float tiempoEspera = 2f;
    // Nombre de la escena principal
    public string nombreEscenaPrincipal = "Principal";

    void Start()
    {
        // Comienza la rutina de carga
        StartCoroutine(CargarEscenaPrincipal());
    }

    IEnumerator CargarEscenaPrincipal()
    {
        // Espera el tiempo de espera
        yield return new WaitForSeconds(tiempoEspera);

        // Carga la escena principal
        SceneManager.LoadScene(nombreEscenaPrincipal);
    }
}