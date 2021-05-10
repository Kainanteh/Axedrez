using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnidadPromocion : MonoBehaviour, IPointerClickHandler
{

    public LayerMask lm;

    UnidadMovimiento UM_Script;

    void Start()
    {

        // Debug.Log(this.gameObject.name);
        UM_Script = GameObject.Find("Cuadricula").GetComponent<UnidadMovimiento>();      

    }

    private RaycastHit2D OnControlRay()
    {

            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero,Mathf.Infinity,lm);

            return hitInfo;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
           
        RaycastHit2D hitInfo = OnControlRay();
            
        if (!hitInfo || UM_Script.enPromocion == false)
        {
            return;
        }

        // Debug.Log(hitInfo.transform.gameObject.name);

        switch(hitInfo.transform.gameObject.name)
        {

            case "IntCaballo":
            {
                UM_Script.Promocionado(1);
                break;
            }
            case "IntTorre":
            {
                UM_Script.Promocionado(2);
                break;
            }
            case "IntAlfil":
            {
                UM_Script.Promocionado(3);
                break;
            }
            case "IntReina":
            {
                UM_Script.Promocionado(4);
                break;
            }

        }

        

    }

}
