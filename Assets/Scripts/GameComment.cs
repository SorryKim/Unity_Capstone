using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameComment : MonoBehaviourPunCallbacks
{
    GameSystem gameSystem;
    public static GameComment instance;

    // 발표 순서
    public List<int> commentSequence;
    public GameObject commentWaitPanel, commentPanel;
    public TMP_InputField commentInput;
    public Text[] comments;

    // 발표 여부
    private bool isCommentAllowed = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        gameSystem = GetComponent<GameSystem>();
        commentInput.onEndEdit.AddListener(OnInputEndEdit);

    }


    // 발표 순서를 정하는 메소드
    public void SetSequence()
    {
        bool[] check = new bool[gameSystem.players.Count];
        for (int i = 0; i < check.Length; i++)
            check[i] = false;

        // 누구부터 시작할지

        int startIdx = UnityEngine.Random.Range(0, gameSystem.players.Count);


        // 시작한사람부터 차례대로
        for (int i = startIdx; i < gameSystem.players.Count; i++)
        {
            commentSequence.Add(i);
        }
        for (int i = 0; i < startIdx; i++)
        {
            commentSequence.Add(i);
        }
    }


    // 발표 시작
    public void StartComment()
    {

        SetSequence();
        foreach (var num in commentSequence)
        {
            Player player = gameSystem.players[num];
            Commenting(player);
        }
    }

    // 발표 메소드
    void Commenting(Player player)
    {
        // 발표자
        if (photonView.IsMine)
        {

            // 코멘트 인풋창 활성화
            commentPanel.SetActive(true);
            StartInput();


        }
        // 발표가 아닌 사람들
        else
        {
            // 코멘트 대기 패널 활성화
            commentWaitPanel.SetActive(true);
        }
    }

    private void OnInputEndEdit(string inputText)
    {
        StopInput();
        commentPanel.SetActive(true);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(30);
        if (isCommentAllowed)
        {
            StopInput();
            // 시간 초과 처리
            // 다음 단계로 이동하거나 다른 처리를 수행
        }
    }

    public void StartInput()
    {
        isCommentAllowed = true;
        StartCoroutine(StartTimer());
    }

    public void StopInput()
    {
        isCommentAllowed = false;
        commentPanel.SetActive(false);
        StopCoroutine(StartTimer());
    }
}