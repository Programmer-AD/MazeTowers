using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MoneyAssistant moneyAssistant;
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private TowerSlot towerSlotPrefab;

    public List<TowerCharacteristics> characteristics = new();

    public void InitTowerSlots(MazeTilemapAdapter mazeTilemap)
    {
        for (int y = 2; y < mazeTilemap.Height - 2; y += 2)
        {
            for (int x = 2; x < mazeTilemap.Width - 2; x += 2)
            {
                if (x == 2 && y > mazeTilemap.Height - 4) continue;
                var position = mazeTilemap.GetTilePosition(x, y) + new Vector3(0.5f, 0.5f, -3);
                var towerSlot = Instantiate(towerSlotPrefab, transform);
                towerSlot.towerManager = this;
                towerSlot.transform.position = position;
            }
        }
    }

    public void UpdateSlot(TowerSlot towerSlot)
    {
        if (towerSlot.tower == null)
        {
            var characteristic = characteristics[0];
            if (moneyAssistant.Withdraw(characteristic.Price))
            {
                towerSlot.Level = 0;

                var towerPosition = towerSlot.transform.position - Vector3.forward;
                towerSlot.tower = Instantiate(towerPrefab, towerPosition, Quaternion.identity, towerSlot.transform);
                towerSlot.tower.gameManager = gameManager;
                towerSlot.tower.SetCharacteristics(characteristic);
            }
        }
        else if (towerSlot.Level + 1 < characteristics.Count)
        {
            var characteristic = characteristics[towerSlot.Level + 1];
            if (moneyAssistant.Withdraw(characteristic.Price))
            {
                towerSlot.tower.SetCharacteristics(characteristic);
                towerSlot.Level++;
            }
        }
    }
}
