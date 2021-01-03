using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveObstacle : ObstacleController
{
    Animator animator;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

    }

    public void DoveDeath()
    {
        animator.SetInteger("State", 2);
        Destroy(gameObject, 0.5f);
    }

}
