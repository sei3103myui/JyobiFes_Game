using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedUI : MonoBehaviour
{
    public GameObject firstSelected;

    private void OnEnable()
    {   
        if(firstSelected != null)
        {
            // ‚à‚µSetActive‚ªfalse‚È‚çtrue‚ªŒ©‚Â‚©‚é‚Ü‚Å’T‚·
            if (!firstSelected.gameObject.activeSelf)
            {
                var childCnt = transform.childCount;
                for(int i = 0; i < childCnt; i++)
                {
                    bool isActive = transform.GetChild(i).gameObject.activeSelf;
                    if (isActive)
                    {
                        firstSelected = transform.GetChild(i).gameObject;
                        break;
                    }
                }
            }
            StartCoroutine(MyCoroutinUI.SetSelectedGameObject(firstSelected));
        }
        else
        {
            firstSelected = transform.GetChild(0).gameObject;
            StartCoroutine(MyCoroutinUI.SetSelectedGameObject(firstSelected));
        }

    }

    private void OnDisable()
    {
        EventSystem.current?.SetSelectedGameObject(null);
    }



}
