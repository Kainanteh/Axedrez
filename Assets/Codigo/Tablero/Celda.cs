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
     

       
        unidadMovimiento.CalculoMovimiento( unidadEnCelda,this);
        
      

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
       
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

            unidadMovimiento.ReiniciarCalculo();

            RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
              
                return;
            }

            Celda unidadceldasoltar = hitInfo.transform.gameObject.GetComponent<Celda>();
            Unidad unidadselecsoltar = unidadceldasoltar.GetUnidadEnCelda();

            if(GetUnidadEnCelda() == null){return;}     // Si esta celda no tiene unidad
            
            
            if(this == unidadceldasoltar){return;}      // Si sueltas en la misma celda

            
      
            if(unidadselecsoltar != null)
            {

                unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,true);

            }   
            else     // Si la celda donde sueltas ya tiene unidad POSIBLE ATAQUE
            {

                unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,false);

            }

          
          


       
    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            
            
            return hitInfo;

    }

    
}
