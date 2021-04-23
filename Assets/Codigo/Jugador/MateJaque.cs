using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateJaque : MonoBehaviour
{


    public bool Jaque = false;
    public bool JaqueMate = false;
 
    Cuadricula cuadricula;
    UnidadMovimiento unidadesMov;

    void Start()
    {

        cuadricula = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();
        unidadesMov = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    }

    // public bool esJaque()
    // {

    //     foreach(Unidad unidad in cuadricula.unidades)
    //     {

    //     // unidadesMov.CalculoMovimiento(unidad,unidad);
        
    //     }


    //     return false;
    // }


}
