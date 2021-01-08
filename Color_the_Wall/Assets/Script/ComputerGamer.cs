using System.Collections.Generic;

public class ComputerGamer
{
    public GameProvider.PlayColors MyTurn(GameGrid gameField, (GameProvider.PlayColors FirstPlayerColor, GameProvider.PlayColors SecondPlayerColor) currentChosenColors)
    {
        HashSet<int> resultsSet = new HashSet<int>();
    	GameProvider.PlayColors chosenColor = (GameProvider.PlayColors) 0;
        var maxResult = 0;

    	foreach (GameProvider.PlayColors currentColor in GameProvider.PlayColors.GetValues(typeof(GameProvider.PlayColors)))
    	{
            if (currentColor == currentChosenColors.FirstPlayerColor || currentColor == currentChosenColors.SecondPlayerColor)
            {
                continue;
            }
            
            int result = gameField.BFSByColor(currentColor, false, true);
            resultsSet.Add(result);

            if (result > maxResult)
            {
                maxResult = result;
                chosenColor = currentColor;
            }
    	}

        if (resultsSet.Count <= 1)
        {
            do
            {
                chosenColor = gameField.GeneratePlayColor();
            } while (chosenColor == currentChosenColors.FirstPlayerColor || chosenColor == currentChosenColors.SecondPlayerColor);
        }

    	return chosenColor;
    }
}
