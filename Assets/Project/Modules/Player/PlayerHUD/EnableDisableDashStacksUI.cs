using UnityEngine;

public class EnableDisableDashStacksUI : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject dashStacksUI;

    void OnEnable()
    {
        TryEnableDashStacksUI();
        _playerData.player.movement.OnDashStacksMaxUpdate += TryEnableDashStacksUI;
    }

    void OnDisable()
    {
        _playerData.player.movement.OnDashStacksMaxUpdate -= TryEnableDashStacksUI;
    }

    public void TryEnableDashStacksUI()
    {
        dashStacksUI.SetActive(false);

        if (_playerData.player.movement.dashStacksMax > 0)
            dashStacksUI.SetActive(true);
    }
}
