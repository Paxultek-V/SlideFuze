using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_SubLevelClearedMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text = null;

    private Sequence m_sequence;

    private void Awake()
    {
        m_text.DOFade(0f, 0f);
    }

    private void OnEnable()
    {
        Level.OnGoToNextSubLevel += OnGoToNextSubLevel;
    }

    private void OnDisable()
    {
        Level.OnGoToNextSubLevel -= OnGoToNextSubLevel;
    }


    private void OnGoToNextSubLevel()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (m_sequence != null && m_sequence.IsPlaying())
            m_sequence.Kill();

        m_text.fontSize = 200f;

        DOTween.To(() => m_text.fontSize, x => m_text.fontSize = x, 100f, 0.15f).SetEase(Ease.Linear)
            .OnComplete(() => DOTween.To(() => m_text.fontSize, x => m_text.fontSize = x, 0f, 0.5f).SetEase(Ease.Linear).SetDelay(0.5f));

        m_text.DOFade(1f, 0.15f)
            .OnComplete(() => m_text.DOFade(0f, 0.5f).SetDelay(0.5f));
        
        // m_sequence = DOTween.Sequence();
        // m_sequence.Append(DOTween.To(() => m_text.fontSize, x => m_text.fontSize = x, 100f, 0.5f).SetEase(Ease.Linear));
        // m_sequence.Append(m_text.DOFade(1f, 0.15f));
        // m_sequence.AppendInterval(0.5f);
        // m_sequence.Append(m_text.DOFade(0f, 0.35f));
        // m_sequence.Append(DOTween.To(() => m_text.fontSize, x => m_text.fontSize = x, 0f, 0.5f).SetEase(Ease.Linear));

        //m_sequence.Play();
    }
}