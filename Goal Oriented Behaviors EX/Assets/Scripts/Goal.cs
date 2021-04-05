using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string name;
    public int value;

    public Goal(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}

public class Action
{
    public string name;
    public List<Goal> affected;

    public Action(string name, List<Goal> stuff)
    {
        this.name = name;
        affected = stuff;
    }

    public int GetGoalChange(Goal g)
    {
        foreach (Goal h in affected)
            if (g.name == h.name)
                return h.value;
        return 0;
    }
}
