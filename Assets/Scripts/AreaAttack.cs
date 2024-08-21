using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AreaAttack : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    
    void FixedUpdate()
    {
        // Vector2 angle = transform.rotation.eulerAngles;
        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }

}
