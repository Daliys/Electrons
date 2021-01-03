using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronControl : MonoBehaviour
{
    public float mouseSensitivity = 25f;

    public float speed = 8f;


    private Vector2 target;
    float signForAnimation = 1;
    Animator animator;
    Animator particalAnimator;
    bool isInvulnerable = false;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        particalAnimator = transform.GetChild(0).GetComponent<Animator>();
    }


    void Update()
    {
        if (Game.isGamePause) return;
       
#if UNITY_EDITOR
        ElectronControlUnity();
#elif UNITY_ANDROID
        ElectronControlAndroid();
#endif

        speed = Game.GameSpeed * 1.6f;

        if (isMoved)    //если движение к другому проводу
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);

            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                isMoved = false;
              
                transform.GetComponentInParent<GameGenerator>().PlayAnimationLanding(signForAnimation);
            }
        }
        else   // иначе повторять движение анимации
        {
            target = transform.GetComponentInParent<GameGenerator>().GetCurrentCablePosition();
             float step = speed * Time.deltaTime;
             transform.position = Vector2.MoveTowards(transform.position, target, step);
            //transform.position = target;

        }
        float addScore = (0.01f) * (Game.GameSpeed + (Game.GameSpeed - 5) * 5);
        Game.score += Bonuses.isBonusX2Score? addScore * 2 : addScore;
    }

    float mousePositionDown;
    bool isButtonUp = false;
    bool isMoved = false;

    void ElectronControlUnity()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionDown = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isButtonUp = false;
        }
        if (Input.GetMouseButton(0))
        {
            if(Mathf.Abs(Mathf.Abs(mousePositionDown) - Mathf.Abs(Input.mousePosition.x)) > mouseSensitivity && !isButtonUp )
            {
                if (mousePositionDown - Input.mousePosition.x > 0)
                {
                    //testSound.Play();
                    target = transform.GetComponentInParent<GameGenerator>().GetLeftCablePosition();
                    signForAnimation = -1f;
                }
                else
                {
                    //testSound.Play();
                    target = transform.GetComponentInParent<GameGenerator>().GetRightCablePosition();
                    signForAnimation = 1f;
                }
                isButtonUp = true;
                isMoved = true;

            }
        }
    
    }
    bool isFirstValue = false;
    void ElectronControlAndroid()
    {
        if(Input.touchCount > 0)
        {

            if (!isFirstValue) {
                mousePositionDown = Input.GetTouch(0).position.x;
                isFirstValue = true;
            }
        
            if (Mathf.Abs((mousePositionDown) - (Input.GetTouch(0).position.x)) > mouseSensitivity && !isButtonUp)
            {
                if (mousePositionDown - Input.mousePosition.x > 0)
                {
                    target = transform.GetComponentInParent<GameGenerator>().GetLeftCablePosition();
                    signForAnimation = -1f;

                }
                else
                {
                    target = transform.GetComponentInParent<GameGenerator>().GetRightCablePosition();
                    signForAnimation = 1f;

                }
                isButtonUp = true;
                isMoved = true;
            }
        }
        else
        {
            isFirstValue = false;
            isButtonUp = false;
        }

            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            if (!isInvulnerable)
            {
                if (collision.GetComponent<ObstacleController>() is DoveObstacle)
                {
                    collision.GetComponent<DoveObstacle>().DoveDeath();
                }           

                if (!Bonuses.IsBonusShield)
                {
                    if (collision.GetComponent<ObstacleController>().typeOfObstacle == ObstacleController.TypeOfObstacle.WireBreak) Game.ElectronHP = 0;
                    else
                    {
                        Game.ElectronHP--;
                        animator.SetTrigger("ElectronDamage");
                        particalAnimator.SetTrigger("ElectronDamage");
                        isInvulnerable = true;
                        StartCoroutine(InvulnerableTimer());
                    }
                }
            }
        }
        if (collision.tag == "Bonus")
        {
            if (collision.transform.GetComponent<BonusInditification>().bonus == BonusInditification.BonusType.Shield) StartCoroutine(ShieldActive(Bonuses.bonusesTime));
            Bonuses.ActiveBonus(collision.transform.GetComponent<BonusInditification>().bonus);
            Destroy(collision.gameObject);
        }
    }   

    IEnumerator InvulnerableTimer()
    {
        IEnumerator coroutine = InvulnerableBlinking();
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(Game.timeInvulnerable);
        isInvulnerable = false;
        StopCoroutine(coroutine);
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator InvulnerableBlinking()
    {
        float value = -0.05f;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Color color = transform.GetComponent<SpriteRenderer>().color;
            color.a += value;
            //color.b += value;
            transform.GetComponent<SpriteRenderer>().color = color;
            if (color.a <= 0.3f && value < 0) value *= -1;
            else if (color.a >= 0.7f && value > 0) value *= -1;
        }
    }

    IEnumerator ShieldActive(float time)
    {
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        transform.GetChild(1).gameObject.SetActive(false);
    }


}
