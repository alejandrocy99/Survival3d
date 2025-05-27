using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//la interfaz es una clase abstrata con metodos sin definir que se implementan en otras clases
//la interfaz es una clase que no se puede instanciar, pero se puede heredar

public class SetaVenenosa : MonoBehaviour
{
    public float cantidadVeneno; // Cantidad de veneno que aplica la seta
    public float indiceDeterioro; // Indice de deterioro que aplica la seta
    private List<IDeterioro> objetosColisionables = new List<IDeterioro>(); // Lista de objetos que colisionan con la seta y sufren deterioro

    void Start()
    {
        StartCoroutine( ManejarDeterioro()); // Inicia la corrutina para manejar el deterioro
    }

    IEnumerator ManejarDeterioro()
    {
        while(true){ // Bucle infinito para aplicar el deterioro continuamente
            for(int i = 0; i < objetosColisionables.Count; i++) // Recorre la lista de objetos colisionables
            {
                objetosColisionables[i].ProduccirDeterioro(cantidadVeneno ); // Aplica el deterioro a cada objeto
            }
            yield return new WaitForSeconds(1f); // Espera 1 segundo
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<IDeterioro>() != null) // Comprueba si el objeto que colisiona implementa la interfaz IDeterioro
        {
            objetosColisionables.Add(collision.gameObject.GetComponent<IDeterioro>()); // Añade el objeto a la lista de objetos colisionables

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDeterioro>() != null) // Comprueba si el objeto que colisiona implementa la interfaz IDeterioro
        {
            objetosColisionables.Remove(collision.gameObject.GetComponent<IDeterioro>()); // Elimina el objeto de la lista de objetos colisionables
            
        }
    }
}
