using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player6 : Player
    {
        const int ANTE = 20;

        private enum Actions { CHECK, BET, CALL, RAISE, FOLD };
        private enum TurnOrder { FIRST, SECOND};

        // risk is a value from -.09 - 0.9
        // risk is calcuted by weighing the projected enemy hand against our own
        float risk = 0.0f;

        // this value represents the hand we believe our opponent to have
        // the scale is from 0.0 - 1.0
        float estimatedEnemyHand = 0.0f;

        Random rng;
        int enemyFirstBet;
        int roundNum;
        int startingMoney;
        int currentMoney;
        
        int lastActionStr;
        int lastActionAmount;
        Actions currentAction;

        public Player6(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
            enemyFirstBet = 0;
            roundNum = 0;
            startingMoney = mny;
            currentMoney = startingMoney;
            rng = new Random();
        }

        //Probabilities: http://www.math.hawaii.edu/~ramsey/Probability/PokerHands.html

        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            // create a test hand to test reactions
            Card[] testHand = new Card[5];
            testHand[0] = new Card("Spades", 14);
            testHand[1] = new Card("Diamonds", 14);
            testHand[2] = new Card("Clubs", 14);
            testHand[3] = new Card("Spades", 13);
            testHand[4] = new Card("Hearts", 13);

            int test = (int)(((float)rng.Next(33, 100) / 0.01) * 100);
            //Console.Writ

            //info to keep track of
            roundNum = actions.Count;
            
            // evaluate the hand
            Card highCard = null;
            int handStrength = Evaluate.RateAHand(hand, out highCard);

            //update stats
            if (actions.Count > 0) //ai goes second
            {
                Console.WriteLine("\n*round stats-> First Player: " + actions[0].Name + " total turns taken: " + actions.Count);
                for (int i = 0; i < actions.Count; i++)
                {
                    Console.WriteLine("   (" + (i) + "): PLR: " + actions[i].Name + ", action: " + actions[i].ActionName + ", inPhase: " + actions[i].ActionPhase + ", amount: " + actions[i].Amount);
                }
            }
            else //ai goes first
            {
                Console.WriteLine("\n*round stats-> player 1: " + Name + " takes first turn of round.");
            }

            //setup action
            //Actions action = Actions.BET;
            PlayerAction pa = null;
            //int amount = 20;

            //start turn
            Console.WriteLine("\n-> in ai betting round 1");
            Console.WriteLine("   Total games played: "+roundNum);
            ListTheHand(hand);

            //if ai is first
            // actions available: bet, check, fold
            if (Round1FirstCheck(handStrength, out pa, highCard))
            {
                // we're at round 0, increment so we get an accurate read
                //roundNum++;
                return pa;
            }
            else // the ai is going second, actions available: check, call, raise, fold
            {
                // evaluate the enemy's hand
                estimatedEnemyHand = EvaluateEnemyHand(actions);
                Console.WriteLine("Estimated enemy hand strength: " + estimatedEnemyHand);
 
                pa = Round1ActionSelector(TurnOrder.SECOND, actions[roundNum - 1], handStrength, highCard);
                return pa;
            }
        }

        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            // select an action
            string actionSelection = "5";
            PlayerAction pa = null;
            int amount = 0;

            Console.WriteLine("-> in ai betting round 2");

            // create the PlayerAction
            switch (actionSelection)
            {
                case "1": pa = new PlayerAction(Name, "Bet2", "bet", amount); break;
                case "2": pa = new PlayerAction(Name, "Bet2", "raise", amount); break;
                case "3": pa = new PlayerAction(Name, "Bet2", "call", amount); break;
                case "4": pa = new PlayerAction(Name, "Bet2", "check", amount); break;
                case "5": pa = new PlayerAction(Name, "Bet2", "fold", amount); break;
                default: Console.WriteLine("Invalid menu selection - try again"); break;
            } Console.WriteLine("< end ai betting round 2 >");

            roundNum++;
            return pa;
        }

        public override PlayerAction Draw(Card[] hand)
        {
            Console.WriteLine("-> in ai draw round");

            // determine how many cards to delete
            int numCardsToDelete = 0;
            int[] cardsToDelete;
            Card curHighCard;
            int curHandStrength = Evaluate.RateAHand(hand, out curHighCard);
            bool takeASmallRisk = false;
            bool takeABigRisk = false;

            if (enemyFirstBet > 250 && enemyFirstBet < 500)
            {
                takeASmallRisk = true;
            } else if (enemyFirstBet > 500)
            {
                takeASmallRisk = true;
                takeABigRisk = true;
            }

            switch (curHandStrength)
            {
                case 1:
                    numCardsToDelete = 4;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    numCardsToDelete = 0;
                    break;
                case 9:
                    numCardsToDelete = 0;
                    break;
                case 10:
                    numCardsToDelete = 0;
                    break;
            }

            PlayerAction pa = null;
            if (curHandStrength > 5)
            {
                if (curHandStrength >= 8)
                {
                    pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                }
                else
                {

                }
            }
            else
            {

            }

            cardsToDelete = new int[numCardsToDelete];

            // which cards to delete if any
            if (numCardsToDelete > 0 && numCardsToDelete < 5)
            {
                for (int i = 0; i < numCardsToDelete; i++) // loop to delete cards
                {
                    Console.WriteLine("\nDelete card " + (i + 1) + ":");
                    for (int j = 0; j < hand.Length; j++)
                    {
                        Console.WriteLine("{0} - {1}", (j + 1), hand[j]);
                    }
                    // selete cards to delete
                    int delete = 0;
                    do
                    {

                        Console.Write("Which card to delete? (1 - 5): ");
                        string delStr = Console.ReadLine();
                        int.TryParse(delStr, out delete);

                        // see if the entry is valid
                        if (delete < 1 || delete > 5)
                        {
                            Console.WriteLine("Invalid entry - enter a value between 1 and 5.");
                            delete = 0;
                        }
                        else if (hand[delete - 1] == null)
                        {
                            Console.WriteLine("Entry was already deleted.");
                            delete = 0;
                        }
                        else
                        {
                            hand[delete - 1] = null; // delete entry
                            delete = 99; // flag to exit loop
                        }
                    } while (delete == 0);
                }
                // set the PlayerAction object
                pa = new PlayerAction(Name, "Draw", "draw", numCardsToDelete);
            }
            else if (numCardsToDelete == 5)
            {
                // delete them all
                for (int i = 0; i < hand.Length; i++)
                {
                    hand[i] = null;
                }
                pa = new PlayerAction(Name, "Draw", "draw", 5);
            }
            else // no cards deleted
            {
                pa = new PlayerAction(Name, "Draw", "stand pat", 0);
            }

            // return the action
            return pa;
        }

        private float EvaluateEnemyHand(List<PlayerAction> enemyActions)
        {
            float estimatedValue = 0.0f;

            // sum up all enemy bets so far
            int totalEnemyBid = 0;
            for (int i = 0; i < enemyActions.Count; i++)
            {
                totalEnemyBid += enemyActions[i].Amount;

                // if the enemy suddenly jumps at a certain point, assume they are trying to trick us
                if (i > 1 && enemyActions[i].Amount - enemyActions[i - 1].Amount > 200)
                {
                    risk -= (float)rng.NextDouble() * (-.1f);
                    estimatedValue += 0.2f;
                }
            }

            if (roundNum > 0)
            { 
                int averageBet = totalEnemyBid / roundNum;

                if (averageBet > 20 && averageBet < 50)
                {
                    risk += (float)rng.NextDouble() * (.1f);
                    estimatedValue+= 0.2f;
                }
                if (averageBet > 49 && averageBet < 100)
                {
                    risk = 0.0f;
                    estimatedValue+= 0.3f;
                }
                if (averageBet > 99 && averageBet < 200)
                {
                    risk -= (float)rng.NextDouble() * (-.2f);
                    estimatedValue += 0.4f;
                }
                if (averageBet > 199 && averageBet < 1000)
                {
                    risk -= (float)rng.NextDouble() * (-.4f);
                    estimatedValue += 0.9f;
                }
            }

            return estimatedValue;
        }

        // the behavior tree will analyze which action seems most logical
        // it will then feed that info into the current state which our FSM
        // will process.

        // behavior tree nodes
        private bool Round1FirstCheck(int handStrength, out PlayerAction action, Card highCard)
        {
            action = null;
            // check am I first?
            // if so, step into action selector
            if (roundNum <= 0)
            {
                currentMoney -= ANTE;
                action = Round1ActionSelector(TurnOrder.FIRST, null, handStrength, highCard);
                return true;
            }
            
            // otherwise return null to represent that we can't act
            return false;
        }

        private PlayerAction Round1ActionSelector(TurnOrder myOrder, PlayerAction prevAction, int handStrength, Card highCard)
        {
            // decalre a param to hold the actual bet
            int bettingAmount;

            // if an action is processed, then return true
            switch(myOrder)
            {
                // all we have to go off of in the first round is our own hand strength
                case TurnOrder.FIRST: // currently in the first turn, the actions we can select are bet, check, and fold
                    if (Round1Check(handStrength, highCard))
                    {
                        currentAction = Actions.CHECK;
                        //lastActionStr = "check";
                        lastActionAmount = 0;
                        return new PlayerAction(Name, "Bet1", "check", 0);
                    }
                    if (Round1Bet(handStrength, out bettingAmount, highCard))
                    {
                        currentAction = Actions.BET;
                        currentMoney -= bettingAmount;
                        lastActionAmount = bettingAmount;
                        return new PlayerAction(Name, "Bet1", "bet", bettingAmount);
                    }
                    if (Round1Fold(handStrength, prevAction))
                    {
                        currentAction = Actions.FOLD;
                        lastActionAmount = 0;
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    }
                    break;
                case TurnOrder.SECOND: // currently it is the second turn, the actions we can select are call, raise, fold
                    if (Round1PossibleCheck(handStrength, prevAction, highCard))
                    {
                        currentAction = Actions.CHECK;
                        lastActionAmount = 0;
                        return new PlayerAction(Name, "Bet1", "check", 0);
                    }
                    if (Round1PossibleBet(handStrength, prevAction, out bettingAmount, highCard))
                    {
                        currentAction = Actions.BET;
                        lastActionAmount = bettingAmount;
                        return new PlayerAction(Name, "Bet1", "bet", bettingAmount);
                    }
                    if (Round1Raise(handStrength, prevAction, out bettingAmount, highCard))
                    {
                        currentAction = Actions.RAISE;
                        lastActionAmount = bettingAmount;
                        return new PlayerAction(Name, "Bet1", "raise", bettingAmount);
                    }
                    if (Round1Call(handStrength, prevAction, out bettingAmount))
                    {
                        currentAction = Actions.CALL;
                        lastActionAmount = bettingAmount;
                        return new PlayerAction(Name, "Bet1", "call", bettingAmount);
                    }
                    if (Round1Fold(handStrength, prevAction))
                    {
                        currentAction = Actions.FOLD;
                        lastActionAmount = 0;
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    }
                    break;
                default:
                    Console.WriteLine("error: turn order not specified.");
                    return new PlayerAction(Name, "Bet1", "fold", 0);
            }

            // otherwise something went wrong and return fold
            Console.WriteLine("Something went wrong in selecting an action...");
            return new PlayerAction(Name, "Bet1", "fold", 0);
        }

        private bool Round1Bet(int handStrength, out int bettingAmount, Card highCard)
        {
            bettingAmount = 0;          
            // the AI is analyzing whether it should bet for round 1
            // assume value bet, that is, make the highest bet we think the opponent will call
            // first, let's calculate our highest bet we're willing to do, we don't want to scare them off by going all in
            int highestWillingBet = currentMoney / 4;


            if (handStrength == 1 && highCard.Value > 12)
            {
                int baseBet = highestWillingBet / 6;
            }

            // determine whether we should bet
            if (handStrength == 2)
            {
                int baseBet = highestWillingBet / 3;
                bettingAmount = rng.Next(baseBet / 2, baseBet);
                return true;
            }
            else if (handStrength == 3)
            {
                int baseBet = highestWillingBet / 2;
                bettingAmount = rng.Next(baseBet / 2, baseBet);
                return true;
            }
            else if (handStrength > 3)
            {
                int baseBet = highestWillingBet;
                bettingAmount = rng.Next(baseBet / 2, baseBet);
                return true;
            }

            return false;
        }
        private bool Round1Check(int handStrength, Card highCard)
        {
            // the AI is analyzing whether it should check for round 1
            if (handStrength == 1)
                return true;

            return false;
        }
        private bool Round1Fold(int handStrength, PlayerAction prevAction)
        {
            // the AI is analyzing whether it should fold for round 1
            //if (prevAction != null && prevAction.Amount > (100 * handStrength))
                return true;

            //return false;
        }
        private bool Round1Call(int handStrength, PlayerAction prevAction, out int callAmount)
        {
            int highestWillingBet = handStrength * 50;
            callAmount = 0;
            // the AI is analyzing whether it should call for round 1
            float diffHands = ((float)handStrength * 0.1f) - estimatedEnemyHand;

            // first, check the states in which it can call
            if (prevAction.ActionName == "bet" || prevAction.ActionName == "raise")
            {
                Console.WriteLine("Difference Between Hands: " + diffHands);
                if (prevAction.Amount <= highestWillingBet && diffHands >= (-0.2 + risk))
                {
                    //int amountToBet = handStrength * 25;
                    if (handStrength >= 1)
                    {
                        if (prevAction.Amount < currentMoney)
                            callAmount = 0;
                        else
                            callAmount = 0;

                        currentMoney -= callAmount;
                        return true;
                    }

                }
            }
            return false;
        }
        private bool Round1Raise(int handStrength, PlayerAction prevAction, out int raiseAmount, Card highCard)
        {
            raiseAmount = 0;
            // the AI is analyzing whether it should raise for round 1
            //int highestWillingBet = currentMoney / 2;
            int tempRaiseAmount = handStrength * 50;

            // difference between our hand and the estimated enemy one
            // if the difference in hands is negative then they've maybe got a better hand than us
            float diffHands = ((float)handStrength * 0.1f) - estimatedEnemyHand;


            if (prevAction.ActionName == "check")
            {
                // if they checked, try to lure them in by offering a low bet
                if (handStrength * 15 < currentMoney)
                    raiseAmount = handStrength * 15;
                else
                    raiseAmount = currentMoney;

                currentMoney -= raiseAmount + prevAction.Amount;
                return true;
            }

            // first, check the states in which it can raise
            if (prevAction.ActionName == "bet" || prevAction.ActionName == "raise")
            {

                if (diffHands >= (0.1 + risk) && highCard.Value > rng.Next(9, 11))
                {
                    if (handStrength > 1)
                    {
                        if (tempRaiseAmount / 2 < currentMoney)
                            raiseAmount = rng.Next(tempRaiseAmount / 4, tempRaiseAmount / 2) + (int)(risk * 10);
                        else
                            raiseAmount = currentMoney;

                        currentMoney -= raiseAmount + prevAction.Amount;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Round1PossibleCheck(int handStrength, PlayerAction prevAction, Card highCard)
        {
            // can I check?
            // if so, step into the checking selector
            if (prevAction.ActionName == "check")
                return Round1Check(handStrength, highCard);

            // otherwise return false
            return false;
        }

        private bool Round1PossibleBet(int handStrength,PlayerAction prevAction, out int bettingAmount, Card highCard)
        {
            bettingAmount = 0;

            // can I bet?
            // if so, step into the betting selector
            if (prevAction == null || prevAction.ActionName == "check")
                return Round1Bet(handStrength, out bettingAmount, highCard);

            // otherwise return false
            return false;
        }

        // helper method - list the hand
        // temporarily copied into here so we can see what the AI has
        // TODO: delete when AI is working properly
        private void ListTheHand(Card[] hand)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.WriteLine("\nName: " + Name + " Your hand:   Rank: " + rank);
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write(hand[i].ToString() + " ");
            }
            Console.WriteLine();
        }
    }
}
