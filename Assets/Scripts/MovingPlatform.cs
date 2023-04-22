using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int startingPoint;
    [SerializeField] Transform[] points;
    [SerializeField] Rigidbody2D rb2D;

    private int index;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
        
        index = startingPoint + 1;
        if (index == points.Length)
        {
            index = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, points[index].position) < 0.02f)
        {
            index++;
            if(index == points.Length)
            {
                index = 0;
            }
        }

        rb2D.velocity = (points[index].position - transform.position).normalized * speed;

        // transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
    */

}
