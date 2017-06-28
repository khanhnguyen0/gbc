using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    public GameObject[] players;
    public Image player;
    public Button prev, next;
    public InputField playerName;
    public Button create;

    int currentPlayer;

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateButton()
    {
        SaveGame.Instance.IsSaveGame = false;
        PlayerPrefs.SetString("PlayerName", playerName.text);
        PlayerPrefs.SetString("PlayerAsset", players[currentPlayer].name);
        SceneManager.LoadScene("World");
    }

    public void PrevButton()
    {
        currentPlayer--;
        RefreshPlayer();
    }

    public void NextButton()
    {
        currentPlayer++;
        RefreshPlayer();
    }

    void Start()
    {
        RefreshPlayer();
    }

    void RefreshPlayer()
    {
        SpriteRenderer playerSpriteRenderer = players[currentPlayer].GetComponent<SpriteRenderer>();
        player.sprite = playerSpriteRenderer.sprite;
        player.color = playerSpriteRenderer.color;
        prev.interactable = currentPlayer > 0;
        next.interactable = currentPlayer < players.Length - 1;
    }

    public void NameChanged(string name)
    {
        create.interactable = name.Length > 0;
    }
}
