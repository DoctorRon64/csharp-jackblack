using _Utils;

public class DeckManager {
    public List<Card> Deck;

    public DeckManager() {
        Deck = new();
        InitDeck();
    }

    private void InitDeck() {
        const int cardSizePerType = 14;
        int cardTypeSize = Enum.GetNames(typeof(CardType)).Length;

        for (int i = 0; i < cardTypeSize; i++) {
            for (int u = 1; u < cardSizePerType; u++) {
                Card newCard = new Card(u, (CardType)i);
                Deck.Add(newCard);
                //Console.WriteLine($"Added new card with Id: {newCard.Id} and is {newCard.Type}");
            }
        }
    }

    public void ShuffleDeck() {
        if (Utils.Instance != null) Deck = Utils.Instance.Shuffle(Deck);
        //Console.WriteLine("Cards are Shuffled");
    }
}

public class Card {
    public Card(int id, CardType type) {
        Id = id;
        Type = type;
        DetermineValue();
    }

    private void DetermineValue() {
        switch (Id) {
            case 1:
                CardIsAce = true;
                baseValue = 1; // Default, player can choose 11 later
                break;
            case 11: // Jack
            case 12: // Queen
            case 13: // King
                baseValue = 10;
                break;
            default:
                baseValue = Id;
                break;
        }
    }

    public int GetValue(bool useElevenForAce = false) {
        if (CardIsAce && useElevenForAce == false) {
            return 11;
        }
        return baseValue;
    }

    public int Id { get; private set; }
    public CardType Type { get; private set; }
    public bool CardIsAce { get; private set; } = false;
    private int baseValue;
}

public enum CardType {
    Hearts,
    Diamonds,
    Spades,
    Clubs,
}