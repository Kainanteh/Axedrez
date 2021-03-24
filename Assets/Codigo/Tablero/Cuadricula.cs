using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Cuadricula : MonoBehaviour
{

    public int columnas = 6;
	public int filas = 6;

	public Celda celdaPrefab;
    Celda[] celdas;

	public TextMeshProUGUI celdaEtiquetaPrefab;

	[SerializeField] private Canvas cuadriculaCanvas;

	public Transform Limite;

	public Color32 ColorNegras;
	public Color32 ColorBlancas;

	void Awake () {

		

		cuadriculaCanvas.transform.position = new Vector2(((float)(columnas-1)/2)*-1,((float)(filas-1)/2)*-1); // Para centrar la cuadricula
		Limite.localScale = new Vector3((float)columnas+0.5f,(float)filas+0.5f,0f); 

		celdas = new Celda[filas * columnas];

		for (int y = 0, i = 0; y < filas; y++) 
		{
			for (int x = 0; x < columnas; x++) 
			{
				CrearCelda(x, y, i++);
			}
		}
		//cuadriculaCanvas = GetComponentInChildren<Canvas>();
	}
	
	void CrearCelda (int x, int y, int i) 
	{
		Vector2 position;
		position.x = x * 1f;
		position.y = y * 1f;

		Celda celda = celdas[i] = Instantiate<Celda>(celdaPrefab);
		celda.transform.SetParent(cuadriculaCanvas.transform, false);
		celda.transform.localPosition = position;
		celda.gameObject.name = "Celda " + x.ToString() + " " + y.ToString(); 
		celda.columna = x;
		celda.fila = y;

		if((x + y)%2 == 0 ) // Si es par color "negro" si es impar color "blanco"
		{
			//celda.gameObject.GetComponent<SpriteRenderer>().color = new Color32(144, 104, 62,255);//90683E
			celda.gameObject.GetComponent<SpriteRenderer>().color = ColorNegras;
		}
		else
		{
			//celda.gameObject.GetComponent<SpriteRenderer>().color = new Color32(222, 161, 98,255);//DEA162
			celda.gameObject.GetComponent<SpriteRenderer>().color = ColorBlancas;
		}

		
		
		
		

		TextMeshProUGUI etiqueta = Instantiate<TextMeshProUGUI>(celdaEtiquetaPrefab);
		//etiqueta.rectTransform.SetParent(cuadriculaCanvas.transform, false); // Padre la Cuadricula
		etiqueta.rectTransform.SetParent(celda.transform, false); // Padre la Celda
		//etiqueta.rectTransform.anchoredPosition =
		//	new Vector2(cuadriculaCanvas.transform.position.x, cuadriculaCanvas.transform.position.y);
		etiqueta.text = x.ToString() + " " + y.ToString();

	}

}
