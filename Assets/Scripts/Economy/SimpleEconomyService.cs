using Managers;
using UnityEngine;

namespace Economy
{
    public class SimpleEconomyService : MonoBehaviour
    {
        private static bool debug = false;
        public static SimpleEconomyService Instance { get; private set; }

        public int Credits
        {
            get
            {
                return TryGetGameModel(out GameModel gameModel) ? gameModel.Coins : 0;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (TryGetGameModel(out GameModel gameModel))
            {
                if(debug) Debug.Log($"[Economy] Podpieto pod GameModel.Coins. Aktualne saldo: {gameModel.Coins}.");
            }
            else
            {
                if(debug) Debug.LogWarning("[Economy] GameModel jeszcze niedostepny. Serwis poczeka na inicjalizacje GameManager.");
            }
        }

        public void AddCredits(int amount, string reason)
        {
            if (amount <= 0)
            {
                return;
            }

            if (!TryGetGameModel(out GameModel gameModel))
            {
                if(debug) Debug.LogWarning($"[Economy] Nie mozna dodac kredytow ({reason}) - GameModel niedostepny.");
                return;
            }

            gameModel.SetCoins(gameModel.Coins + amount);
            if(debug) Debug.Log($"[Economy] +{amount} kredyt za: {reason}. Aktualne saldo: {gameModel.Coins}.");
        }

        public bool TrySpendCredits(int amount, string reason)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (!TryGetGameModel(out GameModel gameModel))
            {
                if(debug) Debug.LogWarning($"[Economy] Nie mozna wydac kredytow ({reason}) - GameModel niedostepny.");
                return false;
            }

            if (!gameModel.TrySpendCoins(amount))
            {
                if(debug) Debug.Log($"[Economy] Brak srodkow na: {reason}. Wymagane: {amount}, posiadane: {gameModel.Coins}.");
                return false;
            }

            if(debug) Debug.Log($"[Economy] Wydano {amount} kredytow na: {reason}. Aktualne saldo: {gameModel.Coins}.");
            return true;
        }

        private bool TryGetGameModel(out GameModel gameModel)
        {
            gameModel = null;

            if (GameManager.Instance == null)
            {
                return false;
            }

            gameModel = GameManager.Instance.GameModel;
            return gameModel != null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureExists()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject economyRoot = new GameObject("SimpleEconomyService");
            economyRoot.AddComponent<SimpleEconomyService>();
        }
    }
}
