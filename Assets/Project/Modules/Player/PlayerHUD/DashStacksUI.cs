using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashStacksUI : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject DashStacksPrefab;
    private List<Image> DashStackImages = new List<Image>(10);
    private Color BLACK = new Color(0f, 0f, 0f);
    private Color WHITE = new Color(255f, 255f, 255f);

    void OnEnable()
    {
        _playerData.player.movement.OnDashStacksMaxUpdate += GenerateDashStacksUI;
        _playerData.player.movement.OnDashStacksCurrentUpdate += UpdateDashStacksUI;
        GenerateDashStacksUI();
    }

    void OnDisable()
    {
        _playerData.player.movement.OnDashStacksMaxUpdate -= GenerateDashStacksUI;
        _playerData.player.movement.OnDashStacksCurrentUpdate -= UpdateDashStacksUI;
    }

    private void GenerateDashStacksUI()
    {
        DestroyDashStacksUI();
        for(int i=0; i < _playerData.player.movement.dashStacksMax; i++)
        {
            GameObject dashStack = Instantiate(DashStacksPrefab, transform);
            DashStackImages.Add(dashStack.GetComponentInChildren<Image>());
        }
        UpdateDashStacksUI();
    }
    
    private void DestroyDashStacksUI()
    {
        gameObject.DeleteAllChildren();
        DashStackImages = new List<Image>(10);
    }

    private void UpdateDashStacksUI()
    {
        int activeDashCount = _playerData.player.movement.dashStacksCurrent;
        
        SetImageColor(DashStackImages, BLACK);

        for(int i=0; i < _playerData.player.movement.dashStacksMax; i++)
        {
            if (activeDashCount == 0)
            {
                SetImageColor(DashStackImages[i], BLACK);
                return;
            }
            
            SetImageColor(DashStackImages[i], WHITE);
            activeDashCount--;
        }
    }

    private void SetImageColor(List<Image> images, Color color) => images.ForEach(image => SetImageColor(image, color));
    private void SetImageColor(Image image, Color color) => image.color = color;
}