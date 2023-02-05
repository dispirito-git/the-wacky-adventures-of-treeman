using Godot;
using System;

public class GameState : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

private static GameState instance;

public int CurrentLevel {get; set;}

public static GameState Instance{
	get
	{
		if(instance == null){
			instance = new GameState();
		}
		return instance;
	}
}

private GameState(){
	CurrentLevel = 0;
}

}
