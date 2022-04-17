using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private TowerSlot towerSlotPrefab;

    public List<TowerCharacteristics> characteristics = new();

    public void InitTowerSlots(MazeTilemapAdapter mazeTilemap)
    {
        for (int y = 2; y < mazeTilemap.Height - 2; y += 2)
        {
            for (int x = 2; x < mazeTilemap.Width - 2; x += 2)
            {
                var towerSlot = Instantiate(towerSlotPrefab);
                towerSlot.towerManager = this;
                var position = mazeTilemap.GetTilePosition(x, y) + new Vector3(0.5f, 1.5f, -3);
                towerSlot.transform.position = position;
            }
        }
    }

    public void UpdateSlot(TowerSlot towerSlot)
    {
        if (towerSlot.tower == null)
        {
            towerSlot.Level = 0;

            var towerPosition = towerSlot.transform.position - Vector3.forward;
            towerSlot.tower = Instantiate(towerPrefab, towerPosition, Quaternion.identity);
            towerSlot.tower.gameManager = gameManager;
            towerSlot.tower.SetCharacteristics(characteristics[0]);
        }else if (towerSlot.Level + 1 < characteristics.Count)
        {
            towerSlot.tower.SetCharacteristics(characteristics[++towerSlot.Level]);
        }
    }
}
