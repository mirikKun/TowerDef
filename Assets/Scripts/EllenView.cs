using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
