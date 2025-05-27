using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    private Vector2 ratonDelta; // Almacena el movimiento del ratón

    [Header("Vista Camara")]
    public Transform camara; // Referencia a la cámara
    public float minVistaX, maxVistaX; // Límites de la vista vertical de la cámara
    public float sensibilidadRaton; // Sensibilidad del ratón
    private float rotacionActualCamara; // Rotación actual de la cámara en el eje X


    [Header("Movimiento Jugador")]
    public float velocidadMovimiento; // Velocidad de movimiento del jugador
    private float rotacionMovimiento; // Rotación del movimiento (no se usa)
    private Vector2 movimientoactual; // Vector que almacena el movimiento actual del jugador
    private Rigidbody rb; // Referencia al componente Rigidbody del jugador

    [Header("Salto")]
    public int fuerzaSalto = 100; // Fuerza del salto
    public LayerMask capaSuelo; // Capa que representa el suelo
    private bool puedeMirar = true; // Booleano que indica si el jugador puede mover la cámara


    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene la referencia al componente Rigidbody
    }

    // Método llamado por el Input System para obtener el movimiento del ratón
    public void OnVistaInput(InputAction.CallbackContext context)
    {
        ratonDelta = context.ReadValue<Vector2>(); // Lee el valor del movimiento del ratón
    }

    private void LateUpdate()
    {
        // Si el jugador puede mirar, actualiza la vista de la cámara
        if (puedeMirar)
        {
            VistaCamara();
        }
    }

    private void FixedUpdate()
    {
        MovimientoJugador(); // Llama al método para mover al jugador
    }

    void Start()
    {
        ModoInventario(false); // Desactiva el modo inventario al inicio
    }

    // Método para controlar la vista de la cámara
    private void VistaCamara()
    {
        rotacionActualCamara += ratonDelta.y * sensibilidadRaton; // Calcula la rotación de la cámara en el eje X
        rotacionActualCamara = Mathf.Clamp(rotacionActualCamara, minVistaX, maxVistaX); // Limita la rotación de la cámara
        //se modifica la rotacion de la camara 
        camara.localEulerAngles = new Vector3(-rotacionActualCamara, 0, 0); // Aplica la rotación a la cámara
        transform.eulerAngles += new Vector3(0, ratonDelta.x * sensibilidadRaton, 0); // Rota al jugador en el eje Y
    }

    // Método para controlar el movimiento del jugador
    private void MovimientoJugador()
    {
        //el movimiento que yo lleve se le aplica el movimiento de la tecla que yo puse 
        Vector3 movimiento = transform.forward * movimientoactual.y + transform.right * movimientoactual.x; // Calcula el vector de movimiento
        movimiento *= velocidadMovimiento; // Aplica la velocidad al movimiento
        movimiento.y = rb.velocity.y; //mantengo la velocidad en y para que no se caiga el jugador
        rb.velocity = movimiento; //aplico el movimiento al rigidbody
    }
    // Método llamado por el Input System para obtener el movimiento del jugador
    public void OnMovimientoInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            movimientoactual = context.ReadValue<Vector2>(); // Lee el valor del movimiento del jugador
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movimientoactual = Vector2.zero; // Si se cancela el movimiento, el vector es cero
        }
    }


    //accion de salto
    public void OnSaltoInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (EstaSuelo())
            {
                rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse); //aplico una fuerza hacia arriba al rigidbody
            }
        }
    }

    // Método para verificar si el jugador está en el suelo
    private bool EstaSuelo()
    {
        //verifico si el jugador esta en el suelo
        if (rb == null) return false;
        //RaycastHit hit;
        Ray[] rays = new Ray[4]{
                new Ray(transform.position + transform.forward * 0.25f, Vector3.down),
                new Ray(transform.position - transform.forward * 0.25f, Vector3.down),
                new Ray(transform.position + transform.right * 0.25f, Vector3.down),
                new Ray(transform.position - transform.right * 0.25f, Vector3.down)
            };

        foreach (Ray ray in rays)
            return Physics.Raycast(ray, 1f, capaSuelo);

        return false;
    }

    // Método para activar o desactivar el modo inventario
    public void ModoInventario(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked; // Bloquea o desbloquea el cursor
        puedeMirar = !value; // Activa o desactiva la vista de la cámara
    }




    void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + transform.forward * 0.25f, Vector3.down);
            Gizmos.DrawRay(transform.position - transform.forward * 0.25f, Vector3.down);
            Gizmos.DrawRay(transform.position + transform.right * 0.25f, Vector3.down);
            Gizmos.DrawRay(transform.position - transform.right * 0.25f, Vector3.down);
            Gizmos.DrawRay(transform.position, Vector3.down);
        }
    }
}



