using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    private Game game;
    private Text intervalCounterText;
    private Text aliveCellCounterText;

    public UIManager(Game game, Text intervalCounterText, Text aliveCellCounterText)
    {
        this.game = game;
        this.intervalCounterText = intervalCounterText;
        this.aliveCellCounterText = aliveCellCounterText;
    }

    public void UpdateIntervalDisplay(int intervalCount)
    {
        if (intervalCounterText != null)
        {
            intervalCounterText.text = "Intervals: " + intervalCount.ToString();
        }
    }

    public void UpdateAliveCellsDisplay(int aliveCellCount)
    {
        if (aliveCellCounterText != null)
        {
            aliveCellCounterText.text = "Alive Cells: " + aliveCellCount.ToString();
        }
    }
}