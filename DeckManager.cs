using _Utils;

public class DeckManager {
    public Stack<Card> Deck;

    public DeckManager() {
        InitDeck();
    }

    private void InitDeck() {
        List<Card> list = new();
        const int cardSizePerType = 14;
        int cardTypeSize = Enum.GetNames(typeof(CardType)).Length;

        for (int i = 0; i < cardTypeSize; i++) {
            for (int u = 1; u < cardSizePerType; u++) {
                list.Add(new Card(u, (CardType)i));
                //Console.WriteLine($"Added new card with Id: {newCard.Id} and is {newCard.Type}");
            }
        }

        Deck = new Stack<Card>(list);
    }

    public void ShuffleDeck() {
        if (Utils.Instance == null) return;

        var list = Deck.ToList();
        list = Utils.Instance.Shuffle(list);

        Deck = new Stack<Card>(list);     
        //Console.WriteLine("Cards are Shuffled");
    }

    public Card Draw() {
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
}

public class Hand {
    private List<Card> cards = new();

    public void AddCard(Card card) => cards.Add(card);
    public void Clear() => cards.Clear();
    public IReadOnlyList<Card> Cards => cards;

    public int GetBestValue() {
        int total = cards.Sum(c => c.BaseValue);
        int aceCount = cards.Count(c => c.CardIsAce);

        // Try upgrading Aces from 1 → 11 when it helps
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