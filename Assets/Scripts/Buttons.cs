using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    [SerializeField] private Button rockButton;
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;

    private List<Button> selectButtons = new();

    private void Awake()
    {
        selectButtons.Add(rockButton);
        selectButtons.Add(paperButton);
        selectButtons.Add(scissorsButton);
    }
    private void Start()
    {
        rockButton.onClick.AddListener(() => GameManager.Instance.SetPlayerSelect(GameManager.RPS.ROCK));
        paperButton.onClick.AddListener(() => GameManager.Instance.SetPlayerSelect(GameManager.RPS.PAPER));
        scissorsButton.onClick.AddListener(() => GameManager.Instance.SetPlayerSelect(GameManager.RPS.SCISSORS));
    }

    public void SetEnable()
    {
        foreach (var s in selectButtons)
        {
            s.interactable = true;
        }
    }

    public void SetDisable()
    {
        foreach (var s in selectButtons)
        {
            s.interactable = false;
        }
    }
}
