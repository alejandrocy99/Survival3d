using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlInteractuable : MonoBehaviour
{
    private Camera cam; // Referencia a la cámara principal

    public float TiemporChequeo = 0.5f; // Tiempo entre chequeos de raycast
    public float ultimoTiempoChequeo; // Tiempo del último chequeo realizado
    public LayerMask capaInteractuable; // Máscara para filtrar objetos interactuables en el raycast

    private GameObject objetoInteractuable; // Objeto actualmente detectado como interactuable
    private IInteractuable interactuable; // Referencia a la interfaz del objeto interactuable

    public TextMeshProUGUI textoInteraccion; // Texto que muestra el mensaje de interacción en pantalla
    public float distaniChequeo = 100f; // Distancia máxima del raycast

    void Start()
    {
        cam = Camera.main; // Asigna la cámara principal
    }

    void Update()
    {
        // Realiza el chequeo solo si ha pasado el tiempo definido
        if (Time.time - ultimoTiempoChequeo > TiemporChequeo)
        {
            ultimoTiempoChequeo = Time.time;
            // Lanza un rayo desde el centro de la pantalla
            RaycastHit hit;
            Ray rayo = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(rayo, out hit, distaniChequeo, capaInteractuable))
            {
                Debug.Log("jesus has roto mi juego");
                // Si el objeto detectado es diferente al anterior, actualiza referencias
                if (hit.collider.gameObject != objetoInteractuable)
                {
                    objetoInteractuable = hit.collider.gameObject;
                    interactuable = objetoInteractuable.GetComponent<IInteractuable>();
                    Debug.Log("Interaccion con: " + objetoInteractuable.name);
                    // Muestra el mensaje de interacción
                    establecerTextoInteraccion();
                }
                else
                {
                    // Si no hay objeto interactuable, limpia referencias y oculta el texto
                    objetoInteractuable = null;
                    interactuable = null;
                    textoInteraccion.gameObject.SetActive(false);
                }
            }
        }
    }

    // Muestra el mensaje de interacción en pantalla
    public void establecerTextoInteraccion()
    {
        textoInteraccion.gameObject.SetActive(true);
        Debug.Log("Interaccion con: " + objetoInteractuable.name);
        textoInteraccion.text = string.Format("<b>[E]</b> {0}", interactuable.ObtenerMensajeInteraccion());
    }

    // Método que se llama al presionar la tecla de interacción (Input System)
    public void Oninteractuar(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            interactuable.OnInteractuar(); // Ejecuta la acción de interacción
            interactuable = null;
            objetoInteractuable = null;
            textoInteraccion.gameObject.SetActive(false); // Oculta el mensaje de interacción
        }
    }
}

// Interfaz para objetos interactuables
public interface IInteractuable
{
    string ObtenerMensajeInteraccion(); // Mensaje que se muestra al interactuar
    void OnInteractuar(); // Acción que se realiza al interactuar
}
