using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//la interfaz es una clase abstrata con metodos sin definir que se implementan en otras clases
//la interfaz es una clase que no se puede instanciar, pero se puede heredar

public class SetaVenenosa : MonoBehaviour
{
    public float cantidadVeneno;
    public float indiceDeterioro;
    private List<IDeterioro> objetosColisionables = new List<IDeterioro>();

    void Start()
    {
        StartCoroutine( ManejarDeterioro());
    }

    IEnumerator ManejarDeterioro()
    {
        while(true){
            for(int i = 0; i < objetosColisionables.Count; i++)
            {
                objetosColisionables[i].ProduccirDeterioro(cantidadVeneno );
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<IDeterioro>() != null)
        {
            objetosColisionables.Add(collision.gameObject.GetComponent<IDeterioro>());

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDeterioro>() != null)
        {
            objetosColisionables.Remove(collision.gameObject.GetComponent<IDeterioro>());
            
        }
    }
}
