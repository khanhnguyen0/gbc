//@McGameStudios 2014-2015 
//mcgamestudios.za.pl / quest-log.net

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

function OnMouseDown () {

	Application.LoadLevel('Options');

}