using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateJaque : MonoBehaviour
{
    public bool Jaque = false;
    public bool JaqueMate = false;
 
    [System.Serializable]public struct MovimientoJaque
    {
        public Unidad unidadJaque;

        public Celda celdaJaque;
        public Celda celdaRey;

        public MovimientoJaque(Unidad unidadJaque, Celda celdaJaque, Celda celdaRey)
        {
            this.unidadJaque = unidadJaque;
            this.celdaJaque = celdaJaque;
            this.celdaRey = celdaRey;
        }
        
    }

    public MovimientoJaque jaqueMovimiento;

    // Cuadricula cuadricula;
    // UnidadMovimiento unidadesMov;

    // void Start()
    // {

    //     cuadricula = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();
    //     unidadesMov = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    // }

    // public bool esJaque()
    // {

    //     foreach(Unidad unidad in cuadricula.unidades)
    //     {

    //     // unidadesMov.CalculoMovimiento(unidad,unidad);
        
    //     }


    //     return false;
    // }


}
