﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player6 : Player
    {
        int enemyFirstBet;
        int roundNum;
        int startingMoney;
        int lastActionStr;
        int lastActionAmount;

        public Player6(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
            enemyFirstBet = 0;
            roundNum = 0;
            startingMoney = mny;
        }

        //Probabilities: http://www.math.hawaii.edu/~ramsey/Probability/PokerHands.html

        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            //info to keep track of
            bool isFirst = false;
            
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

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
            string actionSelection = "1";
            PlayerAction pa = null;
            int amount = 20;

            //start turn
            Console.WriteLine("\n-> in ai betting round 1");
            Console.WriteLine("   Total games played: "+roundNum);

            //if ai is first
            if (isFirst)
            {

            }else
            {

            }



            //end turn and submit action
            //create the PlayerAction
            switch (actionSelection)
            {
                case "1": pa = new PlayerAction(Name, "Bet1", "bet", amount); break;
                case "2": pa = new PlayerAction(Name, "Bet1", "raise", amount); break;
                case "3": pa = new PlayerAction(Name, "Bet1", "call", amount); break;
                case "4": pa = new PlayerAction(Name, "Bet1", "check", amount); break;
                case "5": pa = new PlayerAction(Name, "Bet1", "fold", amount); break;
                default: Console.WriteLine("Invalid menu selection - try again"); break;
            } Console.WriteLine("< end ai betting round 1 >");
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
    }
}
