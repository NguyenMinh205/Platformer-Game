using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

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

    //[Header("Audio")]
    //[SerializeField] private AudioClip[] audioSpeak;
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioSource audioBG;

    [Header("Messages")]
    [TextArea(2, 3)]
    public string[] messagesEnglish = new[]
    {
        "My food supplies are almost gone, and winter is closing in.",
        "If I don't collect those fruits in time, I won’t survive the cold season.",
        "Ah, hello there… the one watching me from the screen!",
        "Can you help me gather some fruits to prepare for the winter?",
        "Press A to move left.",
        "Press D to move right.",
        "Press W or Space to jump.",
        "Press W or Space twice to perform a double jump."
    };

    [TextArea(2, 3)]
    public string[] messagesVietnamese = new[]
    {
        "Kho thức ăn của tôi gần như cạn kiệt, trong khi mùa đông đang đến gần.",
        "Nếu không kịp thu thập những loại trái cây kia, tôi sẽ chẳng còn gì để sống sót qua mùa đông.",
        "A, xin chào... người đang nhìn vào màn hình!",
        "Bạn có thể giúp tôi thu thập trái cây để dự trữ qua mùa đông không?",
        "Hãy nhấn phím A để di chuyển sang trái.",
        "Hãy nhấn phím D để di chuyển sang phải.",
        "Nhấn W hoặc Space để nhảy.",
        "Nhấn W hoặc Space hai lần để thực hiện cú nhảy kép (Double Jump)."
    };

    private string[] currentMessage;
    private int indexMessage = 0;
    public int IndexMessage => indexMessage;

    private bool isTyping = false;
    private bool canProceed = false;
    private bool canShow = false;
    private Coroutine typingCoroutine;
    private Action actionDone;

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
            //audioBG.Stop();
            canShow = true;
            ShowMessage();
        });
    }

    private void Update()
    {
        if (!canShow) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteTyping();
            }
            else if (indexMessage < 4)
            {
                NextMessage();
            }
        }

        if (indexMessage >= 4 && !isTyping && CheckConditionForStep(indexMessage))
        {
            PlayTutorialStep(indexMessage);
            NextMessage();
        }
    }

    private void ShowMessage()
    {
        if (indexMessage >= currentMessage.Length) return;

        popupCanvasGroup.DOFade(1, 0.5f);
        popupTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            objAvt.SetActive(true);
            typingCoroutine = StartCoroutine(ProcessingWriteText(speedRunText));
        });

        if (indexMessage >= 4 && indexMessage <= 7)
        {
            float direction = (indexMessage == 4) ? -1f : 1f;
            PlayerController.Instance.PlayerAnim.PlayAnimRun(direction);
            PlayerController.Instance.PlayerMove.Move(direction, 0f);
        }
        else
        {
            PlayerController.Instance.PlayerAnim.PlayAnimRun(0f);
        }
    }

    IEnumerator ProcessingWriteText(float timer)
    {
        isTyping = true;
        canProceed = false;
        txtMessage.text = "";
        string message = currentMessage[indexMessage];

        foreach (char c in message)
        {
            txtMessage.text += c;
            //if (audioSpeak.Length > 0)
            //    audioSource.PlayOneShot(audioSpeak[Random.Range(0, audioSpeak.Length)]);
            yield return new WaitForSeconds(timer);
        }

        isTyping = false;
        canProceed = true;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            //audioSource.Stop();
        }

        txtMessage.text = currentMessage[indexMessage];
        isTyping = false;
        canProceed = true;
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

    private bool CheckConditionForStep(int step)
    {
        return step switch
        {
            4 => Input.GetKeyDown(KeyCode.A),
            5 => Input.GetKeyDown(KeyCode.D),
            6 => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space),
            7 => Input.GetMouseButtonDown(0),
            _ => false,
        };
    }

    private void PlayTutorialStep(int step)
    {
        switch (step)
        {
            case 4:
                PlayerController.Instance.PlayerAnim.PlayAnimRun(-1f);
                PlayerController.Instance.PlayerMove.Move(-1f, 3f);
                break;
            case 5:
                PlayerController.Instance.PlayerAnim.PlayAnimRun(1f);
                PlayerController.Instance.PlayerMove.Move(1f, 3f);
                break;
            case 6:
                PlayerController.Instance.PlayerAnim.PlayAnimJump(PlayerController.Instance.PlayerMove.Rb.velocity.y);
                PlayerController.Instance.PlayerMove.Jump(7f);
                break;
        }
    }

    private void ActionDoneTutorial()
    {
        GameManager.Instance.IsFirstPlay = false;
        canShow = false;
        GameManager.Instance.State = StateGame.Playing;
    }
}
