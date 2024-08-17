using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Freeroam, Dialog, Battle
}

public class GameController : MonoBehaviour
{
    [SerializeField]PlayerController playerController;

    GameState state;

    private void Start()
    {
        DialogManager.instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.instance.OnHideDialog += () =>
        {
            if(state == GameState.Dialog)
                state = GameState.Freeroam;
        };
        DialogManager.instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
    }


    private void Update()
    {
        if( state == GameState.Freeroam)
        {
            playerController.HandleUpdate();
        }
        else if( state == GameState.Dialog)
        {
            DialogManager.instance.HandleUpdate();
        }
        else if( state == GameState.Battle)
        {

        }
    }

}
