using UnityEngine.SceneManagement;

public class SceneController {

    public static void Menu() {
        SceneManager.LoadScene("Menu");
    }

    public static void Play() {
        SceneManager.LoadScene("Game");
    }

}