using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Celda : MonoBehaviour,IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    public int fila;
    public int columna;
    

    [SerializeField] UnidadMovimiento unidadMovimiento;

    [SerializeField] Unidad unidadEnCelda = null;

    void Awake()
    {

        unidadMovimiento = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    }

    public Unidad GetUnidadEnCelda()
    {

        return unidadEnCelda;

    }

    public void SetUnidadEnCelda(Unidad nuevaUnidad)
    {

        unidadEnCelda = nuevaUnidad;
        if(nuevaUnidad == null){return;}
        unidadEnCelda.transform.position = transform.position;

    }

    public void OnPointerClick(PointerEventData eventData)
    {

                        
            RaycastHit2D hitInfo = OnControlRay();
            
            if (!hitInfo)
            {
                return;
            }

            if(unidadEnCelda == null){return;}

            // Debug.Log("Click en " + hitInfo.transform.gameObject.name);

            unidadEnCelda.gameObject.transform.position = transform.position;
            


    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        RaycastHit2D hitInfo = OnControlRay();
            
        if (!hitInfo)
        {
            return;
        }


        if(unidadEnCelda == null){return;}

        unidadEnCelda.gameObject.transform.position = new Vector3(hitInfo.point.x,hitInfo.point.y,0f);
        //unidadEnCelda.gameObject.transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0f);

            

        //unidadMovimiento.CalculoMovimiento( unidadEnCelda.GetTipoUnidad(),this,unidadEnCelda.movimientofila,unidadEnCelda.movimientocolumna);
        unidadMovimiento.CalculoMovimiento( unidadEnCelda,this);
        
        //Debug.Log("Arrastar en " + hitInfo.transform.gameObject.name);

    }

    public void OnDrag(PointerEventData eventData)
    {

        RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
                return;
            }

        if(unidadEnCelda == null){return;}

        unidadEnCelda.gameObject.transform.position = new Vector3(hitInfo.point.x,hitInfo.point.y,0f);
        //unidadEnCelda.gameObject.transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0f);

        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

            unidadMovimiento.ReiniciarCalculo();

            RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
                //unidadEnCelda.gameObject.transform.position = transform.position;
                return;
            }

            Celda unidadceldasoltar = hitInfo.transform.gameObject.GetComponent<Celda>();
            Unidad unidadselecsoltar = unidadceldasoltar.GetUnidadEnCelda();

            if(GetUnidadEnCelda() == null){return;}     // Si esta celda no tiene unidad
            
            
            if(this == unidadceldasoltar){return;}      // Si sueltas en la misma celda

            //if(unidadselecsoltar != null){return;}      // Si la celda donde sueltas ya tiene unidad POSIBLE ATAQUE

                                                        // Si no hay nadie movimiento?
            

            // int movcolum = 999;
            // int movfila = 999;

            // movcolum = unidadceldasoltar.columna - columna;
            // movfila = unidadceldasoltar.fila - fila;

            // //Debug.Log("Soltar en " + hitInfo.transform.gameObject.name);

            // if(movcolum == 999 || movfila == 999){return;}


            if(unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar))
            {



            }
            else
            {

                

            }

            // switch(unidadEnCelda.GetTipoUnidad())
            // {

            //     case TipoUnidad.Peon:
            //     {
            //         //Debug.Log((movcolum*-1) + " " + (movfila*-1));
            //         //if(movcolum<-1 || movcolum>1 || movfila<-1 || movfila>1){// Limite para el peon
                        
            //             if(movfila == -1 && movcolum == 0)
            //             {
                            
            //                 unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
            //                 unidadEnCelda = null; // Esta celda ya no tiene unidad
            //                 break;
            //             }
            //             else
            //             {
            //                 unidadEnCelda.gameObject.transform.position = transform.position;
            //                 break;
            //             }

            //         //break;
            //     }
            //     case TipoUnidad.Torre:
            //     {
                   
            //         //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
            //         //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
            //             if(unidadceldasoltar.columna == columna  || unidadceldasoltar.fila == fila)
            //             {
                            
            //                 unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
            //                 unidadEnCelda = null; // Esta celda ya no tiene unidad
            //                 break;
            //             }
            //             else
            //             {
            //                 unidadEnCelda.gameObject.transform.position = transform.position;
            //                 break;
            //             }
            //     }
            //     case TipoUnidad.Alfil:
            //     {
                   
            //         //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
                    
            //             if(unidadceldasoltar.columna != columna  || unidadceldasoltar.fila != fila)
            //             {
            //                 //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
            //                 unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
            //                 unidadEnCelda = null; // Esta celda ya no tiene unidad
            //                 break;
            //             }
            //             else
            //             {
            //                 unidadEnCelda.gameObject.transform.position = transform.position;
            //                 break;
            //             }
            //     }
            //     case TipoUnidad.Reina:
            //     {
                   
            //         //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
                    
            //             if(true)
            //             {
            //                 //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
            //                 unidadceldasoltar.SetUnidadEnCelda(unidadEnCelda); // La celda donde muevo la unidad ahora tiene esa unidad
            //                 unidadEnCelda = null; // Esta celda ya no tiene unidad
            //                 unidadMovimiento.GenerarMovimiento(this,unidadceldasoltar);
            //                 break;
            //             }
                    
            //     }
            //     default:
            //     {

            //     return;
                
            //     }




            //}

            
                //Debug.Log("movimiento COLUMNA " + movcolum + " movimiento FILA " + movfila + " ");

                // if(movcolum > 0){Debug.Log("Te has movido hacia izquierda " + movcolum + " casillas");}
                // else if (movcolum < 0){Debug.Log("Te has movido hacia derecha " + movcolum + " casillas");}

                // if(movfila > 0){Debug.Log("Te has movido hacia abajo " + movfila + " casillas");}
                // else if (movfila < 0){Debug.Log("Te has movido hacia arriba " + movfila + " casillas");}

            
       
    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            
            
            return hitInfo;

    }

    
}
