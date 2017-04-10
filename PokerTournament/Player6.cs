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

        float estimatedEnemyStrength;
        int roundNum;
        int startingMoney;
        int currentMoney;
        int lastActionStr;
        int lastActionAmount;
        Actions currentAction;
        int amountToBet;
        int bluffChance;

        public Player6(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
            roundNum = 0;
            startingMoney = mny;
            currentMoney = startingMoney;
        }

        //Probabilities: http://www.math.hawaii.edu/~ramsey/Probability/PokerHands.html

        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            // create a test hand to test reactions
            Card[] testHand = new Card[5];
            testHand[0] = new Card("Spades", 2);
            testHand[1] = new Card("Diamonds", 2);
            testHand[2] = new Card("Clubs", 7);
            testHand[3] = new Card("Spades", 5);
            testHand[4] = new Card("Hearts", 12);

            //info to keep track of
            bool isFirst = false;
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
            //ListTheHand(testHand);

            //if ai is first
            // actions available: bet, check, fold
            if (Round1FirstCheck(handStrength, out pa))
            {
                // we're at round 0, increment so we get an accurate read
                //roundNum++;
                return pa;
            }
            else // the ai is going second, actions available: check, call, raise, fold
            {
                //roundNum++;
                pa = Round1ActionSelector(TurnOrder.SECOND, actions[roundNum - 1], handStrength);
                //int aas = roundNum - 1;
                //Console.WriteLine("roundNum - 1 = " + aas + " roundNum = " + actions.Count);
                //pa = new PlayerAction(Name, "Bet1", "fold", 0);
                return pa;
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
            //return pa;
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
            Card curHighCard;
            int curHandStrength = Evaluate.RateAHand(hand, out curHighCard);
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ {0}", curHandStrength);
            bool takeASmallRisk = false;
            bool takeABigRisk = false;
            
            if (estimatedEnemyStrength > 0.2f && estimatedEnemyStrength <= 0.4f)
            {
                takeASmallRisk = true;
            } else if (estimatedEnemyStrength > 0.4f)
            {
                takeASmallRisk = true;
                takeABigRisk = true;
            }

            
            //Decision Tree (if statements for now)
            PlayerAction pa = null;
            List<int> deleteIndices = new List<int>();
            if (curHandStrength >= 5)
            {
                if (curHandStrength >= 7)
                {
                    if (curHandStrength == 8)
                    {
                        if (curHighCard.Value > 10)
                        {
                            Console.WriteLine("#####################1");
                            pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                        }
                        else
                        {
                            Console.WriteLine("#####################2");
                            DeleteCards(hand, UnmatchingCards(hand, 4));
                            pa = new PlayerAction(Name, "Draw", "draw", 1);
                        }
                    }
                    else
                    {
                        Console.WriteLine("#####################3");
                        pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                    }
                }
                else
                {
                    if (takeABigRisk)
                    {
                        if (curHandStrength == 5)
                        {
                            if (SimilarSuitedCards(hand, out deleteIndices) == 4)
                            {
                                DeleteCards(hand, deleteIndices);
                                Console.WriteLine("#####################4");
                                pa = new PlayerAction(Name, "Draw", "draw", 1);
                            }
                            else
                            {
                                Console.WriteLine("#####################5");
                                pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                            }
                        }
                        else
                        {
                            if (ConsecutiveCards(hand, out deleteIndices) == 4)
                            {
                                DeleteCards(hand, deleteIndices);
                                Console.WriteLine("#####################6");
                                pa = new PlayerAction(Name, "Draw", "draw", 1);
                            }
                            else
                            {
                                Console.WriteLine("#####################7");
                                pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("#####################8");
                        pa = new PlayerAction(Name, "Draw", "stand pat", 0);
                    }
                }
            }
            else
            {
                if (takeASmallRisk)
                {
                    if (curHandStrength > 2)
                    {
                        if (curHandStrength == 4)
                        {
                            DeleteCards(hand, UnmatchingCards(hand, 3));
                        }
                        else
                        {
                            if (SimilarSuitedCards(hand, out deleteIndices) >= 3)
                            {
                                Console.WriteLine("#####################9");
                                DeleteCards(hand, deleteIndices);
                                pa = new PlayerAction(Name, "Draw", "draw", deleteIndices.Count);
                            }
                            else if (ConsecutiveCards(hand, out deleteIndices) >= 3)
                            {
                                Console.WriteLine("#####################10");
                                DeleteCards(hand, deleteIndices);
                                pa = new PlayerAction(Name, "Draw", "draw", deleteIndices.Count);
                            }
                            else
                            {
                                hand[OddCardOut(hand)] = null;
                            }
                        }
                    }
                    else
                    {
                        if (SimilarSuitedCards(hand, out deleteIndices) >= 3)
                        {
                            DeleteCards(hand, deleteIndices);
                            Console.WriteLine("#####################11");
                            pa = new PlayerAction(Name, "Draw", "draw", deleteIndices.Count);
                        }
                        else if (ConsecutiveCards(hand, out deleteIndices) >= 3)
                        {
                            DeleteCards(hand, deleteIndices);
                            Console.WriteLine("#####################12");
                            pa = new PlayerAction(Name, "Draw", "draw", deleteIndices.Count);
                        }
                        else
                        {
                            if (curHandStrength == 2)
                            {
                                DeleteCards(hand, UnmatchingCards(hand, 2));
                                Console.WriteLine("#####################13");
                                pa = new PlayerAction(Name, "Draw", "draw", 3);
                            }
                            else
                            {
                                for (int i = 0; i < hand.Length; i++)
                                {
                                    if (i != 4)
                                    {
                                        hand[i] = null;
                                    }
                                }
                                Console.WriteLine("#####################14");
                                pa = new PlayerAction(Name, "Draw", "draw", 4);
                            }
                        }
                    }
                }
                else
                {
                    if (curHandStrength > 2)
                    {
                        if (curHandStrength == 4)
                        {
                            List<int> tempCardList = UnmatchingCards(hand, 3);
                            int deleteIndex = 0;
                            foreach (int i in tempCardList)
                            {
                                if (deleteIndex == 0)
                                {
                                    deleteIndex = i;
                                }
                                else
                                {
                                    if (hand[deleteIndex].Value > hand[i].Value)
                                    {
                                        hand[i] = null;
                                    }
                                    else
                                    {
                                        hand[deleteIndex] = null;
                                    }
                                }
                            }
                            Console.WriteLine("#####################15");
                            pa = new PlayerAction(Name, "Draw", "draw", 1);
                        }
                        else
                        {
                            hand[OddCardOut(hand)] = null;
                        }
                    }
                    else
                    {
                        if (curHandStrength == 2)
                        {
                            DeleteCards(hand, UnmatchingCards(hand, 2));
                            Console.WriteLine("#####################16");
                            pa = new PlayerAction(Name, "Draw", "draw", 3);
                        }
                        else
                        {
                            for (int i = 0; i < hand.Length; i++)
                            {
                                if (i != 4)
                                {
                                    hand[i] = null;
                                }
                            }
                            Console.WriteLine("#####################17");
                            pa = new PlayerAction(Name, "Draw", "draw", 4);
                        }
                    }
                }
            }

            // return the action
            return pa;
        }

        // the behavior tree will analyze which action seems most logical
        // it will then feed that info into the current state which our FSM
        // will process.

        // behavior tree nodes
        private bool Round1FirstCheck(int handStrength, out PlayerAction action)
        {
            action = null;
            // check am I first?
            // if so, step into action selector
            if (roundNum <= 0)
            {
                currentMoney -= ANTE;
                action = Round1ActionSelector(TurnOrder.FIRST, null, handStrength);
                return true;
            }
            
            // otherwise return null to represent that we can't act
            return false;
        }

        private PlayerAction Round1ActionSelector(TurnOrder myOrder, PlayerAction prevAction, int handStrength)
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
                    if (Round1Bet(handStrength, out bettingAmount))
                    {
                        currentAction = Actions.BET;
                        currentMoney -= bettingAmount;
                        return new PlayerAction(Name, "Bet1", "bet", bettingAmount);
                    }
                    if (Round1Fold(handStrength, prevAction))
                    {
                        currentAction = Actions.FOLD;
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    }
                    break;
                case TurnOrder.SECOND: // currently it is the second turn, the actions we can select are call, raise, fold
                    if (Round1PossibleCheck(handStrength, prevAction))
                    {
                        currentAction = Actions.CHECK;
                        return new PlayerAction(Name, "Bet1", "check", 0);
                    }
                    if (Round1PossibleBet(handStrength, prevAction, out bettingAmount))
                    {
                        currentAction = Actions.BET;
                        return new PlayerAction(Name, "Bet1", "bet", bettingAmount);
                    }
                    if (Round1Raise(handStrength, prevAction, out bettingAmount))
                    {
                        currentAction = Actions.RAISE;
                        return new PlayerAction(Name, "Bet1", "raise", bettingAmount);
                    }
                    if (Round1Call(handStrength, prevAction, out bettingAmount))
                    {
                        currentAction = Actions.CALL;
                        return new PlayerAction(Name, "Bet1", "call", bettingAmount);
                    }
                    if (Round1Fold(handStrength, prevAction))
                    {
                        currentAction = Actions.FOLD;
                        return new PlayerAction(Name, "Bet1", "fold", 0);
                    }
                    break;
                default:
                    Console.WriteLine("error: turn order not specified.");
                    return null;
            }

            // otherwise something went wrong and return false
            return null;
        }

        private bool Round1Bet(int handStrength, out int bettingAmount)
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

            // first, check the states in which it can call
            if (prevAction.ActionName == "bet" || prevAction.ActionName == "raise")
            {
                if (prevAction.Amount <= highestWillingBet)
                {
                    //int amountToBet = handStrength * 25;
                    if (handStrength >= 2)
                    {
                        if (prevAction.Amount < currentMoney)
                            callAmount = prevAction.Amount;
                        else
                            callAmount = currentMoney;

                        currentMoney -= callAmount;
                        return true;
                    }

                }
            }
            return false;
        }
        private bool Round1Raise(int handStrength, PlayerAction prevAction, out int raiseAmount)
        {
            raiseAmount = 0;
            // the AI is analyzing whether it should raise for round 1
            //int highestWillingBet = currentMoney / 2;
            if (prevAction.ActionName == "check")
            {
                if (handStrength * 75 < currentMoney)
                    raiseAmount = handStrength * 75;
                else
                    raiseAmount = currentMoney;

                currentMoney -= raiseAmount;
                return true;
            }

            // first, check the states in which it can raise
            if (prevAction.ActionName == "bet" || prevAction.ActionName == "raise")
            {
                int tempRaiseAmount = handStrength * 50;

                if (prevAction.Amount < tempRaiseAmount)
                {
                    if (handStrength > 1)
                    {
                        if (tempRaiseAmount / 2 < currentMoney)
                            raiseAmount = tempRaiseAmount / 2;
                        else
                            raiseAmount = currentMoney;

                        currentMoney -= raiseAmount;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Round1PossibleCheck(int handStrength, PlayerAction prevAction)
        {
            // can I check?
            // if so, step into the checking selector
            if (prevAction.ActionName == "check")
                return Round1Check(handStrength);

            // otherwise return false
            return false;
        }

        private bool Round1PossibleBet(int handStrength,PlayerAction prevAction, out int bettingAmount)
        {
            bettingAmount = 0;

            // can I bet?
            // if so, step into the betting selector
            if (prevAction == null || prevAction.ActionName == "check")
                return Round1Bet(handStrength, out bettingAmount);

            // otherwise return false
            return false;
        }

        private int SimilarSuitedCards(Card[] hand, out List<int> cardIndices)
        {
            int suitCount = 0;
            List<int> retCardIndices = new List<int>();
            for (int j = 0; j < hand.Length; j++)
            {
                int tempSuitCount = 0;
                List<int> tempRetCardIndeces = new List<int>();
                for (int i = 0; i < hand.Length; i++)
                {
                    if (hand[i].Suit == hand[j].Suit)
                    {
                        tempSuitCount++;
                    }
                    else
                    {
                        tempRetCardIndeces.Add(i);
                    }
                }
                if (tempSuitCount > suitCount)
                {
                    suitCount = tempSuitCount;
                    retCardIndices = tempRetCardIndeces;
                }
            }
            cardIndices = retCardIndices;
            return suitCount;
        }

        private int ConsecutiveCards(Card[] hand, out List<int> cardIndices)
        {
            int consecutiveCount = 0;
            List<int> retCardIndices = new List<int>();
            
            if (hand[0].Value == hand[1].Value - 1 &&
               hand[0].Value == hand[2].Value - 2 &&
               hand[0].Value == hand[3].Value - 3)
            {
                consecutiveCount = 4;
                retCardIndices.Add(4);
            }
            else if (hand[1].Value == hand[2].Value - 1 &&
                    hand[1].Value == hand[3].Value - 2 &&
                    hand[1].Value == hand[4].Value - 3)
            {
                consecutiveCount = 4;
                retCardIndices.Add(0);
            }
            else if (hand[0].Value == hand[1].Value - 1 &&
                    hand[0].Value == hand[2].Value - 2 )
            {
                consecutiveCount = 3;
                retCardIndices.Add(3);
                retCardIndices.Add(4);
            }
            else if (hand[1].Value == hand[2].Value - 1 &&
                    hand[1].Value == hand[3].Value - 2)
            {
                consecutiveCount = 3;
                retCardIndices.Add(0);
                retCardIndices.Add(4);
            }
            else if (hand[2].Value == hand[3].Value - 1 &&
                    hand[2].Value == hand[4].Value - 2)
            {
                consecutiveCount = 3;
                retCardIndices.Add(0);
                retCardIndices.Add(1);
            }
            else
            {
                //We don't care if there's less than 3 consecutive, might as well be none (return that)
            }

            cardIndices = retCardIndices;
            return consecutiveCount;
        }

        private List<int> UnmatchingCards(Card[] hand, int numMatchingCards)
        {
            List<int> retCardIndices = new List<int>();
            for (int i = 2; i < 15; i++)
            {
                if (Evaluate.ValueCount(i, hand) == numMatchingCards)
                {
                    for (int j = 0; j < hand.Length; j++)
                    {
                        if (hand[j].Value != i)
                        {
                            retCardIndices.Add(j);
                        }
                    }
                }
            }

            return retCardIndices;
        }

        private void DeleteCards(Card[] hand, List<int> cardIndices)
        {
            foreach (int i in cardIndices)
            {
                hand[i] = null;
            }
        }

        private int OddCardOut(Card[] hand)
        {
            int retCard = 0;

            int firstPair = 0;
            int secondPair = 0;
            for (int i = 2; i < 15; i++)
            {
                if (Evaluate.ValueCount(i, hand) == 2)
                {
                    if (firstPair == 0)
                    {
                        firstPair = i;
                    }
                    else
                    {
                        secondPair = i;
                    }
                }
            }

            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Value != firstPair && hand[i].Value != secondPair)
                {
                    retCard = i;
                }
            }

            return retCard;
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
