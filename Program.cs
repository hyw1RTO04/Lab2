using System;

class Game
{
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
}