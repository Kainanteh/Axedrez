using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimiento : MonoBehaviour
{
    
    [System.Serializable]public struct Movimiento
    {
        public Celda cinicio;
        public Celda cobjetivo;

        public Movimiento(Celda cinicio, Celda cobjetivo) : this()
        {
            this.cinicio = cinicio;
            this.cobjetivo = cobjetivo;
        }
    }

    [SerializeField] List<Movimiento> movimientos;

    void Start()
    {

        movimientos = new List<Movimiento>();


    }

    public List<Movimiento> GenerarMovimiento(Celda cinicio, Celda cobjetivo)
    {
        
        movimientos.Add(new Movimiento(cinicio,cobjetivo));

        return movimientos;

    }

}
