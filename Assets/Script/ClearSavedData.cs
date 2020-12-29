using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClearSavedData : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private Button confirmationButton;

    private void Start()
    {
        confirmationButton.gameObject.SetActive(false);
    }
    
    private IEnumerator DeactivateConfirm (float delay)
    {
        yield return new WaitForSeconds(delay);
        confirmationButton.gameObject.SetActive(false);
    }

    public void DeleteSavedData ()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnDeselect (BaseEventData data)
    {
        StartCoroutine(DeactivateConfirm(0.2f));
    }

    public void OpenConfirm ()
    {
        confirmationButton.gameObject.SetActive(true);
    }
}