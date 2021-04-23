using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turno : MonoBehaviour
{
   
    [SerializeField]
    private Jugador jugadorActual;

    public List<Jugador> Jugadores;

    void Start()
    {
        jugadorActual = GameObject.Find("Jugador 2").GetComponent<Jugador>();
    }

    public Jugador GetJugadorActual()
    {

        return jugadorActual;

    }

    public void SetJugadorActual(Jugador nuevoJugadorActual)
    {

        nuevoJugadorActual = jugadorActual;

    }

    public void CambiarTurno()
    {

        if(jugadorActual == Jugadores[0]){jugadorActual = Jugadores[1];}
        else{jugadorActual = Jugadores[0];}

    }



                                    
}
