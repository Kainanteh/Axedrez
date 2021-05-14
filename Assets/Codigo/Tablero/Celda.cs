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
  
    AudioSource audioSource;
    AudioSistema audioSistema;


    public bool celdaPromocion = false;

    void Awake()
    {

        unidadMovimiento = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();

    }

    void Start()
    {

        cuadricula  = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        audioSistema = GameObject.Find("AudioSource").GetComponent<AudioSistema>();

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

        unidadMovimiento.CalculoMovimiento( unidadEnCelda,this,false);

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

            if(unidadEnCelda.UnidadJugador != cuadricula.GetComponent<Turno>().GetJugadorActual()) //Si no es el turno del jugador
            {return;}
            
            if(this == unidadceldasoltar){return;}      // Si sueltas en la misma celda

           
            if(unidadselecsoltar != null) // Si la celda donde sueltas ya tiene unidad POSIBLE ATAQUE
            {

                if(unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,true))
                {

                  
                }
                else
                {

                    audioSource.PlayOneShot(audioSistema.audioMovimentoError);

                }

            }   
            else     
            {

                if(unidadMovimiento.GenerarMovimiento(unidadEnCelda,this,unidadceldasoltar,false))
                {

             
                }
                else
                {

                    audioSource.PlayOneShot(audioSistema.audioMovimentoError);

                }

            }

            if(cuadricula.GetComponent<Turno>().GetJugadorActual().gameObject.GetComponent<MateJaque>().Jaque == true)
            {

                unidadMovimiento.JaqueMateCalculo(cuadricula.GetComponent<Turno>().GetJugadorActual().reyCelda.GetUnidadEnCelda(),cuadricula.GetComponent<Turno>().GetJugadorActual().reyCelda); 
                // Si el rey no tiene movimientos Y 
                // la unidad que esta poniendo en jaque al rey no esta amenazada Y 
                // no se puede bloquear el jaque con una unidad del rey que esta en jaque

            }
            else
            {

                unidadMovimiento.ReyAhogado();
                // No esta en jaque
                // No hay ninguna unidad (rey incluido) que pueda hacer movimientos ==> TABLAS
            }

    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

            return hitInfo;

    }

}
