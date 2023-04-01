using System;

namespace PokerSolver
{
    public class PokerSolver
    {   
        // FUNCTION: Main function to test PokerSolver class.
        public static void Main(string[] args)
        {
            // VAR: Declare test hands.
            var hand1 = new int[] {6, 2, 6, 6, 5}; // 3 of a kind
            var hand2 = new int[] {1, 2, 3, 4, 13}; // Wheel Straight
            Console.WriteLine("Welcome to Poker Solver!");
            Console.WriteLine("Solving with hands: [" + string.Join(",", hand1) + "] and [" + string.Join(",", hand2) + "] ..");
            
            // VAR: Create new instance of PokerSolver.
            var pokerSolver = new PokerSolver();
            
            // VAR: Call PokerHandSolver and print out result.
            int result = pokerSolver.PokerHandSolver(hand1, hand2);
            Console.WriteLine("Function returns: " + result);
            
            // Wait for user input to exit.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// PokerHandSolver - Takes in 2 hands and returns the winner, 0 if tie, -1 if error.
        /// * Scope: A valid hand is an array of 5 integers. Deck is 52 cards, no jokers, no suits, aces high. *
        /// </summary>
        /// <param name="player1Hand">Player 1's hand</param>
        /// <param name="player2Hand">Player 2's hand</param>
        /// <returns>0 if tie, 1 if player 1 wins, 2 if player 2 wins, -1 if error</returns>
        public int PokerHandSolver(int[] player1Hand, int[] player2Hand)
        {
            // VAR: Evaluate hands and store results
            int player1Result = EvaluateHand(player1Hand);
            int player2Result = EvaluateHand(player2Hand);

            // // DEBUG: Print out results
            // Console.WriteLine("Player 1 Result: " + player1Result);
            // Console.WriteLine("Player 2 Result: " + player2Result);

            // TO-DO: Decouple hand validity check to an input validity function once input interface is designed.
            // LOGIC: Check if hands are valid, and return resulting winner [0, 1, 2, -1]
            if (player1Result == 0 || player2Result == 0 || player1Hand.Length != 5 || player2Hand.Length != 5)
            {
                // Invalid hand
                return -1;
            }
            else if (player1Result < player2Result)
            {
                // Player 1 wins
                return 1;
            }
            else if (player1Result > player2Result)
            {
                // Player 2 wins
                return 2;
            }
            else
            {
                // Tie
                return 0;
            }
        }

        // ENUM: HandRank - Enum for hand ranking [0, 1, 2, 3, 4, 5, 6, 7] where 1 is best and 7 is worst, 0 if invalid card or error.
        public enum HandRank
        {
            Invalid = 0,
            FourOfAKind = 1, 
            FullHouse = 2,
            Straight = 3,
            ThreeOfAKind = 4,
            TwoPair = 5,
            OnePair = 6,
            HighCard = 7
        }
        /// <summary>
        /// EvaluateHand - Takes in a hand and returns the hand ranking.
        /// * Scope: Deck is 52 cards, no jokers, no suits, aces high. *
        /// </summary>
        /// <param name="hand">Hand to evaluate</param>
        /// <returns>Hand ranking [0, 1, 2, 3, 4, 5, 6, 7] where 1 is best and 7 is worst, 0 if invalid card or error.</returns>
        private static int EvaluateHand(int[] hand)
        {
            /* LOGIC: Valid hand is an array of 5 integers.
                1. Check if cards are valid - [1-13].
                2. Find unique cards and max repeat amount; And sort unique cards for straight check later.
                3. Evaluate hand ranking.
            */
            
            // TO-DO: Decouple card validity check to an input validity function once input interface is designed.
            // TEST: check if cards are valid in the hand passed in.
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i] < 1 || hand[i] > 13)
                {
                    // Invalid card - cannot be less than 1 (TWO) or greater than 13 (ACE).
                    return (int)HandRank.Invalid;
                }
            }

            // TO-DO: Decouple helper vars and logic to a helper class once scope is determined.
            // HELPER VAR: uniqueCardsArray is an array of unique cards in the hand passed in.
            int[] uniqueCardsArray = new int[0];

            // HELPER VAR: maxRepeat is the max number of times a card repeats in the hand passed in.
            int maxRepeat = 0;
            
            // HELPER LOGIC: Find unique cards and max repeat, and sort unique cards for straight check later.
            for (int i = 0; i < hand.Length; i++)
            {
                if (Array.IndexOf(uniqueCardsArray, hand[i]) == -1)
                {
                    int count = 0;
                    for (int j = 0; j < hand.Length; j++)
                    {
                        if (hand[i] == hand[j])
                        {
                            count++;
                        }
                    }
                    if (count > maxRepeat)
                    {
                        maxRepeat = count;
                    }
                    Array.Resize(ref uniqueCardsArray, uniqueCardsArray.Length + 1);
                    uniqueCardsArray[uniqueCardsArray.Length - 1] = hand[i];
                    Array.Sort(uniqueCardsArray);
                }
            }

            // // DEBUG: Print out attributes of hand
            // Console.WriteLine("Hand: [" + string.Join(",", hand) + "]");
            // Console.WriteLine("Max Repeat: " + maxRepeat);
            // Console.WriteLine("New Hand: [" + string.Join(",", uniqueCardsArray) + "]"); 

            // LOGIC: Evaluate hand ranking based on helpers 'unique cards' and 'max repeat'.
            if (uniqueCardsArray.Length == 4) // 4 unique cards in hand of 5 = one pair
            {
                // One pair
                return (int)HandRank.OnePair;
            }
            if (uniqueCardsArray.Length == 3) // 3 unique cards in hand of 5 = two pair || three of a kind
            {
                if (maxRepeat == 3) // 3 repeats of 1 card = three of a kind
                {
                    // Three of a kind
                    return (int)HandRank.ThreeOfAKind;
                }
                else
                {
                    // Two pair
                    return (int)HandRank.TwoPair;
                }
            }
            if (uniqueCardsArray.Length == 2) // 2 unique cards in hand of 5 = four of a kind || full house
            {
                if (maxRepeat == 4) // 4 repeats of 1 card = four of a kind
                {
                    // Four of a kind
                    return (int)HandRank.FourOfAKind;
                }
                else
                {
                    // Full house
                    return (int)HandRank.FullHouse;
                }
            }
            if (uniqueCardsArray.Length == 5) // 5 unique cards in hand of 5 = straight || high card
            {
                /* LOGIC: Check if hand is a straight. If not, it is a high card.
                    1. Check if hand is sequential (excluding wheel).
                    2. Check if hand is a wheel (A, 2, 3, 4, 5).
                */
                
                // HELPER VAR: isSequential is a bool to check if hand is sequential (excluding wheel)
                bool isSequential = true;

                // Check if hand is sequential (excluding wheel)
                for (int i = 0; i < uniqueCardsArray.Length - 1; i++) 
                {
                    if (uniqueCardsArray[i] + 1 != uniqueCardsArray[i + 1])
                    {
                        isSequential = false;
                        break;
                    }
                }

                // HELPER VAR: isWheel is a bool to check if hand is a wheel (A, 2, 3, 4, 5)
                bool isWheel = false;

                // Check if hand is a wheel (A, 2, 3, 4, 5)
                if (uniqueCardsArray[0] == 1 && uniqueCardsArray[1] == 2 && uniqueCardsArray[2] == 3 && uniqueCardsArray[3] == 4 && uniqueCardsArray[4] == 13)
                {
                    isWheel = true;
                }
                if (isSequential || isWheel) // If hand is sequential or a wheel, it is a straight.
                {
                    // Straight
                    return (int)HandRank.Straight;
                }
                else // If hand is not sequential or a wheel, it is a high card.
                {
                    // High card
                    return (int)HandRank.HighCard;
                }
            }
            // Valid hands of 5, in a deck of 52 cards, will always have > 1 unique cards.
            return (int)HandRank.Invalid;
        }
    }
}