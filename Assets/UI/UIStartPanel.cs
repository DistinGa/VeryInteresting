using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartPanel : MonoBehaviour
{
    [SerializeField]
    Transform PlayersParent;
    [SerializeField]
    List<GameObject> ChipPrefabs;

    public void StartGame()
    {
        List<Player> playersList = new List<Player>();
        Player player;

        for (int i = 0; i < ChipPrefabs.Count; i++)
        {
            string playerName = PlayersParent.GetChild(i).GetComponent<InputField>().textComponent.text;
            if (playerName != string.Empty)
            {
                player = Instantiate(ChipPrefabs[i]).GetComponent<Player>();
                player.Name = playerName;
                playersList.Add(player);
            }
        }

        GameManager.GM.InitGame(playersList);
    }
}
