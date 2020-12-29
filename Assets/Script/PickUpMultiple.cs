using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PickUpMultiple : Objective
{
    public List<Transform> itemsToPick = new List<Transform>();
    
    public PickMode pickMode;
    
    public enum PickMode
    {
        Any,
        All
    }

    private new void Start()
    {
        base.Start();
        player = GameSingleton.instance.player;
    }

    private void Update()
    {
        for (int i = 0; i < itemsToPick.Count; i++)
        {
            if (itemsToPick[i] != null)
            {
                if (itemsToPick[i] == itemsToPick.Where(x => x != null).OrderBy(
                    t => Vector3.Distance(player.transform.position, t.position)).FirstOrDefault())
                    transform.position = itemsToPick[i].position;

               if (itemsToPick[i].IsChildOf(player.transform))
                    itemsToPick[i] = null;
            }
        }

        switch (pickMode)
        {
            case PickMode.Any:
                completed = itemsToPick.Any(t => t == null);
                break;
            case PickMode.All:
                completed = itemsToPick.All(t => t == null);
                break;
        }
    }
}
