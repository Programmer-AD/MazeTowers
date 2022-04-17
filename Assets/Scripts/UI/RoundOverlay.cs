using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundOverlay : MonoBehaviour
{
    private const string RoundStartFormat = "����� �{0} ����������";
    private const string RoundWinFormat = "����� �{0} �������!";
    private const string RoundLoseFormat = "����� �{0} ��������";

    [SerializeField] private TMP_Text text;
    [SerializeField] private float HideDelay;
    [SerializeField] private Color RoundStartColor;
    [SerializeField] private Color RoundWinColor;
    [SerializeField] private Color RoundLoseColor;


    private void Start()
    {
        SetShow(false);
    }

    public void SetShow(bool show)
    {
        gameObject.SetActive(show);
    }

    public void OnGameRoundStart(int roundNumber)
    {
        ShowWithRoundInfo(RoundStartFormat, RoundStartColor, roundNumber);
    }

    public void OnGameRoundEnd(int roundNumber, bool win)
    {
        if (win)
        {
            ShowWithRoundInfo(RoundWinFormat, RoundWinColor, roundNumber);
        }
        else
        {
            ShowWithRoundInfo(RoundLoseFormat, RoundLoseColor, roundNumber);
        }
    }

    private void ShowWithRoundInfo(string format, Color color, int roundNumber)
    {
        text.text = string.Format(format, roundNumber);
        text.color = color;
        SetShow(true);

        Invoke(nameof(Hide), HideDelay);
    }

    private void Hide()
    {
        SetShow(false);
    }
}
