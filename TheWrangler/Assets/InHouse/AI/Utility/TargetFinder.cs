using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public static TargetFinder Instance { get; private set; }

    public List<IInteractable> interactables { get; private set; } = new List<IInteractable>();
    private float updateInteractablesSeconds = 10f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persisence Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        UpdateInteractables();
        StartCoroutine(UpdateInteractables());
    }


    private IEnumerator UpdateInteractables()
    {
        while (true)
        {
            interactables = new List<IInteractable>(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IInteractable>());
            yield return new WaitForSeconds(updateInteractablesSeconds);
        }
    }
}
