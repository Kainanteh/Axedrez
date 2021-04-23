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


    Cuadricula cuadricula;
  
    public bool cAmenazada = false;


    void Awake()
    {

        unidadMovimiento = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    }

    void Start()
    {

        cuadricula  = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();
      

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

        if(unidadEnCelda.UnidadJugador != GameObject.Find("Cuadricula").GetComponent<Turno>().GetJugadorActual())
        {return;}

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

        if(unidadEnCelda.UnidadJugador != GameObject.Find("Cuadricula").GetComponent<Turno>().GetJugadorActual())
        {return;}

        unidadEnCelda.gameObject.transform.position = new Vector3(hitInfo.point.x,hitInfo.point.y,0f);
       
        unidadEnCelda.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 25; // El sprite seleccionado tiene que estar por encima de todo lo demas
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

            

            unidadMovimiento.ReiniciarCalculo(); // Se reinicia en todas las celdas los sprite de movimiento y ataque

           

            RaycastHit2D hitInfo = OnControlRay();

            if (!hitInfo)
            {
                this.SetUnidadEnCelda(this.GetUnidadEnCelda()); // Si sueltas la unidad y no es una celda
                return;
            }
            else if(hitInfo.transform.gameObject.name=="PantallaLimite"){this.SetUnidadEnCelda(this.GetUnidadEnCelda());return;}

            Celda unidadceldasoltar = hitInfo.transform.gameObject.GetComponent<Celda>();
            Unidad unidadselecsoltar = unidadceldasoltar.GetUnidadEnCelda();

          
            if(GetUnidadEnCelda() == null){return;}     // Si esta celda no tiene unidad
            
               unidadEnCelda.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 5; 


            if(unidadEnCelda.UnidadJugador != GameObject.Find("Cuadricula").GetComponent<Turno>().GetJugadorActual()) //Si no es el turno del jugador
            {return;}
            
            if(this == unidadceldasoltar){return;}      // Si sueltas en la misma celda

              //if(ataque == true && cobjetivo.GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador){cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); return false;} //Si la unidad es del mismo jugador
        
      
            if(unidadselecsoltar != null) // Si la celda donde sueltas ya tiene unidad POSIBLE ATAQUE
            {

                if(unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,true))
                {
                   
                }

            }   
            else     
            {

                if(unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,false))
                {
                  
                }

            }

           
          
       
    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            
            
            return hitInfo;

    }

    
}
