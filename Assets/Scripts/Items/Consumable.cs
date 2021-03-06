using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Usable
{
    protected float lerpDuration = 0.25f;
    protected Coroutine itemPickUpCoroutine;

    public override void UseObject()
    {
        base.UseObject();
        if(itemPickUpCoroutine == null)
            itemPickUpCoroutine = StartCoroutine(ItemPickUp(Camera.main.transform.position - new Vector3(0,0.05f,0), lerpDuration));
    }
    private IEnumerator ItemPickUp(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = transform.localScale * 0.15f;
        this.GetComponent<Collider>().enabled = false;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;  
        }
        transform.position = targetPosition;
        transform.localScale = targetScale;
        Destroy(this.gameObject);
        yield return null;
    }
}