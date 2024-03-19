using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public RPS playerSelect;

    private RPS computerSelect;

    [System.Serializable]
    private class GameResources
    {
        [Header("Text Components")]
        [SerializeField] public TMP_Text scoreText;
        [SerializeField] public TMP_Text timerText;

        [Header("Sprites")]
        [SerializeField] public Sprite rockSprite;
        [SerializeField] public Sprite paperSprite;
        [SerializeField] public Sprite scissorsSprite;
        [SerializeField] public Sprite questionMarkSprite;

        [Header("Image Components")]
        [SerializeField] public Image playerImage;
        [SerializeField] public Image computerImage;

        [Header("Buttons")]
        //버튼 제어용 스크립트 가져오기
        [SerializeField] public Buttons _buttons;

        [Header("Result UI")]
        //승패 결과창
        [SerializeField] public GameObject resultUI;
    }

    [SerializeField] private GameResources _gameResources;
    
    private int playerScore = 0;
    private int computerScore = 0;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Start()
    {
        _gameResources.scoreText.text = playerScore.ToString() + " : " + computerScore.ToString();
        StartGame();
    }

    private void SetComputerSelect()
    {
        //랜덤함수 시드 변경
        float temp = Random.Range(0f, 1f) * 10000;
        Random.InitState((int)temp);

        //컴퓨터 랜덤 선택
        computerSelect = (RPS)(Random.Range(0, 3));
    }

    private IEnumerator GameTimer()
    {
        //타이머
        for(int i = 10; i > 0; i--)
        {
            _gameResources.timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        _gameResources.timerText.text = "0";

        //종료시 게임 종료 함수 호출
        EndTimer();
    }

    private void EndTimer()
    {
        //버튼 비활성화
        _gameResources._buttons.SetDisable();
        // 컴퓨터 선택에 따른 이미지 변경
        switch (computerSelect)
        {
            case RPS.ROCK:
                _gameResources.computerImage.sprite = _gameResources.rockSprite;
                break;
            case RPS.PAPER:
                _gameResources.computerImage.sprite = _gameResources.paperSprite;
                break;
            case RPS.SCISSORS:
                _gameResources.computerImage.sprite = _gameResources.scissorsSprite;
                break;
        }

        //플레이어가 버튼을 안눌렀을 경우 랜덤선택 후 결과 표시
        if(_gameResources.playerImage.sprite == _gameResources.questionMarkSprite)
        {
            //랜덤함수 시드 변경
            float temp = Random.Range(0f, 1f) * 10000;
            Random.InitState((int)temp);

            //플레이어 랜덤 선택
            playerSelect = (RPS)(Random.Range(0, 3));

            switch (playerSelect)
            {
                case RPS.ROCK:
                    _gameResources.playerImage.sprite = _gameResources.rockSprite;
                    break;
                case RPS.PAPER:
                    _gameResources.playerImage.sprite = _gameResources.paperSprite;
                    break;
                case RPS.SCISSORS:
                    _gameResources.playerImage.sprite = _gameResources.scissorsSprite;
                    break;
            }
        }

        //결과 UI 표시
        _gameResources.resultUI.SetActive(true);

        //승패판정
        if ((int)playerSelect - (int)computerSelect == 1 || (int)playerSelect - (int)computerSelect == -2)
        {
            _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "Player WIn";
            playerScore++;
        }
        else if((int)playerSelect - (int)computerSelect == 0)
        {
            _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "Draw";
        }
        else
        {
            _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "computer WIn";
            computerScore++;
        }
        Debug.Log(_gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text);
        _gameResources.scoreText.text = playerScore.ToString() + " : " + computerScore.ToString();

        //결과 표시 3초 후 게임 다시시작
        Invoke("StartGame", 3f);
    }

    private void StartGame()
    {
        _gameResources.resultUI.SetActive(false);
        //이미지 둘 다 물음표로 초기화
        _gameResources.computerImage.sprite = _gameResources.questionMarkSprite;
        _gameResources.playerImage.sprite = _gameResources.questionMarkSprite;

        //컴퓨터 랜덤선택
        SetComputerSelect();

        _gameResources._buttons.SetEnable();
        StartCoroutine(GameTimer());
    }

    //버튼 클릭시 플레이어 선택지 지정
    public void SetPlayerSelect(RPS _select)
    {
        playerSelect = _select;
        switch (_select)
        {
            case RPS.ROCK:
                _gameResources.playerImage.sprite = _gameResources.rockSprite;
                break;
            case RPS.PAPER:
                _gameResources.playerImage.sprite = _gameResources.paperSprite;
                break;
            case RPS.SCISSORS:
                _gameResources.playerImage.sprite = _gameResources.scissorsSprite;
                break;
        }
    }

    public enum RPS
    {
        ROCK,
        PAPER,
        SCISSORS
    }
}
