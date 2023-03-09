using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopePlacementController : MonoBehaviour
{

    public void SelectObject(RaycastHit hit)
    {
        if (!hit.collider.gameObject.CompareTag("Object")) return;
        PlayerData.Instance.selectedGameObject = hit;
        hit.collider.gameObject.GetComponentInChildren<ObjectMechanicsController>().SetSelected();

        InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.ObjectSelected;
    }

    public void SelectSurface(RaycastHit hit)
    {
        if (PlayerData.Instance.selectedGameObject.collider.gameObject.Equals(hit.collider.gameObject)) return;
            
        PlayerData.Instance.activeRopes.AddLast(Instantiate(PlayerData.Instance.ropePrefab));
            
        GameObject lastRopeObject = PlayerData.Instance.TryPeekActiveRope();
        Rope lastRope = lastRopeObject.GetComponent<Rope>();
        lastRope.PlaceEnd(PlayerData.Instance.selectedGameObject);

        lastRope.PlaceEnd(hit);
    }

    public void DestroyOldestBatch()
    {
        LinkedList<GameObject> previousRopeStack = PlayerData.Instance.placedRopes.First.Value;
        PlayerData.Instance.placedRopes.RemoveFirst();
        DestroyRopeBatch(previousRopeStack);
        
        UpdateRopeColor();
            
        InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
    }

    public void DestroyNewestBatch()
    {
        LinkedList<GameObject> previousRopeStack = PlayerData.Instance.placedRopes.Last.Value;
        PlayerData.Instance.placedRopes.RemoveLast();
        DestroyRopeBatch(previousRopeStack);
            
        UpdateRopeColor();

        InstructionGroup.Instance.CurrentState = InstructionGroup.DisplayState.NotAiming;
    }

    public void ConfirmPlacement()
    {
        foreach (var rope in PlayerData.Instance.activeRopes.Select(ropeObject => ropeObject.GetComponent<Rope>()))
        {
            rope.CreateJoint();
        }

        PlayerData.Instance.placedRopes.AddLast(new LinkedList<GameObject>(PlayerData.Instance.activeRopes));
        PlayerData.Instance.activeRopes.Clear();

        UpdateRopeColor();
    }
    
    private void DestroyRopeBatch(LinkedList<GameObject> linkedList)
    {
        while (linkedList.Count > 0)
        {
            GameObject ropeObject = linkedList.Last.Value;
            Rope rope = ropeObject.GetComponent<Rope>();

            if (rope.joint) Destroy(rope.joint);
            if (rope.attachmentPoint) Destroy(rope.attachmentPoint);
            Destroy(ropeObject);
            linkedList.RemoveLast();
        }
    }

    private void UpdateRopeColor()
    {
        int i = 0;
        int lastIndex = PlayerData.Instance.placedRopes.Count - 1;
        foreach (var batch in PlayerData.Instance.placedRopes)
        {
            foreach (var rope in batch.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                if (i == 0) rope.SetColor(1);
                else if (i == lastIndex) rope.SetColor(2);
                else rope.SetColor(0);
            }
            i++;
        }
        
        if (PlayerData.Instance.placedRopes.First != null)
        {
            foreach (var rope in PlayerData.Instance.placedRopes.First.Value.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                rope.SetColor(1);
            }
        }
            
        if (PlayerData.Instance.placedRopes.Last != null)
        {
            foreach (var rope in PlayerData.Instance.placedRopes.Last.Value.Select(ropeObject => ropeObject.GetComponent<Rope>()))
            {
                rope.SetColor(2);
            }
        }
    }
}
