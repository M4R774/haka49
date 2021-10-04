using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    public virtual void UseObject() {
        messageManager.DisplayDialogue(itemDescription);
        OnUseItem();
    }

    public virtual void OnUseItem() {
        messageManager.DisplayDialogue(itemDescription);
    }
}