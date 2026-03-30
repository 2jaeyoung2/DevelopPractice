using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectList
{
    public List<IInteractable> objectsList = new List<IInteractable>();

    public void AddInteractable(IInteractable newInteractable)
    {
        if (objectsList.Contains(newInteractable) == false)
        {
            objectsList.Add(newInteractable);
        }
    }

    public void RemoveInteractable(IInteractable oldInteractable)
    {
        if (oldInteractable == null)
        {
            return;
        }

        objectsList.Remove(oldInteractable);
    }

    // null Á¤¸®
    public void CleanupNull()
    {
        for (int i = objectsList.Count - 1; i >= 0; i--)
        {
            if (objectsList[i] == null)
            {
                objectsList.RemoveAt(i);
            }
        }
    }
}
