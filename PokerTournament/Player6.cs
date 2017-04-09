using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player6 : Player
    {
        private enum Actions { CHECK, BET, CALL, RAISE, FOLD };
        private enum TurnOrder { FIRST, SECOND};
        
        int enemyFirstBet;
        int roundNum;
        int startingMoney;
        int lastActionStr;
        int lastActionAmount;
        Actions currentAction;
        int amountToBet;
        int bluffChance;

        public Player6(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
            enemyFirstBet = 0;
            roundNum = 0;
            startingMoney = mny;
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

            //info to keep track of
            bool isFirst = false;
            
            // evaluate the hand
            Card highCard = null;
            int handStrength = Evaluate.RateAHand(testHand, out highCard);

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
                isFirst = true;
            }

            //setup action
            //Actions action = Actions.BET;
            PlayerAction pa = null;
            //int amount = 20;
            Random rng = new Random();

            //start turn
            Console.WriteLine("\n-> in ai betting round 1");
            Console.WriteLine("   Total games played: "+roundNum);
            ListTheHand(testHand);

            //if ai is first
            // actions available: bet, check, fold
            if (Round1FirstCheck(actions, handStrength, startingMoney, out pa))
                return pa;
            else
            {
                pa = new PlayerAction(Name, "Bet1", "fold", 0);
            }
                //int riskTaking= 0; //low=take no risk -- high=take much risk
                                              //probably replace with calculated value based on overall game
                //int risk = 0;
                //amount = 0;
                //int act = 0;

                //check hand strength
                //switch (handStrength)
                //{
                //    case 1: //fold, check or evaluate risk
                //        riskTaking = rng.Next(4); //low=take no risk -- high=take much risk
                //
                //        switch (riskTaking)
                //        {
                //            case 0: action = Actions.FOLD;
                //                Console.WriteLine("AI plays it safe and folds");
                //                break;
                //            case 1: action = Actions.FOLD;
                //                Console.WriteLine("AI plays it safe and folds");
                //                break;
                //            case 2:
                //                act = rng.Next(3); //2/3 chance of folding
                //                if (act == 0) action = Actions.FOLD;
                //                else if (act == 1) action = Actions.FOLD;
                //                else if (act == 2) action = Actions.CHECK;
                //                Console.WriteLine("AI is considering taking a risk");
                //                break;
                //            case 3: action = Actions.BET;
                //                amount = 10; //<- replace with calc bet amount
                //                Console.WriteLine("AI is taking a risk");
                //                break;
                //        }
                //        break;
                //    case 2: //fold, check or evaluate risk
                //        break;
                //    case 3: //fold, check or evaluate risk
                //        break;
                //    case 4: //check or evaluate risk
                //        break;
                //    case 5: //check or evaluate risk
                //        break;
                //    case 6: //check or evaluate risk
                //        break;
                //    case 7: //check or evaluate risk
                //        break;
                //    case 8: action = Actions.BET;
                //            //calculate bet amount
                //        break;
                //    case 9: action = Actions.BET;
                //        //calculate bet amount
                //        break;
                //    case 10: action = Actions.BET;
                //        //calculate bet amount
                //        break;
                //}
            //}
            //else
            //{
            //
            //}



            //end turn and submit action
            //create the PlayerAction
            //switch (action)
            //{
            //    case Actions.BET: pa = new PlayerAction(Name, "Bet1", "bet", amount); break;
            //    case Actions.RAISE: pa = new PlayerAction(Name, "Bet1", "raise", amount); break;
            //    case Actions.CALL: pa = new PlayerAction(Name, "Bet1", "call", amount); break;
            //    case Actions.CHECK: pa = new PlayerAction(Name, "Bet1", "check", amount); break;
            //    case Actions.FOLD: pa = new PlayerAction(Name, "Bet1", "fold", amount); break;
            //    default: Console.WriteLine("Invalid menu selection - try again"); break;
            //} Console.WriteLine("< end ai betting round 1 >");
            return pa;
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

        // the behavior tree will analyze which action seems most logical
        // it will then feed that info into the current state which our FSM
        // will process.

        // behavior tree nodes
        private bool Round1FirstCheck(List<PlayerAction> actions, int handStrength, int currentMoney, out PlayerAction action)
        {
            action = null;
            // check am I first?
            // if so, step into action selector
            if (actions.Count <= 0)
            {
                action = Round1ActionSelector(TurnOrder.FIRST, handStrength, currentMoney, 0);
                return true;
            }
            
            // otherwise return null to represent that we can't act
            return false;
        }

        private PlayerAction Round1ActionSelector(TurnOrder myOrder, int handStrength, int currentMoney, int opponentBet)
        {
            // decalre a param to hold the actual bet
            int bettingAmount;

            // if an action is processed, then return true
            switch(myOrder)
            {
                // all we have to go off of in the first round is our own hand strength
                case TurnOrder.FIRST: // currently in the first turn, the actions we can select are bet, check, and fold
                    if (Round1Check(handStrength))
                    {
                        currentAction = Actions.CHECK;
                        return new PlayerAction(Name, "Bet1", "check", 0);
                    }
                    if (Round1Bet(handStrength, currentMoney, out bettingAmount))
                    {
                        currentAction = Actions.BET;
                        return new PlayerAction(Name, "Bet1", "bet", bettingAmount);
                    }
                    if (Round1Fold(handStrength, 0))
                    {
                        currentAction = Actions.FOLD;
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    }
                    break;
                case TurnOrder.SECOND: // currently it is the second turn, the actions we can select are call, raise, fold
                    if (Round1PossibleCheck())
                        return new PlayerAction(Name, "Bet1", "check", 0);
                    if (Round1Call())
                        return new PlayerAction(Name, "Bet1", "call", 0);
                    if (Round1Raise())
                        return new PlayerAction(Name, "Bet1", "raise", 0);
                    if (Round1Fold(handStrength, opponentBet))
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    break;
                default:
                    Console.WriteLine("error: turn order not specified.");
                    return null;
            }

            // otherwise something went wrong and return false
            return null;
        }

        private bool Round1Bet(int handStrength, int currentMoney, out int bettingAmount)
        {
            bettingAmount = 0;
            // the AI is analyzing whether it should bet for round 1
            // assume value bet, that is, make the highest bet we think the opponent will call
            // first, let's calculate our highest bet we're willing to do, we don't want to scare them off by going all in
            int highestWillingBet = currentMoney / 3;

            // determine whether we should bet
            if (handStrength == 2)
            {
                bettingAmount = highestWillingBet / 3;
                return true;
            }
            else if (handStrength == 3)
            {
                bettingAmount = highestWillingBet / 2;
                return true;
            }
            else if (handStrength > 3)
            {
                bettingAmount = highestWillingBet;
                return true;
            }

            return false;
        }
        private bool Round1Check(int handStrength)
        {
            // the AI is analyzing whether it should check for round 1
            if (handStrength == 1)
                return true;

            return false;
        }
        private bool Round1Fold(int handStrength, int opponentBet)
        {
            // the AI is analyzing whether it should fold for round 1
            if (opponentBet >= (100 * handStrength))
                return true;

            return false;
        }
        private bool Round1Call()
        {
            // the AI is analyzing whether it should call for round 1
            return false;
        }
        private bool Round1Raise()
        {
            // the AI is analyzing whether it should raise for round 1
            return false;
        }

        private bool Round1PossibleCheck()
        {
            // can I check?
            // if so, step into the checking selector

            // otherwise return false
            return false;
        }

        private bool Round1CheckingSelector()
        {
            // should I check?
            // if so, set the current state to checking and return true

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
