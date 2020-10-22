using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfoOnDeath : MonoBehaviour
{
    [SerializeField]
    private bool _isPlayerOneDeadInfo = false;

    [SerializeField]
    private bool _isPlayerTwoDeadInfo = false;

    public bool IsPlayerOneDeadInfo
    {
        get => _isPlayerOneDeadInfo;
        set => _isPlayerOneDeadInfo = value;
    }

    public bool IsPlayerTwoDeadInfo
    {
        get => _isPlayerTwoDeadInfo;
        set => _isPlayerTwoDeadInfo = value;
    }
}
