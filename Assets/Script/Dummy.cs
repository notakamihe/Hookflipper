using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (alarmState == AlarmState.Dead)
        {
            animator.SetBool("IsDead", true);
            this.enabled = false;
        }
    }
}
