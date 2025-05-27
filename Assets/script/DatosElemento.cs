using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumeración para definir los tipos de elementos que pueden existir
public enum TipoElemento
{
    Comida,  // Elemento de tipo comida
    Bebida,  // Elemento de tipo bebida
    Descanso, // Elemento de tipo descanso
}

// Atributo que permite crear instancias de esta clase desde el menú de Unity
[CreateAssetMenu(fileName = "NuevoElemento", menuName = "Inventario/Elemento")]
public class DatosElemento : ScriptableObject
{
    [Header("Datos del Elemento")]
    public string nombre;       // Nombre del elemento
    public string descripcion;  // Descripción del elemento
    public Sprite icono;        // Icono del elemento para mostrar en la UI
    public GameObject prefab;   // Prefab del elemento para instanciar en el juego
    public TipoElemento tipoElemento; // Tipo del elemento (Comida, Bebida, Descanso)
}
