using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    private Vector2 ratonDelta;

    [Header("Vista Camara")]
    public Transform camara;
    public float minVistaX, maxVistaX;
    public float sensibilidadRaton;
    private float rotacionActualCamara;


    [Header("Movimiento Jugador")]
    public float velocidadMovimiento;
    private float rotacionMovimiento;
    private Vector2 movimientoactual;
    private Rigidbody rb;

    [Header("Salto")]
    public int fuerzaSalto = 100;
    public LayerMask capaSuelo;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void OnVistaInput(InputAction.CallbackContext context)
    {
        ratonDelta = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        VistaCamara();
    }

    private void FixedUpdate()
    {
        MovimientoJugador();
    }

    private void VistaCamara()
    {
        rotacionActualCamara += ratonDelta.y * sensibilidadRaton;
        rotacionActualCamara = Mathf.Clamp(rotacionActualCamara, minVistaX, maxVistaX);
        //se modifica la rotacion de la camara 
        camara.localEulerAngles = new Vector3(-rotacionActualCamara, 0, 0);
        transform.eulerAngles += new Vector3(0, ratonDelta.x * sensibilidadRaton, 0);
    }

    private void MovimientoJugador()
    {
        //el movimiento que yo lleve se le aplica el movimiento de la tecla que yo puse 
        Vector3 movimiento = transform.forward * movimientoactual.y + transform.right * movimientoactual.x;
        movimiento *= velocidadMovimiento;
        movimiento.y = rb.velocity.y; //mantengo la velocidad en y para que no se caiga el jugador
        rb.velocity = movimiento; //aplico el movimiento al rigidbody
    }
    public void OnMovimientoInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            movimientoactual = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movimientoactual = Vector2.zero;
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

    private bool EstaSuelo()
    {
        //verifico si el jugador esta en el suelo
        if(rb == null) return false;
        //RaycastHit hit;
        Ray[] rays = new Ray[4]{
                new Ray(transform.position + transform.forward * 0.25f, Vector3.down),
                new Ray(transform.position - transform.forward * 0.25f, Vector3.down),
                new Ray(transform.position + transform.right * 0.25f, Vector3.down),
                new Ray(transform.position - transform.right * 0.25f, Vector3.down)
            };

        foreach (Ray ray in rays)
            return Physics.Raycast(ray, 1f, capaSuelo);
            
        return false;}

        



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
}}



