using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : LevelBase
{
    public static Action OnLevelComplete;
    public static Action OnGoToNextSubLevel;

    [SerializeField] private List<SubLevel> m_subLevelList = null;

    [SerializeField] private Transform m_subLevelParent = null;

    [SerializeField] private float m_delayBeforeNextSubSet = 1f;
    
    private Coroutine m_instantiateSubLevelCoroutine;
    private SubLevel m_currentSubLevel;
    private int m_currentSubLevelIndex;
    
    
    private void OnEnable()
    {
        SubLevel.OnSubLevelComplete += OnSubLevelComplete;
    }

    private void OnDisable()
    {
        SubLevel.OnSubLevelComplete -= OnSubLevelComplete;
    }


    public override void Initialize()
    {
        base.Initialize();
        m_currentSubLevelIndex = 0;

        if(m_instantiateSubLevelCoroutine != null)
            StopCoroutine(m_instantiateSubLevelCoroutine);
        
        m_instantiateSubLevelCoroutine = StartCoroutine(InstantiateSubLevel(0f));
    }

    private void OnSubLevelComplete()
    {
        m_currentSubLevelIndex++;

        if (m_currentSubLevelIndex >= m_subLevelList.Count)
        {
            OnLevelComplete?.Invoke();
            return;
        }

        OnGoToNextSubLevel?.Invoke();
        
        if(m_instantiateSubLevelCoroutine != null)
            StopCoroutine(m_instantiateSubLevelCoroutine);
        
        m_instantiateSubLevelCoroutine = StartCoroutine(InstantiateSubLevel(m_delayBeforeNextSubSet));
    }

    private IEnumerator InstantiateSubLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (Transform child in m_subLevelParent)
        {
            Destroy(child.gameObject);
        }

        m_currentSubLevel = Instantiate(m_subLevelList[m_currentSubLevelIndex], m_subLevelParent);

        //Waiting a few frames to make sure all the level is instantiated
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

    }
    
}
