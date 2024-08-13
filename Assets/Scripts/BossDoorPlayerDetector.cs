using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorPlayerDetector : MonoBehaviour
{
    // We have this as a child so it can be in a different layer
    // The different layer allows the door to act as a platform (i.e. an actual door)
    // But the trigger is only a trigger. It should not have any physical interaction

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Layer
        if (collision.gameObject.layer == 6)
        {
            GetComponentInParent<BossDoor>().UpdateState(false);
        }
    }
}
