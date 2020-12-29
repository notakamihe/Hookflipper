using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public Text title;
    public Text description;

    private new Animation animation;

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if (!animation.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }
}
