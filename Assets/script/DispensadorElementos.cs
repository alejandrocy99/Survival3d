using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DispensadorElementos : MonoBehaviour
{

    //metodo para dispensar un prefabs mediantye un input map
    public GameObject prefabElemento1;//
    public GameObject prefabElemento2;//
    public GameObject prefabElemento3;//
    public Transform puntoDeGeneracion; // Punto donde se generará el prefab

    



    public void OnGenerarElemento1() {


        Instantiate(prefabElemento1, puntoDeGeneracion.position, puntoDeGeneracion.rotation); // Genera el prefab en la posición y rotación del punto de generación


    }

    public void OnGenerarElemento2() {


        Instantiate(prefabElemento2, puntoDeGeneracion.position, puntoDeGeneracion.rotation); // Genera el prefab en la posición y rotación del punto de generación


    }

    public void OnGenerarElemento3() {


        Instantiate(prefabElemento3, puntoDeGeneracion.position, puntoDeGeneracion.rotation); // Genera el prefab en la posición y rotación del punto de generación


    }


    public void OnGenerarElemento1(InputAction.CallbackContext context)
    {
        if (context.performed) // Verifica si la acción fue realizada
        {
            OnGenerarElemento1();// Llama al método para generar el elemento
        }
    }
    public void OnGenerarElemento2(InputAction.CallbackContext context)
    {
        if (context.performed) // Verifica si la acción fue realizada
        {
            OnGenerarElemento2(); // Llama al método para generar el elemento
        }
    }
    public void OnGenerarElemento3(InputAction.CallbackContext context)
    {
        if (context.performed) // Verifica si la acción fue realizada
        {
            OnGenerarElemento3(); // Llama al método para generar el elemento
        }
    }

}
