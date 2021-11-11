using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    public RectTransform arrow;
    private int counter = 0;
    private float aimX;
    private float aimY;
    private Vector3 aimPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        arrow.position += new Vector3(aimX, aimY, 0);
        counter++;
        if (counter == 1)
        {
            aimX = Random.Range(-5.0f, 5.0f); 
            //Debug.Log(aimX);
            aimY = Random.Range(-5.0f, 5.0f);
            //Debug.Log(aimY);
        }
        else if (counter == 100)
        {
            counter = 0;
        }
        //Debug.Log(arrow.position.x + "+"  + arrow.position.y);
        //à⁄ìÆêßå¿
        arrow.position = new Vector3(Mathf.Clamp(arrow.position.x, 860, 1060),Mathf.Clamp(arrow.position.y, 240, 640), arrow.position.z);
    }
}
