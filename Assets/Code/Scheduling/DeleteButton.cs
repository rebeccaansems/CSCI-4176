using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
public class DeleteButton : UIBehaviour
{
    [SerializeField]
    private Button remove = null;
    [SerializeField]
    private GameObject selectedItem;

	// Use this for initialization
	public void Start () {
        // Listerner for when scheduled item has delete button pressed
        remove.onClick.AddListener(DestroyItem);
		
	}

    public void DestroyItem()
    {
        // Get a reference to the list item being removed
        ListItem parts = selectedItem.GetComponent<ListItem>();
        // Remove list item from list of saved items
        Scheduler.savedItems.Remove(parts);
        // Remove the list item from the scene
        Destroy(selectedItem);
        

    }
}
