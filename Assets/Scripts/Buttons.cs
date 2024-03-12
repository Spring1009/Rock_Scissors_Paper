using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    [SerializeField] private Button[] selectButtons;

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
    public void OnClickRockButton()
    {
        GameManager.Instance.SetPlayerSelect(GameManager.RPS.ROCK);
    }

    public void OnClickPaperButton()
    {
        GameManager.Instance.SetPlayerSelect(GameManager.RPS.PAPER);
    }

    public void OnClickScissorsButton()
    {
        GameManager.Instance.SetPlayerSelect(GameManager.RPS.SCISSORS);
    }
}
