using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MessageTutorial : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speedRunText = 0.05f;
    [SerializeField] private float timeDelayNextMessage = 0.5f;
    [SerializeField] private bool useVietnamese;

    [Header("UI")]
    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private RectTransform popupTransform;
    [SerializeField] private TextMeshProUGUI txtMessage;
    [SerializeField] private GameObject objAvt;

    [Header("Messages")]
    [TextArea(2, 3)]
    public string[] messagesEnglish;
    [TextArea(2, 3)]
    public string[] messagesVietnamese;

    private string[] currentMessage;
    private int indexMessage = 0;
    public int IndexMessage => indexMessage;

    private bool isTyping = false;
    private bool canShow = false;
    private Coroutine typingCoroutine;
    private Action actionDone;

    private bool isWaitingInput = false; // << mới thêm: nếu đang yêu cầu input thì ko click

    private void Start()
    {
        popupCanvasGroup.alpha = 0;
        popupTransform.localScale = Vector3.zero;
        currentMessage = useVietnamese ? messagesVietnamese : messagesEnglish;
        actionDone = ActionDoneTutorial;
    }

    public void ShowDisplayDialog()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            canShow = true;
            ShowMessage();
        });
    }

    private void Update()
    {
        if (!canShow) return;

        if (isWaitingInput)
        {
            if (CheckInputForStep(indexMessage))
            {
                isWaitingInput = false;
                NextMessage();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteTyping();
            }
            else
            {
                NextMessage();
            }
        }
    }

    private void ShowMessage()
    {
        if (indexMessage >= currentMessage.Length) return;

        popupCanvasGroup.gameObject.SetActive(true);
        popupCanvasGroup.DOFade(1, 0.5f);
        popupTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            objAvt.SetActive(true);
            typingCoroutine = StartCoroutine(TypeText(speedRunText));
        });

        // Nếu bước yêu cầu input thì bật flag
        if (NeedInput(indexMessage))
        {
            isWaitingInput = true;
        }
        else
        {
            isWaitingInput = false;
        }
    }

    private IEnumerator TypeText(float timer)
    {
        isTyping = true;
        txtMessage.text = "";
        string message = currentMessage[indexMessage];

        foreach (char c in message)
        {
            txtMessage.text += c;
            yield return new WaitForSeconds(timer);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        txtMessage.text = currentMessage[indexMessage];
        isTyping = false;
    }

    private void NextMessage()
    {
        indexMessage++;

        if (indexMessage >= currentMessage.Length)
        {
            popupCanvasGroup.DOFade(0, 0.5f).OnStart(() => objAvt.SetActive(false))
                .OnComplete(() =>
                {
                    popupTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
                    actionDone?.Invoke();
                    popupCanvasGroup.gameObject.SetActive(false);
                });
        }
        else
        {
            StartCoroutine(DelayNextMessage());
        }
    }

    private IEnumerator DelayNextMessage()
    {
        yield return new WaitForSeconds(timeDelayNextMessage);
        ShowMessage();
    }

    private bool NeedInput(int step)
    {
        return (step == 3 || step == 4 || step == 5);
    }

    private bool CheckInputForStep(int step)
    {
        switch (step)
        {
            case 3:
                return Input.GetKeyDown(KeyCode.A);
            case 4:
                return Input.GetKeyDown(KeyCode.D);
            case 5:
                return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
            default:
                return false;
        }
    }

    private void ActionDoneTutorial()
    {
        canShow = false;
        GameManager.Instance.IsFirstPlay = false;
        GameManager.Instance.State = StateGame.Playing;
    }
}
