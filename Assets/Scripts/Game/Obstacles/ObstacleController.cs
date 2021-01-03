using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Vector2 currentGoal;
    protected int numCurrentGoal = -1; // текущая цель
    protected int numGoalPoits;       // количество целей
    protected bool lastGoal = false;

    public TypeOfObstacle typeOfObstacle;
    public enum TypeOfObstacle { Knot, Dove, Stick, WireBreak};
 
    public virtual void Start()
    {
        numGoalPoits = transform.parent.GetChild(1).transform.childCount;
        GetNextGoal();
    }

    public virtual void Update()
    {
        if (Game.isGamePause) return;

        transform.localScale = new Vector3(Mathf.Sign(transform.parent.localScale.x), transform.parent.localScale.y, transform.parent.localScale.z) ;

        Movement();

        if (transform.position.y - GetCurrentPosition().y  < 0.01f) GetNextGoal();
        if (transform.position.y < -10) Destroy(gameObject);
    }

    protected virtual void Movement()
    {
        float step = Game.GameSpeed * Time.deltaTime;
        transform.position = new Vector2(GetCurrentPosition().x, transform.position.y - step);
    }

    protected void GetNextGoal()
    {
        //print(transform.parent.GetChild(1).transform.childCount);
        if (numCurrentGoal + 1 < numGoalPoits)
        {
            // if(numCurrentGoal != 5)
            numCurrentGoal++;
            currentGoal = transform.parent.GetChild(1).transform.GetChild(numCurrentGoal).transform.position;
        }
        else {  lastGoal = true; }
    }

    protected Vector2 GetCurrentPosition()
    {
        if (lastGoal) return new Vector2(transform.position.x, -20);
        return transform.parent.GetChild(1).transform.GetChild(numCurrentGoal).transform.position;
    }


}
