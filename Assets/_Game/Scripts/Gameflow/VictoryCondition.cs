using System;
using UnityEngine;

public class VictoryCondition : GameflowBehavior
{
    public static Action OnVictoryConditionMet;

    [SerializeField] private float m_delayBeforeTriggerVictory = 1f;

    private bool m_canTrackVictory;

    protected override void OnEnable()
    {
        base.OnEnable();
        Level.OnLevelComplete += OnLevelComplete;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Level.OnLevelComplete -= OnLevelComplete;
    }


    protected override void OnGameplay()
    {
        base.OnGameplay();
        m_canTrackVictory = true;
    }

    protected override void OnVictory()
    {
        base.OnVictory();
        m_canTrackVictory = false;
    }

    protected override void OnGameover()
    {
        base.OnGameover();
        m_canTrackVictory = false;
    }

    private void OnLevelComplete()
    {
        if (m_canTrackVictory == false)
            return;

        m_canTrackVictory = false;
        Invoke(nameof(TriggetVictory), m_delayBeforeTriggerVictory);
    }

    private void TriggetVictory()
    {
        OnVictoryConditionMet?.Invoke();
    }
}