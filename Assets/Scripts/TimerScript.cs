using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    public int seconds, minutes;
    // Start is called before the first frame update
    void Start()
    {
        AddtoSecond();
    }
    private void AddtoSecond()
    {
        seconds++;
        if(seconds > 59)
        {
            minutes++;
            seconds = 0;
        }

        timeText.text = (minutes < 10?"0":"") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        Invoke(nameof(AddtoSecond), 1);
    }

    public void StopTimer()
    {
        CancelInvoke(nameof(AddtoSecond));
        timeText.gameObject.SetActive(false);
    }
}
