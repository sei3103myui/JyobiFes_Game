using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonMove : MonoBehaviour
{
    public float to = 3;
    public float speed = 1f;
    private float defY;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        defY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time * speed;
        transform.position = new Vector3(transform.position.x, defY + Mathf.PingPong(time, to), transform.position.z);
    }

}
