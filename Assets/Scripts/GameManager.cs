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
        //��ư ����� ��ũ��Ʈ ��������
        [SerializeField] public Buttons _buttons;

        [Header("Result UI")]
        //���� ���â
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
        //�����Լ� �õ� ����
        float temp = Random.Range(0f, 1f) * 10000;
        Random.InitState((int)temp);

        //��ǻ�� ���� ����
        computerSelect = (RPS)(Random.Range(0, 3));
    }

    private IEnumerator GameTimer()
    {
        //Ÿ�̸�
        for(int i = 10; i > 0; i--)
        {
            _gameResources.timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        _gameResources.timerText.text = "0";

        //����� ���� ���� �Լ� ȣ��
        EndTimer();
    }

    private void EndTimer()
    {
        //��ư ��Ȱ��ȭ
        _gameResources._buttons.SetDisable();
        // ��ǻ�� ���ÿ� ���� �̹��� ����
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

        //�÷��̾ ��ư�� �ȴ����� ��� �������� �� ��� ǥ��
        if(_gameResources.playerImage.sprite == _gameResources.questionMarkSprite)
        {
            //�����Լ� �õ� ����
            float temp = Random.Range(0f, 1f) * 10000;
            Random.InitState((int)temp);

            //�÷��̾� ���� ����
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

        //��� UI ǥ��
        _gameResources.resultUI.SetActive(true);

        //��������
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

        //��� ǥ�� 3�� �� ���� �ٽý���
        Invoke("StartGame", 3f);
    }

    private void StartGame()
    {
        _gameResources.resultUI.SetActive(false);
        //�̹��� �� �� ����ǥ�� �ʱ�ȭ
        _gameResources.computerImage.sprite = _gameResources.questionMarkSprite;
        _gameResources.playerImage.sprite = _gameResources.questionMarkSprite;

        //��ǻ�� ��������
        SetComputerSelect();

        _gameResources._buttons.SetEnable();
        StartCoroutine(GameTimer());
    }

    //��ư Ŭ���� �÷��̾� ������ ����
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
