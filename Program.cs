using System;

class Game
{
    private void DoMoveCommand(char command, int steps)
    {
        switch (command)
        {
            case 'M': mouse.Move(steps); break;
            case 'C': cat.Move(steps); break;
        }
        CheckGameStatus();
    }

    private void CheckGameStatus()
    {

        if (cat.IsInGame() && (cat.location < 0 || cat.location >= size))
        {
            cat.state = State.Looser;
            if (mouse.IsInGame())
                mouse.state = State.Winner;
            state = GameState.End;
        }

        if (mouse.IsInGame() && (mouse.location < 0 || mouse.location >= size))
        {
            mouse.state = State.Looser;
            if (cat.IsInGame())
                cat.state = State.Winner;
            state = GameState.End;
        }

        // Проверяем, поймал ли кот мышь
        if (cat.IsInGame() && mouse.IsInGame() && cat.location == mouse.location)
        {
            cat.state = State.Winner;
            mouse.state = State.Looser;
            state = GameState.End;
        }
    }
    private void DoPrintCommand()
    {
        //вывод позиций игроков и расстояния между ними (если это возможно)
        string cat_location, mouse_location, distance;

        if (cat.IsInGame())
        {
            cat_location = cat.GetLocation().ToString();
        }
        else
        {
            cat_location = "??";
        }

        if (mouse.IsInGame())
        {
            mouse_location = mouse.GetLocation().ToString();
        }
        else
        {
            mouse_location = "??";
        }

        if (cat.IsInGame() && mouse.IsInGame())
        {
            distance = GetDistance().ToString();
        }
        else
        {
            distance = "??";
        }

        Console.WriteLine($"{cat_location,-7}{mouse_location,-9}{distance}");
    }

    private int GetDistance()
    {
        //найти расстояние между игроками cat.location - mouse.location
        return Math.Abs(cat.GetLocation() - mouse.GetLocation());
    }

    private void DoStartCommand(char player, int position)
    {
        if (player == 'C')
        {
            cat.location = position;
            cat.state = State.Playing;
        }
        else if (player == 'M')
        {
            mouse.location = position;
            mouse.state = State.Playing;
        }
    }
}