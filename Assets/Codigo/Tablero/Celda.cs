﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Celda : MonoBehaviour,IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    public int columna;
    public int fila;

 

    [SerializeField] Unidad unidadEnCelda = null;



    public Unidad GetUnidadEnCelda()
    {

        return unidadEnCelda;

    }

    public void SetUnidadEnCelda(Unidad nuevaUnidad)
    {

        unidadEnCelda = nuevaUnidad;
        unidadEnCelda.transform.position = transform.position;

    }

    public void OnPointerClick(PointerEventData eventData)
    {

                        
            RaycastHit2D hitInfo = OnControlRay();
            
            if (!hitInfo)
            {
                return;
            }

            Debug.Log("Click en " + hitInfo.transform.gameObject.name);

        


    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        RaycastHit2D hitInfo = OnControlRay();
            
        if (!hitInfo)
        {
            return;
        }


        /*if(unidadEnCelda == null){return;}

        unidadEnCelda.gameObject.transform.position = hitInfo.transform.position;*/
        
        
        //Debug.Log("Arrastar en " + hitInfo.transform.gameObject.name);

    }

    public void OnDrag(PointerEventData eventData)
    {

        RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
                return;
            }

        /*if(unidadEnCelda == null){return;}

        unidadEnCelda.gameObject.transform.position = hitInfo.transform.position;*/
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

            RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
                //unidadEnCelda.gameObject.transform.position = transform.position;
                return;
            }

            /*Celda unidadceldasoltar = hitInfo.transform.gameObject.GetComponent<Celda>();
            //Unidad unidadselecsoltar = unidadceldasoltar.GetUnidadEnCelda();

            if(GetUnidadEnCelda() == null){return;} // Si esta celda no tiene unidad
            //if(unidadselecsoltar != null){return;}        // Si la celda donde sueltas ya tiene unidad

            
            

            int movcolum = 999;
            int movfila = 999;

            movcolum = columna - unidadceldasoltar.columna;
            movfila = fila - unidadceldasoltar.fila;

            //Debug.Log("Soltar en " + hitInfo.transform.gameObject.name);

            if(movcolum == 999 || movfila == 999)

            switch(unidadEnCelda.GetTipoUnidad())
            {

                case TipoUnidad.Peon:
                {
                    //Debug.Log((movcolum*-1) + " " + (movfila*-1));
                    //if(movcolum<-1 || movcolum>1 || movfila<-1 || movfila>1){// Limite para el peon
                        
                        if(movfila == -1 && movcolum == 0)
                        {
                            
                            unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
                            unidadEnCelda = null; // Esta celda ya no tiene unidad
                            break;
                        }
                        else
                        {
                            unidadEnCelda.gameObject.transform.position = transform.position;
                            break;
                        }

                    //break;
                }
                case TipoUnidad.Torre:
                {
                   
                    //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
                    //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
                        if(unidadceldasoltar.columna == columna  || unidadceldasoltar.fila == fila)
                        {
                            
                            unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
                            unidadEnCelda = null; // Esta celda ya no tiene unidad
                            break;
                        }
                        else
                        {
                            unidadEnCelda.gameObject.transform.position = transform.position;
                            break;
                        }
                }
                case TipoUnidad.Alfil:
                {
                   
                    //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
                    
                        if(unidadceldasoltar.columna != columna  || unidadceldasoltar.fila != fila)
                        {
                            //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
                            unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
                            unidadEnCelda = null; // Esta celda ya no tiene unidad
                            break;
                        }
                        else
                        {
                            unidadEnCelda.gameObject.transform.position = transform.position;
                            break;
                        }
                }
                default:
                {

                return;
                
                }




            }

            

                /*if(movcolum > 0){Debug.Log("Te has movido hacia izquierda " + movcolum + " casillas");}
                else if (movcolum < 0){Debug.Log("Te has movido hacia derecha " + movcolum*-1 + " casillas");}

                if(movfila > 0){Debug.Log("Te has movido hacia abajo " + movfila + " casillas");}
                else if (movfila < 0){Debug.Log("Te has movido hacia arriba " + movfila*-1 + " casillas");}*/

            
       
    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            
            return hitInfo;

    }

    
}