using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timerDuration = 10.0f; // 타이머 기간(초)
    public float currentTime; // 현재 시간
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timerDuration;
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime; // 시간을 감소시킴

        // 시간이 0보다 작아지면 0으로 설정
        if (currentTime < 0)
        {
            currentTime = 0;
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        // 타이머 텍스트 업데이트
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // 거꾸로 출력
        timerText.text = seconds.ToString();
    }

    // 문자열을 거꾸로 만드는 함수
    string ReverseString(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}