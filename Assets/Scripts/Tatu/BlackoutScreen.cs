using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutScreen : MonoBehaviour
{
    [SerializeField] public GameObject blackoutSquare;
    [SerializeField] public GameObject whiteoutSquare;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FadeFromBlack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeFromBlack(float fadeSpeed = 0.15f) {
        
        float newFade;
        Color newColor;
        while (blackoutSquare.GetComponent<Image>().color.a > 0) {
            var color = blackoutSquare.GetComponent<Image>().color;
            newFade = color.a - (fadeSpeed * Time.deltaTime);
            newColor = new Color(color.r, color.g, color.b, newFade);
            blackoutSquare.GetComponent<Image>().color = newColor;
            yield return null;
        }
        yield return StartCoroutine(LightsOn());
    }

    public IEnumerator LightsOn(float fadeSpeed = 0.15f) {
        float newFade;
        Color newColor;
        whiteoutSquare.SetActive(true);
        while (whiteoutSquare.GetComponent<Image>().color.a < 0.7f) {
            var color = whiteoutSquare.GetComponent<Image>().color;
            newFade = color.a + (fadeSpeed * Time.deltaTime);
            newColor = new Color(color.r, color.g, color.b, newFade);
            whiteoutSquare.GetComponent<Image>().color = newColor;
            yield return null;
        }
    }
}
