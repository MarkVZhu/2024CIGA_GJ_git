using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : SingletonMono<Game>
{
    private Dictionary<Type, ViewModel> m_modelDict;
    private GameModel m_gameModel;
    private void Start()
    {
        m_gameModel = GetComponent<GameModel>();

    }
    public ViewModel TryGetModel<T>(Type type) where T:ViewModel
    {
        if(!m_modelDict.TryGetValue(type, out ViewModel model))
        {
            model = GetComponent<T>();
            m_modelDict.Add(type, model);
        }
        if(model == null)
        {
            Debug.LogError($"Game doesn't has this model:{type}");
        }

        return (T)model;
    }
}
