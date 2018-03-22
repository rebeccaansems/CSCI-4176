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
        remove.onClick.AddListener(() => Destroy(selectedItem));
		
	}
}
