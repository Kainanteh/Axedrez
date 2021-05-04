using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimientoEspecial : MonoBehaviour
{
 
    UnidadMovimiento unidadMovimiento;

    void Awake()
    {

        unidadMovimiento = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    }

    public bool Enroque(Celda cinicio,Celda cobjetivo,Unidad unidad)
    {
       
        

        if(unidad.UnidadJugador != cobjetivo.GetUnidadEnCelda().UnidadJugador){return false;}
            

        if(cinicio.columna > cobjetivo.columna)  // enroque corto
        {

            for(int i = cinicio.columna-1; i >= cobjetivo.columna+1; i--)
            {

                Celda ctemp = GameObject.Find("Celda " + cinicio.fila + " " + i).GetComponent<Celda>();
                if(ctemp.GetUnidadEnCelda() != null)
                {
                    return false;   //No puede haber unidades entre el rey y la torre
                }
                if(unidadMovimiento.JaqueCalculo(ctemp,false,null,null))
                {
                    return false;   //Ninguna de las celdas por las que va a pasar el rey puede estar amenazada
                }
              
            }

        }
        else                                   // enroque largo
        {

            for(int i = cinicio.columna+1; i <= cobjetivo.columna-1; i++)
            {

                Celda ctemp = GameObject.Find("Celda " + cinicio.fila + " " + i).GetComponent<Celda>();
                if(ctemp.GetUnidadEnCelda() != null)
                {
                    return false;   //No puede haber unidades entre el rey y la torre
                }
                if(unidadMovimiento.JaqueCalculo(ctemp,false,null,null))
                {
                    return false;   //Ninguna de las celdas por las que va a pasar el rey puede estar amenazada
                }

            }        

        }

            bool tMovimiento = false;
              
            foreach(UnidadMovimiento.Movimiento movimientos in unidadMovimiento.movimientos)
            {
                    
                if(movimientos.uinicio == cinicio.GetUnidadEnCelda() || movimientos.uinicio == cobjetivo.GetUnidadEnCelda())
                {

                    tMovimiento = true;

                }
                    
            }

            if(tMovimiento == true) //Ni el rey ni la torre a la que quiere hacer el enroque se han movido en la partida
            {
                return false;
            }

        return true;

    }


}
