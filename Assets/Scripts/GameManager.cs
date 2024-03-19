using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;


    private RPS playerCard;
    private RPS computerCard;

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

    // 리소스 및 각종 컴포넌트들 저장용 클래스
    [SerializeField] private GameResources _gameResources;

    //FSM용 상태 저장 변수
    //근데 FSM 이렇게쓰는게 맞는건지 모르겠음
    [SerializeField] private State _gameState;
    
    //플레이어, 컴퓨터 점수 저장용
    private int playerScore = 0;
    private int computerScore = 0;

    private readonly float endTime = 10f;       //타이머 시간 지정(C#은 define이 없음....)

    //Timer 변수
    private float timer;    

    //Singleton 패턴
    #region Singleton
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

    #endregion
    private void Update()
    {
        switch(_gameState)
        {
            case State.TIMER:
                timer -= Time.deltaTime;
                _gameResources.timerText.text = ((int)timer).ToString();
                if (timer <= 0f)
                {
                    //타이머 작동 종료시 결과 상태로 전환
                    _gameState = State.RESULT;
                }
                break;
            case State.RESULT:
                EndTimer();
                break;
            default:
                break;
        }

    }

    private void Start()
    {
        //점수 텍스트 초기화
        _gameResources.scoreText.text = playerScore.ToString() + " : " + computerScore.ToString();
        //게임 시작
        StartGame();
    }
    private void StartGame()
    {
        //결과창 UI 비활성화
        _gameResources.resultUI.SetActive(false);

        //이미지 둘 다 물음표로 초기화
        _gameResources.computerImage.sprite = _gameResources.questionMarkSprite;
        _gameResources.playerImage.sprite = _gameResources.questionMarkSprite;

        //컴퓨터 랜덤선택
        SetRandomSelect(computerCard);

        _gameResources._buttons.SetEnable();
        timer = endTime;
        //현재 상태 : 타이머 작동
        _gameState = State.TIMER;
    }
    
    private void EndTimer()
    {
        //현재 상태 : 대기중
        _gameState = State.WAIT;

        //버튼 비활성화
        _gameResources._buttons.SetDisable();

        // 컴퓨터 선택에 따른 이미지 변경
        switch (computerCard)
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
            SetRandomSelect(playerCard);

            switch (playerCard)
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

        //결과 UI Active
        _gameResources.resultUI.SetActive(true);

        //enum 타입의 인덱스 뺄셈을 통해 승패 판정
        switch((int)playerCard - (int)computerCard)
        {
            case 1:
            case -2:
                _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "Player WIn";
                playerScore++;
                break;
            case 0:
                _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "Draw";
                break;
            default:
                _gameResources.resultUI.transform.GetComponentInChildren<TMP_Text>().text = "computer WIn";
                computerScore++;
                break;
        }

        _gameResources.scoreText.text = $"{playerScore} : {computerScore}";

        //결과 표시 3초 후 게임 다시시작
        Invoke(nameof(StartGame), 3f);
    }

    //버튼 클릭시 플레이어 선택지 지정
    public void SetPlayerSelect(RPS _select)
    {
        playerCard = _select;
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


    private void SetRandomSelect(RPS _rps)
    {
        //랜덤함수 시드 변경
        float temp = Random.Range(0f, 1f) * 10000;
        Random.InitState((int)temp);

        //컴퓨터 랜덤 선택
        _rps = (RPS)(Random.Range(0, 3));
    }

    public enum RPS
    {
        ROCK,
        PAPER,
        SCISSORS
    }

    public enum State
    {
        TIMER,
        RESULT,
        WAIT
    }
}
