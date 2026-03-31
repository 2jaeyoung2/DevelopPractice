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

    // null 정리
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

    public void ShowAllInteractables()
    {
        foreach (var obj in objectsList)
        {
            // TODO: 범위 내에 아이템이나 상호작용 요소가 많을 때 리스트 보여주기
        }
    }
}
