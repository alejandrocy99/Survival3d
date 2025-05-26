using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlInteractuable : MonoBehaviour
{
    private Camera cam;

    public float TiemporChequeo = 0.5f; // Tiempo entre chequeos
    public float ultimoTiempoChequeo; // Tiempo del último chequeo
    public LayerMask capaInteractuable; // Mascara para el raycast

    private GameObject objetoInteractuable; // Objeto que se va a interactuar
    private IInteractuable interactuable; // Interfaz para la interacción

    public TextMeshProUGUI textoInteraccion; // Texto que muestra el mensaje de interacción
    public float distaniChequeo = 100f; // Distancia del chequeo


    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Time.time - ultimoTiempoChequeo > TiemporChequeo)
        {
            ultimoTiempoChequeo = Time.time;
            //Lanzar rayo
            RaycastHit hit;
            Ray rayo = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(rayo, out hit, distaniChequeo, capaInteractuable))
            {
                Debug.Log("jesus has roto mi juego");
                if (hit.collider.gameObject != objetoInteractuable)
                {
                    objetoInteractuable = hit.collider.gameObject;
                    interactuable = objetoInteractuable.GetComponent<IInteractuable>();
                    Debug.Log("Interaccion con: " + objetoInteractuable.name);
                    //Debug.Log(interactuable.ObtenerMensajeInteraccion());
                    establecerTextoInteraccion();
                }
                else
                {
                    objetoInteractuable = null;
                    interactuable = null;
                    textoInteraccion.gameObject.SetActive(false);
                }

            }

        }
    }


    public void establecerTextoInteraccion()
    {
        textoInteraccion.gameObject.SetActive(true);
        Debug.Log("Interaccion con: " + objetoInteractuable.name);
        textoInteraccion.text = string.Format("<b>[E]</b> {0}", interactuable.ObtenerMensajeInteraccion());
    }
    public void Oninteractuar(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            interactuable.OnInteractuar();
            interactuable = null;
            objetoInteractuable = null;
            textoInteraccion.gameObject.SetActive(false);
        }    
    }
}

public interface IInteractuable
{
    string ObtenerMensajeInteraccion();// mensaje que se muestra al interactuar
    void OnInteractuar();// que se hace al recoger
}
