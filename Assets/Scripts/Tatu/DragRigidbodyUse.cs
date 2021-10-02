/*
DragRigidbodyUse.cs ver. 11.1.16 - wirted by ThunderWire Games * Script for Drag & Drop & Throw Objects & Draggable Door & PickupObjects
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	[System.Serializable]
	public class GrabObjectClass
	{
		public bool m_FreezeRotation;
		public float m_PickupRange = 3f; 
		public float m_ThrowStrength = 50f;
		public float m_distance = 3f;
		public float m_maxDistanceGrab = 4f;
	}
	
	[System.Serializable]
	public class ItemGrabClass
	{
		public bool m_FreezeRotation;
		public float m_ItemPickupRange = 2f;
		public float m_ItemThrow = 45f;
		public float m_ItemDistance = 1f;
		public float m_ItemMaxGrab = 2.5f;
	}
	
	[System.Serializable]
	public class DoorGrabClass
	{	
		public float m_DoorPickupRange = 2f;
		public float m_DoorThrow = 10f;
		public float m_DoorDistance = 2f;
		public float m_DoorMaxGrab = 3f;
	}
	
	[System.Serializable]
	public class TagsClass
	{
		public string m_InteractTag = "Interact";
		public string m_InteractItemsTag = "InteractItem"; 
		public string m_DoorsTag = "Door"; 
		public string m_NoteTag = "Note";
		public string m_SwitchTag = "Switch";
	}

public class DragRigidbodyUse : MonoBehaviour
{
	
	public GameObject playerCam;
	
	public string GrabButton = "Grab";
	public string ThrowButton = "Throw";
	public string UseButton = "Use";
	public GrabObjectClass ObjectGrab = new GrabObjectClass();
	public ItemGrabClass ItemGrab = new ItemGrabClass();
	public DoorGrabClass DoorGrab = new DoorGrabClass();
	public ItemGrabClass NoteGrab = new ItemGrabClass();
	public TagsClass Tags = new TagsClass();
	
	private float PickupRange = 3f;
	private float ThrowStrength = 50f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;
	
	private Ray playerAim;
	private GameObject objectHeld;
	private bool isObjectHeld;
	private bool tryPickupObject;

	[SerializeField] LayerMask layerMask;
	
	void Start ()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		objectHeld = null;
	}

	/*void Update()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;

			if (Physics.Raycast (playerAim, out hit, PickupRange, layerMask))
			{
				GameObject objectHit = hit.collider.gameObject;

				if(hit.collider.tag == Tags.m_SwitchTag)
				{
					objectHit.GetComponent<Switch>().Use();
				}
			}
		}
	}*/
	
	void Update ()
	{
		if(Input.GetButton(GrabButton))
		{
			if(!isObjectHeld)
			{
				TryPickObject();
				tryPickupObject = true;
			}
			else
			{
				if(objectHeld != null)
				{
					if(objectHeld.tag == Tags.m_DoorsTag)
						OpenDoor();
					else if(objectHeld.tag == Tags.m_NoteTag)
						HoldNote();
					else
					{
						HoldObject();
					}
				}
			}
		}
		else if(isObjectHeld)
		{
			if(objectHeld != null)
				DropObject();
			else
			{
				isObjectHeld = false;
				tryPickupObject = false;
			}
		}
		
		if(Input.GetButton(ThrowButton) && isObjectHeld)
		{
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}
		
		if(Input.GetButtonDown(UseButton)) //&& !objectHeld) // Uses item held in hand atm
		{
			if(objectHeld != null)
			{
				//isObjectHeld = false;
				//TryPickObject();
				//tryPickupObject = false;
				Use();
			}
			else
			{
				Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
				RaycastHit hit;
				
				if (Physics.Raycast (playerAim, out hit, PickupRange, layerMask))
				{
					hit.collider.gameObject.SendMessage ("UseObject",SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	
	private void TryPickObject()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if (Physics.Raycast (playerAim, out hit, PickupRange, layerMask))
		{
			objectHeld = hit.collider.gameObject;
			//Debug.Log(objectHeld);

			// Props that are movable
			if(hit.collider.tag == Tags.m_InteractTag && tryPickupObject)
			{
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
				if(ObjectGrab.m_FreezeRotation)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
				}
				if(ObjectGrab.m_FreezeRotation == false)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				}
				PickupRange = ObjectGrab.m_PickupRange; 
				ThrowStrength = ObjectGrab.m_ThrowStrength;
				distance = ObjectGrab.m_distance;
				maxDistanceGrab = ObjectGrab.m_maxDistanceGrab;
			}

			// Items
			if(hit.collider.tag == Tags.m_InteractItemsTag && tryPickupObject)
			{
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
				if(ItemGrab.m_FreezeRotation)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
				}
				if(ItemGrab.m_FreezeRotation == false)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				}
				PickupRange = ItemGrab.m_ItemPickupRange; 
				ThrowStrength = ItemGrab.m_ItemThrow;
				distance = ItemGrab.m_ItemDistance;
				maxDistanceGrab = ItemGrab.m_ItemMaxGrab;

				//ActivateHighLight();
			}

			// Post it notes
			if(hit.collider.tag == Tags.m_NoteTag && tryPickupObject)
			{
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
				if(ItemGrab.m_FreezeRotation)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
				}
				if(ItemGrab.m_FreezeRotation == false)
				{
					objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				}
				PickupRange = NoteGrab.m_ItemPickupRange; 
				ThrowStrength = NoteGrab.m_ItemThrow;
				distance = NoteGrab.m_ItemDistance;
				maxDistanceGrab = NoteGrab.m_ItemMaxGrab;
			}

			// Doors
			if(hit.collider.tag == Tags.m_DoorsTag && tryPickupObject)
			{
				isObjectHeld = true;
				//objectHeld.GetComponent<Rigidbody>().useGravity = true;
				//objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				// these are already true
				
				PickupRange = DoorGrab.m_DoorPickupRange; 
				ThrowStrength = DoorGrab.m_DoorThrow;
				distance = DoorGrab.m_DoorDistance;
				maxDistanceGrab = DoorGrab.m_DoorMaxGrab;
			}
		}
	}
	
	private void HoldObject()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		Vector3 currPos = objectHeld.transform.position;
		
		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;
		
        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            DropObject();
        }
	}
	private void HoldNote()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		Vector3 currPos = objectHeld.transform.position;
		
		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

		objectHeld.transform.LookAt(this.transform);
		
        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            DropObject();
        }
	}

	private void OpenDoor()
	{
		// Door object pivot is best placed where the player is supposedly going to grab the door from
		// ie. the handle
		// Otherwise the interaction feels even jankier
		objectHeld.SendMessage("Open",SendMessageOptions.DontRequireReceiver);

		Openable openable = objectHeld.GetComponent<Openable>();

		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		float dist = Vector3.Distance(objectHeld.transform.position, playerCam.transform.position);

		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		//Vector3 nextPos = playerCam.transform.position + playerAim.direction; // * dist;
		Vector3 currPos = objectHeld.transform.position;
		
		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 50;
		
        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            DropObject();
        }
	}
	
    private void DropObject() 
    {
		isObjectHeld = false;
		tryPickupObject = false;
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		//DeactivateHighLight();
		objectHeld = null;
    }
	
    private void ThrowObject()
    {
        objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength);
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		//DeactivateHighLight();
		objectHeld = null;
    }
	
    private void Use()
    {

		if(objectHeld != null && objectHeld.GetComponent<Item>()) // && objectHeld.GetComponent<Item>().usable) // This caused a bug with items that are not usable but emit a message
		{
			Debug.Log(objectHeld + "UseObject should be called now");
			objectHeld.SendMessage ("UseObject",SendMessageOptions.DontRequireReceiver); //Every script attached to the PickupObject that has a UseObject function will be called.
		}
    }

	private void ActivateHighLight()
	{
		if(objectHeld.GetComponent<Item>() && objectHeld.GetComponent<Item>().usable)
		{
			Color objectColor = objectHeld.GetComponent<Renderer>().materials[1].color;
			//Debug.Log(objectColor);
			objectColor.a = 0.5f;
			objectHeld.GetComponent<Renderer>().materials[1].color = objectColor;
		}
	}
	private void DeactivateHighLight()
	{
		if(objectHeld.GetComponent<Item>() && objectHeld.GetComponent<Item>().usable)
		{
			Color objectColor = objectHeld.GetComponent<Renderer>().materials[1].color;
			//Debug.Log(objectColor);
			objectColor.a = 0f;
			objectHeld.GetComponent<Renderer>().materials[1].color = objectColor;
		}
	}

	public void ObjectUsed() // PlayerStatus calls this when object is destroyed
	{
		objectHeld = null;
		isObjectHeld = false;
		tryPickupObject = false;
	}
}