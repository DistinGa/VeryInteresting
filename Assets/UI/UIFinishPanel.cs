using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFinishPanel : MonoBehaviour
{
    [SerializeField]
    GameObject LinePrefab;
    [SerializeField]
    Transform StatsParent;

    public void Fill(List<Player> players)
    {
        Transform newLine;
        for (int i = 0; i < players.Count; i++)
        {
            newLine = Instantiate(LinePrefab, StatsParent).transform;
            newLine.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            newLine.GetChild(1).GetComponent<Text>().text = players[i].Name;
            newLine.GetChild(2).GetComponent<Text>().text = players[i].Turns.ToString();
            newLine.GetChild(3).GetComponent<Text>().text = players[i].Bonuses.ToString();
            newLine.GetChild(4).GetComponent<Text>().text = players[i].Penalties.ToString();
        }
    }

    public void StartNewGame()
    {
        GameManager.GM.SetState(GameStates.Start);
    }
}
