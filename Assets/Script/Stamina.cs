using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float drainSpeed = -4f;
    public float rechargeSpeed = 5f;
    [HideInInspector] public float stamina { get; private set; }

    private float depleteTime = 0;
    private bool doneStayingDepleted = true;

    void Start ()
    {
        stamina = maxStamina;
    }

    private void Update()
    {
        if (IsDepleted() && doneStayingDepleted)
        {
            doneStayingDepleted = false;
            depleteTime = Time.time;
        }
    }

    public void DecreaseStamina (float speed)
    {
        float speedDelta = speed * Time.deltaTime;
        stamina = stamina - speedDelta > 0 ? stamina - speedDelta : 0;
    }

    public void IncreaseStamina (float speed)
    {
        if (depleteTime == 0 || Time.time > depleteTime + 5f)
        {
            doneStayingDepleted = true;
            float speedDelta = speed * Time.deltaTime;
            stamina = stamina + speedDelta < maxStamina ? stamina + speedDelta : maxStamina;
        }
    }

    public bool IsDepleted () => stamina <= 0;
}
