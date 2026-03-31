using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private InteractableObjectList interactableObjList;

    private void Start()
    {
        interactableObjList = new InteractableObjectList();
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        // TODO: 플레이어 및 상호작용 대상 좌표 계산 후 최근접 찾기
        if (interactableObjList.objectsList.Count <= 0)
        {
            return;
        }

        GetNearestInteractable();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            foreach (var a in interactableObjList.objectsList)
            {
                a.Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            return;
        }

        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactableObjList.AddInteractable(interactable);

            Debug.Log($"추가: {interactableObjList.objectsList.Count}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            return;
        }

        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactableObjList.RemoveInteractable(interactable);

            Debug.Log($"제거: {interactableObjList.objectsList.Count}");
        }
    }

    private IInteractable GetNearestInteractable()
    {
        float nearestDistance = float.MaxValue;

        IInteractable nearest = null;

        Transform nearestPos = null;

        foreach (var interactable in interactableObjList.objectsList)
        {
            if (interactable is MonoBehaviour monoBehaviour)
            {
                float distance = Vector3.Distance(gameObject.transform.position, monoBehaviour.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;

                    nearest = interactable;

                    nearestPos = monoBehaviour.transform;
                }
            }
        }

        Debug.Log(nearestPos.position);

        return nearest;
    }



    // Gizmos
    private void OnDrawGizmos()
    {
        if (TryGetComponent<SphereCollider>(out var sphere) == false)
        {
            return;
        }

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);

        Gizmos.DrawSphere(sphere.center, sphere.radius);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(sphere.center, sphere.radius);
    }
}
