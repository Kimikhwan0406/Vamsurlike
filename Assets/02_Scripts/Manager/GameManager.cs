using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }

    protected override void Init()
    {
        base.Init();

        ui = new();
        ui.Init(Instantiate(Resources.Load<GameObject>("UI/UIRoot")).transform);
    }
     

    UIManager ui;
}
