using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    [SerializeField]
    GameObject goStartPanel, goFinishPanel, goTurnPanel;
    [SerializeField]
    Button btnDice;
    [SerializeField]
    Text txtPlayerName, txtDiceResult;
    [SerializeField]
    Dice Dice;
    [SerializeField]
    Transform trfmZones;

    List<Zone> Zones;

    //Из очереди берётся игрок, делающий ход, после хода возвращается в конец очереди. 
    //Если игрок дошёл до конца, он не возвращается в очередь. 
    //Если очередь опустела - игра закончилась.
    Queue<Player> Players;  
    List<Player> FinishedPlayers;
    GameStates State;

    private void Awake()
    {
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        Zones = new List<Zone>();
        FinishedPlayers = new List<Player>();

        foreach (Transform item in trfmZones)
        {
            Zones.Add(item.GetComponent<Zone>());
        }
        State = GameStates.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameStates.Hold:
                break;
            case GameStates.Turn:
                PrepareTurn();
                break;
            case GameStates.Start:
                StartGame();
                break;
            case GameStates.Finish:
                FinishGame();
                break;
            default:
                break;
        }
    }

    void PrepareTurn()
    {
        if (Players.Count == 0)
        {
            State = GameStates.Finish;
            return;
        }

        State = GameStates.Hold;
        Dice.DiceResult = 0;
        btnDice.interactable = true;

        Player player = Players.Peek();

        if (player.PassTurn)
        {
            player.PassTurn = false;
            Players.Enqueue(Players.Dequeue());
            player = Players.Peek();
        }

        txtPlayerName.text = player.Name;
        StartCoroutine(MakeTurn(player));
    }

    IEnumerator MakeTurn(Player player)
    {
        while (!Dice.isDone)
            yield return new WaitForSeconds(1f);

        btnDice.interactable = false;
        txtDiceResult.text = Dice.DiceResult.ToString();
        int steps = Dice.DiceResult;
        yield return new WaitForSeconds(1f);

        //Перемещение фишки игрока.
        for (int i = player.CurrentGamePos; i < player.CurrentGamePos + steps; i++)
        {
            if (i+1 < Zones.Count)
            {
                Zones[i].FreePosition();
                player.ChangePosition(Zones[i+1].GetPosition());
                while (!player.isDone)
                    yield return new WaitForSeconds(1f);
            }
        }

        player.CurrentGamePos += steps;
        if (player.CurrentGamePos >= Zones.Count)
            player.CurrentGamePos = Zones.Count - 1; //При "перелёте" ставим фишку на последнюю клетку.

        switch (Zones[player.CurrentGamePos].ZoneType)
        {
            case ZoneTypes.Regular:
                player.Turns++;

                if (player.CurrentGamePos >= Zones.Count - 1)
                    FinishedPlayers.Add(Players.Dequeue()); //Удаляем из очереди.
                else
                    Players.Enqueue(Players.Dequeue());
                break;
            case ZoneTypes.Bonus:
                //Если попали на клетку с допходом, игрока в очереди не переставляем. При следующем ходе будет ходить он же.
                player.Bonuses++;
                break;
            case ZoneTypes.Penalty:
                player.Penalties++;
                player.PassTurn = true;
                Players.Enqueue(Players.Dequeue());
                break;
            default:
                break;
        }

        State = GameStates.Turn;
    }

    public void RollDice()
    {
        Dice.RollDice();
    }

    public void SetState(GameStates NewState)
    {
        State = NewState;
    }

    void StartGame()
    {
        State = GameStates.Hold;
        goStartPanel.SetActive(true);
        goFinishPanel.SetActive(false);
        goTurnPanel.SetActive(false);
    }

    void FinishGame()
    {
        State = GameStates.Hold;
        goFinishPanel.SetActive(true);
        goStartPanel.SetActive(false);
        goTurnPanel.SetActive(false);

        goFinishPanel.GetComponent<UIFinishPanel>().Fill(FinishedPlayers);
    }

    public void InitGame(List<Player> playersList)
    {
        Players = new Queue<Player>();

        foreach (var item in playersList)
        {
            Players.Enqueue(item);
            item.transform.position = Zones[item.CurrentGamePos].GetPosition();
        }

        goStartPanel.SetActive(false);
        goTurnPanel.SetActive(true);
        State = GameStates.Turn;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public enum GameStates
{
    Hold,
    Turn,
    Start,
    Finish
}
