using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveControler : MonoBehaviour
{
    public bool isMove = false;
    public float time = 3;
    float sec;
    Quaternion defaultRotation;

    [SerializeField] float angle;
    [SerializeField] float rotateSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            sec += Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(Mathf.Sin(sec * rotateSpeed) * angle, Vector3.right) * defaultRotation;
            StartCoroutine(MoveCountdown());
        }
        
    }

    private IEnumerator MoveCountdown()
    {
        yield return new WaitForSeconds(time);
        isMove = false;
    }
}
