using System;

enum State
{
    Winner,
    Looser,
    Playing,
    NotInGame
}

class Player
{
    public string name;
    public int location;
    public State state = State.NotInGame;
    public int distanceTraveled = 0;

    public Player(string name)
    {
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps, int boardSize)
    {
        if (state == State.Playing)
        {
            location = (location + steps) % boardSize;
            if (location < 0)
                location += boardSize;

            distanceTraveled += Math.Abs(steps);
        }
    }

    public bool IsInGame()
    {
        return state == State.Playing || state == State.Winner || state == State.Looser;
    }

    public int GetLocation()
    {
        return location;
    }
}
