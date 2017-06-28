//@McGameStudios 2014-2015 
//mcgamestudios.za.pl /

#pragma strict
var playNormal : Texture2D;
var playHover : Texture2D;



function Start () {

}

function OnMouseEnter () {

	GetComponent.<GUITexture>().texture = playHover;

}

function OnMouseExit () {

	GetComponent.<GUITexture>().texture = playNormal;

}

function OnMouseDown ()
 {
 	//Nilupul
 	//SaveGame.Instance.IsSaveGame = false;
	Application.LoadLevel('World');

}