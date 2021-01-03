using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStick : ObstacleController
{
    public Vector2 indent;
    protected override void Movement()
    {
        float step = Game.GameSpeed * Time.deltaTime;
        transform.position = new Vector2(GetCurrentPositionStick().x - indent.x, transform.position.y - step);
    }

    protected Vector2 GetCurrentPositionStick()
    {
        if (lastGoal) return new Vector2(transform.position.x, -20);
        return transform.parent.transform.position;
    }
}
