using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRepultionAnimation(float sign)
    {
        Vector3 scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = Mathf.Sign(transform.localScale.x) != sign ? new Vector3(scale.x * -1f, scale.y, scale.z) : scale;
        animator.SetInteger("State", 1);
    }
    public void PlayLandingAnimation(float sign)
    {
        Vector3 scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = Mathf.Sign(transform.localScale.x) != sign ? new Vector3(scale.x * -1f, scale.y, scale.z) : scale;
        animator.SetInteger("State", 2);
    }

    public void PlayOff()
    {
        animator.SetInteger("State", 0);
    }
}
