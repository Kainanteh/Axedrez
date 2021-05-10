using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimientoEspecial : MonoBehaviour
{
 
    UnidadMovimiento unidadMovimiento;

    Transform InterfazPromocion;

    void Awake()
    {

        unidadMovimiento = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();
        InterfazPromocion = GameObject.Find("IntPromocion").GetComponent<Transform>();

        InterfazPromocion.gameObject.GetComponent<Renderer>().enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[0].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[1].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[2].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[3].enabled = false;


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

    public void Promocion(Celda cPromocion)
    {

        if(cPromocion.GetUnidadEnCelda().UnidadJugador.idJugador == 1)
        {

            InterfazPromocion.position = new Vector3(cPromocion.gameObject.transform.position.x,cPromocion.gameObject.transform.position.y-1.5f,0f);
        
        }
        else if(cPromocion.GetUnidadEnCelda().UnidadJugador.idJugador == 2)
        {

            InterfazPromocion.position = new Vector3(cPromocion.gameObject.transform.position.x,cPromocion.gameObject.transform.position.y+1.5f,0f);

        }

        InterfazPromocion.gameObject.GetComponent<Renderer>().enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[1].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[2].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[3].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[4].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[0].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[1].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[2].enabled = true;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[3].enabled = true;

        unidadMovimiento.enPromocion = true;
        unidadMovimiento.cEnPromocion = cPromocion;

        // Debug.Log(InterfazPromocion);
        // Debug.Log("Promocionado!");

    }


}
