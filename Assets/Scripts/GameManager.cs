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
        //��ư ����� ��ũ��Ʈ ��������
        [SerializeField] public Buttons _buttons;

        [Header("Result UI")]
        //���� ���â
        [SerializeField] public GameObject resultUI;
    }

    // ���ҽ� �� ���� ������Ʈ�� ����� Ŭ����
    [SerializeField] private GameResources _gameResources;

    //FSM�� ���� ���� ����
    //�ٵ� FSM �̷��Ծ��°� �´°��� �𸣰���
    [SerializeField] private State _gameState;
    
    //�÷��̾�, ��ǻ�� ���� �����
    private int playerScore = 0;
    private int computerScore = 0;

    private readonly float endTime = 10f;       //Ÿ�̸� �ð� ����(C#�� define�� ����....)

    //Timer ����
    private float timer;    

    //Singleton ����
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
                    //Ÿ�̸� �۵� ����� ��� ���·� ��ȯ
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
        //���� �ؽ�Ʈ �ʱ�ȭ
        _gameResources.scoreText.text = playerScore.ToString() + " : " + computerScore.ToString();
        //���� ����
        StartGame();
    }
    private void StartGame()
    {
        //���â UI ��Ȱ��ȭ
        _gameResources.resultUI.SetActive(false);

        //�̹��� �� �� ����ǥ�� �ʱ�ȭ
        _gameResources.computerImage.sprite = _gameResources.questionMarkSprite;
        _gameResources.playerImage.sprite = _gameResources.questionMarkSprite;

        //��ǻ�� ��������
        SetRandomSelect(computerCard);

        _gameResources._buttons.SetEnable();
        timer = endTime;
        //���� ���� : Ÿ�̸� �۵�
        _gameState = State.TIMER;
    }
    
    private void EndTimer()
    {
        //���� ���� : �����
        _gameState = State.WAIT;

        //��ư ��Ȱ��ȭ
        _gameResources._buttons.SetDisable();

        // ��ǻ�� ���ÿ� ���� �̹��� ����
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

        //�÷��̾ ��ư�� �ȴ����� ��� �������� �� ��� ǥ��
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

        //��� UI Active
        _gameResources.resultUI.SetActive(true);

        //enum Ÿ���� �ε��� ������ ���� ���� ����
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

        //��� ǥ�� 3�� �� ���� �ٽý���
        Invoke(nameof(StartGame), 3f);
    }

    //��ư Ŭ���� �÷��̾� ������ ����
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
        //�����Լ� �õ� ����
        float temp = Random.Range(0f, 1f) * 10000;
        Random.InitState((int)temp);

        //��ǻ�� ���� ����
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
