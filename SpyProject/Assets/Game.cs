using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {

    [SerializeField]
    private GameObject selectRegionMode;
    [SerializeField]
    private GameObject selectActionMode;
    [SerializeField]
    private GameObject resultMode;
    [SerializeField]
    private Text ActionDescription;
    [SerializeField]
    private Text[] ButtonActionDescription = new Text[2];
    [SerializeField]
    private Text ResultDescription;
    [SerializeField]
    private Text[] resources = new Text[4]; // people, money, ammunition, winpoints

    private Mode mode = Mode.Region;
    private State state;
    private Choise currentChoise;
    private Choise[] warBaseChoise;
    private Choise[] portChoise;
    private Choise[] govChoise;
    private Choise[] cityChoise;

    public enum Mode {
        Region,
        Action,
        Result
    }

    public void OnSelectRegionClick(int id) {
        SetMode(Mode.Action);
        switch (id) {
            default:
            case 0:
                currentChoise = GetRandom(warBaseChoise);
                break;
            case 1:
                currentChoise = GetRandom(portChoise);
                break;
            case 2:
                currentChoise = GetRandom(govChoise);
                break;
            case 3:
                currentChoise = GetRandom(cityChoise);
                break;
        }
        ActionDescription.text = currentChoise.Description;
        ButtonActionDescription[0].text = currentChoise.Сareful;
        ButtonActionDescription[1].text = currentChoise.Risky;
    }

    private Choise GetRandom(Choise[] choises) {
        return choises[Random.Range(0, warBaseChoise.Length)];
    }

    public void OnSelectActionClick(bool isRisky) {
        if (isRisky) {
            var isSuccess = Random.Range(0, 100) < 50;
            if (isSuccess) {
                ApplyChanges(currentChoise.RiskyWin, currentChoise.RiskyWinChange);
            } else {
                ApplyChanges(currentChoise.RiskyFail, currentChoise.RiskyFailChange);
            }
        } else {
            ApplyChanges(currentChoise.Сareful, currentChoise.СarefulChange);
        }
        SetMode(Mode.Result);
    }

    private void ApplyChanges(string message, State change) {
        ResultDescription.text = message + " " + ChangeResources(change);
        state.Peoples += change.Peoples;
        state.Ammunition += change.Ammunition;
        state.Money += change.Money;
        state.WinPoints += change.WinPoints;
    }

    private string ChangeResources(State change) {
        return (
            (change.Peoples != 0 ? "Люди: " + change.Peoples.ToString() + "\n" : "") +
            (change.Ammunition != 0 ? "Снаряжение: " + change.Ammunition.ToString() + "\n" : "") +
            (change.Money != 0 ? "Деньги: " + change.Money.ToString() + "\n" : "") +
            (change.WinPoints != 0 ? "\n*: " + change.WinPoints.ToString() + "\n" : "")
            );
    }

    public void OnResultClick(bool isRisky) {
        SetMode(Mode.Region);
    }

    private void Start() {
        warBaseChoise = Choise.GetWarBase();
        portChoise = Choise.GetPort();
        govChoise = Choise.GetGov();
        cityChoise = Choise.GetCity();

        state = new State() {
            Peoples = 10,
            Ammunition = 10,
            Money = 10,
            WinPoints = 0
        };
        SetMode(mode);
    }

    private void SetMode(Mode newMode) {
        mode = newMode;
        selectRegionMode.SetActive(mode == Mode.Region);
        selectActionMode.SetActive(mode == Mode.Action);
        resultMode.SetActive(mode == Mode.Result);
        resources[0].text = state.Peoples.ToString();
        resources[1].text = state.Money.ToString();
        resources[2].text = state.Ammunition.ToString();
        resources[3].text = state.WinPoints.ToString();
    }
}
