using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FPSControllerLPFP;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//
// Tracks and changes player's status
// Statuses are things the player needs to get rid of
//

public enum Status
{
    needSunglasses,
    needPainkillers,
    needCoffee
}
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public List<Status> statusList;
    [SerializeField] DragRigidbodyUse dragRigidbodyUse;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI statusText;

    [Header("Tasks")]
    [SerializeField] bool areLightsOn;
    [SerializeField] GameObject lightImage;
    [SerializeField] bool isShaking;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] bool isInverted;
    [SerializeField] FpsControllerLPFP fpsController;
    [SerializeField] bool highHeartRate;
    [SerializeField] GameObject heart;

    [TextArea]
    [SerializeField] string lightsAreOnText;
    [TextArea]
    [SerializeField] string stillShakingText;
    [TextArea]
    [SerializeField] string controlsInvertedText;
    [SerializeField] GameObject endingScreen;
    [SerializeField] Clock clock;

    void Start()
    {
        InitStatusList();
        InitTasks();
    }

    void Update()
    {
        if (statusList.Count != 0)
            statusText.text = statusList[0].ToString();
        else
            statusText.text = "";
        
        CheckTasks();
    }

    void CheckTasks()
    {
        areLightsOn = lightImage.activeSelf;
        isShaking = cameraShake.enabled;
        isInverted = fpsController.toggleInversion;
    }

    public void InitStatusList() // public so can be called from outside if needed
    {
        statusList.Add(Status.needSunglasses);
        statusList.Add(Status.needPainkillers);
        statusList.Add(Status.needCoffee);
    }

    public void InitTasks()
    {
        areLightsOn = true; // this will always be true
        isShaking = cameraShake.enabled;
        isInverted = fpsController.toggleInversion;
    }

    public bool RemoveStatus(Status st)
    {
        if (HasStatus(st)) {
            statusList.Remove(st);
            dragRigidbodyUse.ObjectUsed();
            return true;
        } else {
            return false;
        }
    }

    public bool HasStatus(Status st)
    {
        return statusList.Contains(st);
    }

    public bool CanOpenDoor()
    {
        if(!isShaking && !isInverted && !areLightsOn)
            return true;
        else
        {
            return false;
        }
    }

    public void EndGame()
    {
        Debug.Log("You won!");
        StartCoroutine("EndingScreen");
    }
    IEnumerator EndingScreen()
    {
        TextMeshProUGUI victoryText = endingScreen.GetComponentInChildren<TextMeshProUGUI>();
        string endingText = "You Managed get to work! \n" +
            "You left home at: " + clock.hour + ":" + clock.minutes + ":" + clock.seconds + "\n";
        if (clock.hour > 7)
        {
            endingText += "\nYou were " + (((clock.hour-8)*60) + clock.minutes) + " minutes late.\n";
        }
        else
        {
            endingText += "\nYou on time! You even had " + (((clock.hour - 8) * -60) - clock.minutes) + " minutes left!\n";
        }
        endingScreen.SetActive(true);
        victoryText.text = endingText;
        for (float i = 0; i < 1; i += 0.01f)
        {
            endingScreen.GetComponent<Image>().color = new Color(1, 1, 1, i);
            victoryText.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenu");
    }

    public string TaskList()
    {
        string toDolist = "I can't leave yet.";
        if(areLightsOn)
        {
            toDolist = toDolist + "*" + lightsAreOnText;
        }
        if(isShaking)
        {
            toDolist = toDolist + "*" + stillShakingText;
        }
        if(isInverted)
        {
            toDolist = toDolist + "*" + controlsInvertedText;
        }

        return toDolist;
    }

}
