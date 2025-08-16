using _Utils;

public class DeckManager {
    private const int MinThreshold = 15; // when fewer cards left, reshuffle/reset
    private const int cardSizePerType = 14; // Ace (1) through King (13)
    public Stack<Card> Deck { get; private set; }

    public DeckManager() {
        InitDeck();
        ShuffleDeck();
    }

    private void InitDeck() {
        List<Card> list = new();
        int cardTypeSize = Enum.GetNames(typeof(CardType)).Length;

        for (int i = 0; i < cardTypeSize; i++) {
            for (int u = 1; u < cardSizePerType; u++) {
                list.Add(new Card(u, (CardType)i));
            }
        }

        Deck = new Stack<Card>(list);
    }

    public void ShuffleDeck() {
        if (Utils.Instance == null) return;

        var list = Deck.ToList();
        list = Utils.Instance.Shuffle(list);

        Deck = new Stack<Card>(list);
    }

    /// <summary>
    /// Checks if the deck is running low and resets it if needed.
    /// </summary>
    public void ResetIfLow() {
        if (Deck.Count <= MinThreshold) {
            InitDeck();
            ShuffleDeck();
        }
    }

    public Card Draw() {
        ResetIfLow(); // always check before drawing
        if (Deck.Count == 0) throw new InvalidOperationException("Deck is empty");
        return Deck.Pop();
    }
}

public class Card {
    public int Id { get; private set; }
    public CardType Type { get; private set; }
    public bool CardIsAce { get; private set; } = false;
    public int BaseValue { get; private set; }

    public Card(int id, CardType type) {
        Id = id;
        Type = type;
        DetermineValue();
    }

    private void DetermineValue() {
        switch (Id) {
            case 1:
                CardIsAce = true;
                BaseValue = 1; // Always store Ace as 1
                break;
            case 11: // Jack
            case 12: // Queen
            case 13: // King
                BaseValue = 10;
                break;
            default:
                BaseValue = Id;
                break;
        }
    }

    public override string ToString() {
        string name = Id switch {
            1 => "Ace",
            11 => "Jack",
            12 => "Queen",
            13 => "King",
            _ => Id.ToString()
        };
        return $"{name} of {Type}";
    }
}

public class Hand {
    private readonly List<Card> cards = new();
    public IReadOnlyList<Card> Cards => cards;
    public void AddCard(Card card) => cards.Add(card);
    public void Clear() => cards.Clear();

    public int GetBestValue() {
        int total = cards.Sum(c => c.BaseValue);
        int aceCount = cards.Count(c => c.CardIsAce);

        // Upgrade Aces from 1 → 11 if it doesn’t bust
        while (aceCount > 0 && total + 10 <= 21) {
            total += 10;
            aceCount--;
        }
        return total;
    }

    public int GetValueByCard(int index) => cards[index].BaseValue;
}

public enum CardType {
    Hearts,
    Diamonds,
    Spades,
    Clubs,
}
