using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalOrientedThing : MonoBehaviour
{
    public float tickDelay;
    public Text goalsText;
    public Text actionText;

    Goal[] goals;
    int[] values;
    Action[] actions;
    float currentDiscontent = 0;

    // Start is called before the first frame update
    void Start()
    {
        goals = new Goal[5];
        values = new int[5];
        actions = new Action[5];

        values[0] = 2;
        values[1] = 2;
        values[2] = 2;
        values[3] = 2;
        values[4] = 50;

        goals[0] = new Goal("Hunger", 2);
        goals[1] = new Goal("Fatigue", 2);
        goals[2] = new Goal("Work", 3);
        goals[3] = new Goal("Boredom", 3);
        goals[4] = new Goal("Life", 1);

        List<Goal> stuff = new List<Goal> { new Goal("Work", -3)};
        actions[0] = new Action("procrastinate", stuff);
        stuff = new List<Goal> { new Goal("Hunger", 3), new Goal("Boredom", -5), new Goal("Life", -4)};
        actions[1] = new Action("kill some zombies", stuff);
        stuff = new List<Goal> { new Goal("Work", -5), new Goal("Fatigue", 3) };
        actions[2] = new Action("do some actual work", stuff);
        stuff = new List<Goal> { new Goal("Hunger", -2), new Goal("Life", 2)};
        actions[3] = new Action("eat", stuff);
        stuff = new List<Goal> { new Goal("Fatigue", -3), new Goal("Life", 3)};
        actions[4] = new Action("sleep", stuff);

        writeText("");
        InvokeRepeating("Tick", tickDelay, tickDelay);
    }

    void writeText(string action)
    {
        string newText = "";
        for (int i = 0; i < goals.Length; ++i)
        {
            newText += goals[i].name + ": " + values[i] + '\n';
        }
        goalsText.text = newText;

        if (action != "")
        {
            actionText.text = action;
        }
    }

    void Tick()
    {
        Debug.Log("I decided not to be lazy, so I'm going to do something");
        Debug.Log("    Right now my discontent is: " + currentDiscontent);
        string output = "    My needs right now are: " + goals[0].name + ": " + values[0];
        for (int i = 1; i < goals.Length; ++i)
        {
            output += ", " + goals[i].name + ": " + values[i];
        }
        Debug.Log(output);

        float lowest = Mathf.Infinity;
        int index = -1;

        for (int i = 0; i < actions.Length; ++i)
        {
            float test = discontent(actions[i]);
            if (test < lowest)
            {
                lowest = test;
                index = i;
            }
        }

        for (int i = 0; i < goals.Length; ++i)
        {
            int number = actions[index].GetGoalChange(goals[i]);
            values[i] = Mathf.Max(values[i] + (number != 0 ? number : goals[i].value), 0);
        }
        currentDiscontent = lowest;

        Debug.Log("I decided to " + actions[index].name);
        Debug.Log("    Now my discontent is: " + currentDiscontent);
        if (values[4] > 0)
            writeText("I decided to " + actions[index].name);
        else
        {
            writeText("I decided to " + actions[index].name + ", and now I'm dead...");
            CancelInvoke();
        }
    }

    float discontent(Action a)
    {
        int discontent = 0;

        for (int i = 0; i < goals.Length; ++i)
        {
            if (goals[i].name == "Life")
                continue;
            int goalChange = a.GetGoalChange(goals[i]);
            int value = Mathf.Max(values[i] + (goalChange != 0 ? goalChange : values[i]), 0);
            discontent +=  value * value;
        }

        return discontent;
    }
}
