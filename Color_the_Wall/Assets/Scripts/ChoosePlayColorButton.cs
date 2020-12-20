using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayColorButton : MonoBehaviour
{
    public ChoosePlayColorButton(GameProvider.PlayColors playColor)
    {
    	GameObject newButton = new GameObject($"{playColor.ToString()}Button", typeof(Image), typeof(Button));
    }

    public GameProvider.PlayColors PressButton()
    {
    	return GameProvider.PlayColors.GRAY;
    }
}
