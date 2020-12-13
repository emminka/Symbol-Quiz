using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skript : MonoBehaviour
{
    public Leaderboard Leaderboard;
    public ScoreRegistry ScoreRegistry;

    public GameObject[] levels;

    public GameObject button_zmena_light, button_zmena_dark,rebricek_back_light, rebricek_back_dark, zadanie_back_light, zadanie_back_dark, lost, win, dark_pozadie_vsade, light_pozadie_vsade, otazka, otazka_kladna_light, otazka_kladna_dark, otazka_zaporna_light, otazka_zaporna_dark, odpoved, menu_light, menu_dark, odpocitavanie, hra, hra_light_pozadie, hra_dark_pozadie;
    public Text odpocet_text;
    public GameObject restart_light, restart_dark, back_to_light, back_to_dark, wow, wow_restart_light, wow_restart_dark, wow_back_to_light, wow_back_to_dark;
    public GameObject[] layers;

    public GameObject rebricek, zadanie_mena;

    public Sprite[] obrazky;
    public Level[] policka;

    public Image obrazok_na_otazku;

    public Image TimeBar;

    public Text levelText;

    private List<int> indices;

    private bool odpovedJeAno;

    public InputField nameField;

    int cisloLevelu = 1;
    void Start()
    {
        DisableLayers();
        if (PlayerPrefs.GetInt("wasFirstLaunch") == 0)
        {
            PlayerPrefs.SetInt("wasFirstLaunch", 1);
            PlayerPrefs.SetString("username", "player" + Random.Range(1000, 9999).ToString());
            PlayerPrefs.SetString("localScore", "{ \"scores\" : [ ]}");
        }

        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            
            light_pozadie_vsade.SetActive(true);
            menu_light.SetActive(true);
        }
        else
        {
            
            dark_pozadie_vsade.SetActive(true);
            menu_dark.SetActive(true);
        }

        nameField.text = PlayerPrefs.GetString("username");
    }

    public void RunGame(int level) //sputenie hry podla levelu
    {
        DisableLayers();

        if (PlayerPrefs.GetInt("darkMode") == 0) //vyber dark alebo light modu
        {
            hra_light_pozadie.SetActive(true);
        }
        else
        {
            hra_dark_pozadie.SetActive(true);
        }

        levelText.text = "Level " + level; //zmena cisla v leveli

        hra.SetActive(true);

        foreach (GameObject levelObject in levels)
        {
            levelObject.SetActive(false);
        }

        levels[level - 1].SetActive(true);

        int pictureCount = obrazky.Length;
        indices = new List<int>();

        for (int i = 0; i < policka[level - 1].images.Length; i++) //vyberanie nahodnych obrazkov
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, pictureCount);
            } while (indices.Contains(randomIndex));

            indices.Add(randomIndex);

            policka[level - 1].images[i].sprite = obrazky[randomIndex]; 
        }


        if (cisloLevelu == 16) //nastavovanie roznych casov pre leveli
        {
            StartCoroutine(TimeBarAnimate(8));
        }
        else if (cisloLevelu == 4 || cisloLevelu == 7 || cisloLevelu == 11 || cisloLevelu == 17 || cisloLevelu == 21 || cisloLevelu == 26)
        {
            StartCoroutine(TimeBarAnimate(7));
        }
        else if (cisloLevelu == 8  || cisloLevelu == 5 || cisloLevelu == 12 || cisloLevelu == 18 || cisloLevelu == 22 || cisloLevelu == 27)
        {
            StartCoroutine(TimeBarAnimate(6));
        }
        else if (cisloLevelu == 1 || cisloLevelu == 6 || cisloLevelu == 9 || cisloLevelu == 13 || cisloLevelu == 19 || cisloLevelu == 23 || cisloLevelu == 28)
        {
            StartCoroutine(TimeBarAnimate(5));
        }
        else if (cisloLevelu == 2 || cisloLevelu == 10 || cisloLevelu == 14 || cisloLevelu == 20 || cisloLevelu == 24 || cisloLevelu == 29)
        {
            StartCoroutine(TimeBarAnimate(4));
        }
        else if (cisloLevelu == 3 || cisloLevelu == 15 || cisloLevelu == 25 || cisloLevelu == 30)
        {
            StartCoroutine(TimeBarAnimate(3));
        }

    }

    public IEnumerator TimeBarAnimate(int seconds) //odpocitanavie do noveho kola
    {
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            hra_light_pozadie.SetActive(true);

        }
        else
        {
            hra_dark_pozadie.SetActive(true);

        }
        for (float f = 1; f > 0; f -= 1f / seconds * Time.deltaTime)
        {
            TimeBar.fillAmount = f;
            yield return null; // wait for new frame
        }


        if (PlayerPrefs.GetInt("darkMode") == 0) //vybranie modu a otazky
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
            otazka.SetActive(true);
            otazka_kladna_dark.SetActive(false);
            otazka_kladna_light.SetActive(true);
            otazka_zaporna_dark.SetActive(false);
            otazka_zaporna_light.SetActive(true);

        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);
            otazka.SetActive(true);
            otazka_kladna_dark.SetActive(true);
            otazka_kladna_light.SetActive(false);
            otazka_zaporna_dark.SetActive(true);
            otazka_zaporna_light.SetActive(false);
        }

        // ukazanie otazky a nasledne nahodne priradenie obrazku do nej
        if (Random.value > 0.5)
        {
            print("mala by tam byt");
            // answer should be true
            int questionImageIndex = Random.Range(0, indices.Count);

            obrazok_na_otazku.sprite = obrazky[indices[questionImageIndex]];

            odpovedJeAno = true;
        }
        else
        {
            print("nemala by tam byt");

            int pictureCount = obrazky.Length;
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, pictureCount);
            } while (indices.Contains(randomIndex));

            obrazok_na_otazku.sprite = obrazky[randomIndex];

            odpovedJeAno = false;
        }
    }
    public void DisableLayers() //func na vypnutie vsetkych vrstiev
    {
        foreach (GameObject lay in layers)
        {
            lay.SetActive(false);
        }
    }

    public IEnumerator odpocet(int levelNumber) //odpocet do zacatia hry 3 2 1
    {

        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);

        }
        odpocitavanie.SetActive(true);
        if (cisloLevelu >= 2) //od prvej dobrej odpovede sa zobrazuje aj ''correct''
        {
            odpoved.SetActive(true);
            lost.SetActive(false);
            win.SetActive(true);
            wow.SetActive(false);
            odpocet_text.text = "";
            yield return new WaitForSeconds(1f);
        }
        if (cisloLevelu == 31) //pri spravnom odpovedani posledneho levelu (30) sa ukaze specialna vrstva o vyhre
        {

            if (PlayerPrefs.GetInt("darkMode") == 0)
            {
                DisableLayers();
                light_pozadie_vsade.SetActive(true);
                wow_restart_dark.SetActive(false);
                wow_restart_light.SetActive(true);
                wow_back_to_dark.SetActive(false);
                wow_back_to_light.SetActive(true);
            }
            else
            {
                DisableLayers();
                dark_pozadie_vsade.SetActive(true);
                wow_restart_dark.SetActive(true);
                wow_restart_light.SetActive(false);
                wow_back_to_dark.SetActive(true);
                wow_back_to_light.SetActive(false);

            }
            odpoved.SetActive(true);
            wow.SetActive(true);
            lost.SetActive(false);
            win.SetActive(false);
            yield break;
        }

        odpoved.SetActive(false);
        lost.SetActive(false);
        win.SetActive(false);
        wow.SetActive(false);
        odpocet_text.text = "3";
        yield return new WaitForSeconds(1f);

        odpocet_text.text = "2";
        yield return new WaitForSeconds(1f);

        odpocet_text.text = "1";
        yield return new WaitForSeconds(1f);

        DisableLayers();

        RunGame(levelNumber);
    }
    public void menu_na_odpocet_light()
    {
        menu_light.SetActive(false);
        StartCoroutine(odpocet(cisloLevelu));

    }

    public void menu_na_odpocet_dark()
    {
        menu_dark.SetActive(false);
        StartCoroutine(odpocet(cisloLevelu));
    }

    public void darkmode() //zapamatanie si dark modu a zapnutie
    {
        menu_light.SetActive(false);

        menu_dark.SetActive(true);

        PlayerPrefs.SetInt("darkMode", 1);
    }

    public void lightmode() //zapamatanie si light modu a zapnutie
    {
        menu_light.SetActive(true);

        menu_dark.SetActive(false);

        PlayerPrefs.SetInt("darkMode", 0);
    }

    public void answear_yes() //funkcia ktora spracovava odpoved ano v otazke
    {
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);

        }
        odpoved.SetActive(true);

        if (odpovedJeAno)
        {
            answeredCorrectly();  //ked ma byt odpoved ano a aj dali sme ano, je to spravne
        }
        else
        {
            answeredIncorrectly();  //ked ma byt odpoved nie ale dali sme ano, je to nespravne
        }
    }

    public void answear_no() //funkcia ktora spracovava odpoved nie v otazke
    {
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);

        }
        odpoved.SetActive(true);

        if (odpovedJeAno)
        {
            answeredIncorrectly(); //ked ma byt odpoved ano, ale dali sme nie, je to nepsravne
        }
        else
        {
            answeredCorrectly(); //ked ma byt odpoved nie a aj dali sme nie, je to spravne
        }
    }

    public void answeredCorrectly() //odpovedane spravne, cislo levelu sa navysi a zacne sa odpovet dalsieho
    {
        cisloLevelu++;
        StartCoroutine(odpocet(cisloLevelu));
    }

    public void answeredIncorrectly() //odpvedane nespravne
    {
        //zobrazi sa ci chces hrat znova alebo ci chces ist do menu
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
            odpoved.SetActive(true);
            lost.SetActive(true);
            win.SetActive(false);
            wow.SetActive(false);
            restart_dark.SetActive(false);
            restart_light.SetActive(true);
            back_to_dark.SetActive(false);
            back_to_light.SetActive(true);
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);
            lost.SetActive(true);
            win.SetActive(false);
            wow.SetActive(false);
            odpoved.SetActive(true);
            restart_dark.SetActive(true);
            restart_light.SetActive(false);
            back_to_dark.SetActive(true);
            back_to_light.SetActive(false);
        }
        //zapis do registra score
        ScoreRegistry.AddNewScore("symbol quiz", PlayerPrefs.GetString("username"), (cisloLevelu - 1) * 10);

    }

    public void restart() //hra sa resatrtuje po stlaceni tlacidla
    {
        cisloLevelu = 1;
        StartCoroutine(odpocet(cisloLevelu));
    }

    public void back_to_menu() //vrati sa do povodneho menu 
    {
        cisloLevelu = 1;
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
            menu_light.SetActive(true);
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);
            menu_dark.SetActive(true);
        }
    }

    public void ukaz_rebricek() //zobrazi sa leaderboard
    {
        DisableLayers();
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
            rebricek.SetActive(true);
            rebricek_back_dark.SetActive(false);
            rebricek_back_light.SetActive(true);
            button_zmena_light.SetActive(true);
            button_zmena_dark.SetActive(false);
            
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);
            rebricek.SetActive(true);
            rebricek_back_dark.SetActive(true);
            rebricek_back_light.SetActive(false);
            button_zmena_light.SetActive(false);
            button_zmena_dark.SetActive(true);
            
        }

       
        Leaderboard.RefreshScores();
    }

    public void Quit() //applikacia sa vypne
    {
        Application.Quit();
    }

    public void zmenit_meno(InputField self) //zmena mena na vlastne
    {
        PlayerPrefs.SetString("username", self.text);
    }

    public void nove_meno() //vrstva na vytvorebie noveho mena kvoli prehladnosti
    {
        DisableLayers();
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            DisableLayers();
            light_pozadie_vsade.SetActive(true);
            zadanie_back_dark.SetActive(false);
            zadanie_back_light.SetActive(true);
                
        }
        else
        {
            DisableLayers();
            dark_pozadie_vsade.SetActive(true);
            zadanie_back_dark.SetActive(true);
            zadanie_back_light.SetActive(false);
        }

        zadanie_mena.SetActive(true);
    }
}

[System.Serializable]
public class Level
{
    public Image[] images;
}