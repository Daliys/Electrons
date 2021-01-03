using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 0.32f;

    void Update()
    {
        if (Game.isGamePause) return;
        transform.position += transform.up * Time.deltaTime * speed; 
    }
}
