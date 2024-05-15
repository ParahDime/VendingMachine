using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FoodMachine
{
    class Program
    {
        

        static void Main(string[] args)
        {

            bool programRun = true; //as long as it is true, the logo will run
            bool logoRun = true; //shows whether the logo will run

            char answerCheck = ' '; //allows the user to check for an answer by changing the char to y or n

            int menuSelection = 0; //used for selecting the different menus
            int loginAttempts = 0; //used for gaining access to the program
            int subMenu = 0; //used to define the in sub menu
            string checkString; //used for passing values

            int pin = 1234;
            int attemptPin = 0000;
            int accessLevel = 0;

            double balance = 0.00;
            double tempBal = 0.00;
            double basketTotal = 0.00;

            List<string> userName = new List<string>();
            List<string> passWord = new List<string>();

            const int maxBasket = 0;
            const int items = 8;
            string[] itemName = new string[items] { "Chocolate", "Cookie", "Crisps", "Chewing Gum", "Soda", "Water", "Lemonade", "Juice"  };//array for item names
            double[] itemPrice = new double[items] { 0.85, 1.30, 1.10, 0.95, 1.10, 0.70, 1.30, 1.00 }; //array for the prices of items
            
            List<string> itemBasket = new List<string>();

            while (loginAttempts < 4) //Used to login to gain access to the system
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("[1] : Login");
                Console.WriteLine("[0] : Exit");
                checkString = Console.ReadLine();
                menuSelection = numberCheck(menuSelection, checkString);
                Console.Clear();

                while (menuSelection == 1) //if they select the login method
                {
                    Console.WriteLine("[1] : Login");
                    Console.WriteLine(" ");

                    while (attemptPin != pin)                                              //used to check the password once it has been entered
                    {
                        //if they fail to input the right password, they will be given X more chances
                        Console.WriteLine("Please enter your pin:");
                        checkString = Console.ReadLine();
                        attemptPin = numberCheck(attemptPin, checkString);

                        if (attemptPin != pin)
                        {
                            //if they fail to input a correct password, they will be given X more attempts (max 4)
                            Console.WriteLine("Incorrect PIN");
                            loginAttempts++;
                            if (loginAttempts < 4) {
                                Console.WriteLine("Please try again");
                            }
                            else //if 4 attempts fail, end the program
                            {
                                return;
                            }
                        }

                    }

                    Console.Clear();
                    do
                    {
                        Console.Clear();

                        subMenu = 0;
                        menuSelection = 5;
                        while (menuSelection == 5)
                        {
                            //load the main menu
                            systemAccess();

                            checkString = Console.ReadLine();
                            menuSelection = numberCheck(subMenu, checkString);
                            Console.Clear();
                        }

                        while (menuSelection == 1) //used for purchasing items available
                        {
                            if (itemBasket.Count() >= items)
                            {
                                Console.WriteLine("Current item basket is full. Press any key to continue");
                                Console.ReadKey();
                                break;
                            }


                            Console.Clear();

                            //list options
                            Console.WriteLine("[1] : View items");
                            Console.WriteLine("[2] : View basket");
                            Console.WriteLine("[3] : Pay");
                            Console.WriteLine("[4] : Clear Basket");
                            Console.WriteLine();
                            Console.WriteLine("[0] : Exit");

                            checkString = Console.ReadLine();
                            subMenu = numberCheck(subMenu, checkString);

                            while (subMenu == 1) //view items
                            {
                                if (itemBasket.Count >= maxBasket)
                                {
                                    Console.WriteLine("Your basket is currently full.");
                                    Console.WriteLine("Please choose the pay option or clear basket");
                                    Console.WriteLine("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                }
                                int itemChoice = 0;
                                //Output the items in the basket
                                itemsAvailable(itemName, itemPrice, itemBasket, balance, basketTotal);

                                //Read line
                                checkString = Console.ReadLine();
                                itemChoice = numberCheck(itemChoice, checkString);
                                //output response based on input

                                //if response is valid
                                if (itemChoice >= 0 && itemChoice <= items)
                                {
                                    //add item to basket
                                    Console.WriteLine("{0} has been added to the list", itemName[itemChoice - 1]); //selects the item (list starts from position 0)
                                    Console.WriteLine("Press enter to continue");
                                    Console.ReadLine();

                                    basketTotal = basketTotal + itemPrice[itemChoice - 1]; //adds the price of the item onto the temporary total cost
                                    checkString = itemName[itemChoice - 1]; //takes the item name and puts it into the checkstring
                                    itemBasket.Add(itemName[itemChoice - 1]); //adds the checkstring value to the list
                                }
                                //if response isn't valid
                                else
                                {
                                    Console.WriteLine("Input was not valid. Press any key to continue.");
                                    Console.ReadKey();
                                }


                                //check if basket is full
                                //if full, exit loop

                            }
                            if (subMenu == 2) //view the basket
                            {
                                //view all items in the basket
                                Console.WriteLine("Items within your basket: ");
                                Console.WriteLine();
                                //use for loop to output all items

                                Console.Write(String.Join(", ", itemBasket)); //prints the items in the basket from the list
                                //console writeline to output amount
                                Console.WriteLine();
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();

                            }
                            if (subMenu == 3) //make a payment
                            {
                                Console.Clear();
                                if (itemBasket.Count >= 1) //if item is in the basket
                                {
                                    Console.Write("Items in basket: ");
                                    Console.Write(String.Join(", ", itemBasket)); //prints the items in the basket from the list

                                    Console.WriteLine();
                                    Console.WriteLine("Total items in basket: {0}", itemBasket.Count); //calculates the amount of items in the basket
                                    Console.WriteLine("Total cost: {0:F2}", basketTotal);
                                    Console.WriteLine();
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();

                                    if (basketTotal > balance) //if total of basket is bigger than the balance
                                    {
                                        Console.WriteLine("You do not have enough credits to purchase these items.");
                                        Console.WriteLine("Amount of credits over: {0:F2}", basketTotal - balance); //shows how many credits over they are
                                        Console.WriteLine("Would you like to add more credits to your account? Y/N");
                                        Console.WriteLine("This will take you to [2] : Add more credits"); //moves the user to [2], but keeps the items in their basket for when they select option 1
                                        checkString = Console.ReadLine();
                                        answerCheck = characterCheck(answerCheck, checkString);

                                        if (answerCheck == 'y') //if they agreed to put more credits into their account
                                        {
                                            menuSelection = 2;
                                            break;
                                        }
                                        else if (answerCheck == 'n') //if they did not agree to put more credits into their account
                                        {
                                            Console.WriteLine("Press enter to return to the main menu");
                                            Console.ReadLine();

                                            Console.Clear();

                                            break;
                                        }
                                        else //if they did not give a valid response
                                        {
                                            Console.WriteLine("Your input was invalid.");
                                        }
                                    }

                                    else if (basketTotal <= balance) //if the cost of the items is less than or equal to the total credit balance
                                    {
                                        Console.WriteLine("Would you like to purchase these items? Y/N");
                                        checkString = Console.ReadLine();
                                        answerCheck = characterCheck(answerCheck, checkString);

                                        if (answerCheck == 'y') //if uses says y to purchasing the items
                                        {
                                            Console.WriteLine(String.Join(" Purchased.", itemBasket));

                                            balance = balance - basketTotal;

                                            Console.WriteLine("You have {0:F2} credits remaining.", balance);
                                            Console.WriteLine("Press enter to return to the main menu");
                                            Console.ReadLine();
                                            itemBasket.Clear(); //fix clearing the list
                                            basketTotal = 0.00;

                                            Console.Clear();
                                            menuSelection = 5;
                                        }
                                        else if (answerCheck == 'n') //if user says n to purchasing the items
                                        {
                                            Console.WriteLine("Items were not bought");
                                            Console.WriteLine("Items have been removed from the basket");
                                            Console.WriteLine("Press enter to return to the item menu");
                                            Console.ReadLine();

                                            itemBasket.Clear();
                                            basketTotal = 0.00;
                                            break;
                                        }
                                        else //if user does not give a proper answer to purchasing the items
                                        {
                                            Console.WriteLine("{0} is not a valid option", checkString);
                                            Console.ReadLine();
                                            //user will still keep the items they have inputted, however will go to the item menu
                                        }
                                    }

                                }
                                else //if no items are in the basket
                                {

                                    Console.WriteLine("No items are in the basket. Please make a selection");
                                    Console.ReadKey();


                                }
                            }
                            if (subMenu == 4) //if basket is to be emptied
                            {
                                itemBasket.Clear();
                                Console.Write("Basket has been cleared. Press any key to continue.");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                break;
                            }
                            subMenu = 0;
                        }
                        while (menuSelection == 2) //Used for dealing with account funds
                        {
                            Console.WriteLine($"Available balance:  £{balance:F2}");
                            Console.WriteLine();
                            Console.WriteLine("[1] : Add funds");
                            Console.WriteLine("[0] : Exit");

                            checkString = Console.ReadLine();
                            subMenu = numberCheck(subMenu, checkString);

                            if (subMenu == 1)
                            {
                                double inputValue = 0.00;
                                //if they have chosen to add funds
                                Console.WriteLine($"Available balance: £{balance:F2}");
                                Console.WriteLine();
                                Console.WriteLine("Input an amount you would like to add");
                                Console.WriteLine("A maximum amount of £10 can be added. Anything else will  be returned");
                                checkString = Console.ReadLine();
                                //parse the value and read it
                                tempBal = balUsage(checkString);
                                //check the amount
                                if (tempBal > 0.00)
                                {
                                    Console.WriteLine("Do you wish to add £{0:F2} credits to your account? Y/N", tempBal);
                                    Console.WriteLine("Funds currently in account: £{0:F2}", balance);
                                    Console.WriteLine("Amount after adding:£{0:F2}", balance + tempBal);
                                    checkString = Console.ReadLine();
                                    answerCheck = characterCheck(answerCheck, checkString);

                                    if (answerCheck == 'y')
                                    {
                                        balance = balance + tempBal;
                                        Console.WriteLine("Your credit balanced has been updated. New Total, £{0:F2}", balance);
                                        Console.WriteLine("Press enter to return to the main menu");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    }
                                    else if (answerCheck == 'n')
                                    {
                                        Console.WriteLine("Your funds were not updated. Total credits: £{0:F2}", balance);
                                        Console.WriteLine("Press enter to return to the main menu");
                                        Console.ReadLine();
                                        Console.Clear();
                                        menuSelection = 5;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Unknown Input. Press Enter to head back to the menu");
                                        Console.ReadLine();

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Input was not valid.");
                                    Console.WriteLine("Press any key to continue");
                                    Console.ReadKey();
                                }
                                //if amount is under 0


                                Console.ReadLine();
                            }
                            else
                            {
                                //value is treated as 0
                                break;
                            }
                        }
                        while (menuSelection == 3) //Used for changing values of the system
                        {
                            Console.Clear();
                            Console.WriteLine("[4] : Access System Values\n");
                           
                            if(accessLevel > 1) //if admin and above
                            {
                                Console.WriteLine("Select mode");
                                // add, edit, remove
                                Console.WriteLine("[1] : Add items");
                                Console.WriteLine("[2] : Edit items");
                                Console.WriteLine("[3] : Remove items");
                                Console.WriteLine("[0] : Exit");

                                
                            }
                            else //if regular user
                            {
                                Console.WriteLine("ERROR! \nYou do not have the correct permissions.");
                                Console.WriteLine("\n\n Press any key to return to main menu.");
                                Console.ReadKey();
                                break;
                            }

                            menuSelection = 5;
                        }
                        while (menuSelection == 4) //Used for changing the values/adding items
                        {
                            Console.Clear();
                            Console.WriteLine("[5] : Access System Items\n");
                            Console.WriteLine("[1] : Edit PIN");
                            Console.WriteLine("[2] : Add users\n");
                            Console.WriteLine("[0] : Exit");

                            int adminMenu = 0;
                            //exit
                            do
                            {
                                break;
                            } while (adminMenu == 0);

                            do //edit PIN
                            {
                                //output PIN

                                //ask if user wants to change

                                //if yes, user inputs 4 digit PIN
                                //check if PIN in use

                                //if yes, loop, error
                                //if no, ask verif
                                //if verif, accept, break
                                //if no, do not accept, break

                                //if no, break
                                break;
                            } while (adminMenu == 1);
                            //submenu 2
                            do
                            {
                                //if have validation
                                if (accessLevel > 1)
                                {
                                    Console.WriteLine("This requires login. Please use a staff username and password");
                                    //give ability for user to add
                                    //provide name
                                    //access Level must be 1 less than them
                                    //balance is auto 0.00

                                    //provide output, check to confirm

                                    //if yes push and update data
                                    //if no clear vars, press any key to return

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("ERROR! \nYou do not have the correct permissions.");
                                    Console.WriteLine("\n\n Press any key to return to main menu.");
                                    Console.ReadKey();
                                    break;
                                }

                                
                                //if not
                            } while (adminMenu == 2);
                            
                            Console.ReadKey();
                            menuSelection = 5;
                        }
                        while (menuSelection == 0) //used for exiting the program
                        {
                            checkString = " ";

                            Console.Clear();
                            while (menuSelection == 0 && (answerCheck != 'y' || answerCheck != 'n'))
                            {
                                //will continuously run until either the menuselection is changed or the character is y or n
                                Console.WriteLine(" Do you want to exit the program? Y/N");
                                checkString = Console.ReadLine();
                                answerCheck = characterCheck(answerCheck, checkString);

                                if (answerCheck == 'y') //if they typed y, it will close the program
                                {
                                    programRun = false;
                                    return;
                                }
                                else if (answerCheck == 'n') //if they type no, they go back to the main menu
                                {
                                    Console.WriteLine("Press enter to return to the main menu");
                                    Console.ReadLine();
                                    menuSelection = 5;
                                }
                                else if (answerCheck != 'y' && answerCheck != 'n') //if the answer was not accepted
                                {
                                    Console.WriteLine("Invalid Input. Please try again");

                                }

                                checkString = " ";
                            }
                        }



                    } while (programRun == true);
                }
                if(menuSelection == 0) //if the user chooses to exit the program
                {
                    loginAttempts = 4;
                    return;
                }

                if(menuSelection != 1 && menuSelection != 2) //if neither option is correct, the user will be told
                {
                    Console.WriteLine("The option you entered was not available. Please try again");
                    Console.WriteLine(" ");
                }

            }
            while ((programRun == true))
            {
                
            }
        }

        static void systemAccess()
        {
            menuLogo();
            mainMenu();
            return;
        }
        static void mainMenu() //will print the main menu
        {
            Console.WriteLine(" ");
            Console.WriteLine(" Please Select an option:");
            Console.WriteLine("[1] : Select items to purchase");
            Console.WriteLine("[2] : Manage balance");
            Console.WriteLine("[3] : Change account settings");
            Console.WriteLine("[4] : Change system settings");
            Console.WriteLine("");
            Console.WriteLine("[0] : Exit");
        }

        static void menuLogo() //will print the menu logo
        {
            Console.WriteLine("================================================================================================================================================================================");
            Console.WriteLine("================================================================================================================================================================================");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(",ggggggggggg,                                           ,gggggggggggg,                                                 ");
            Console.WriteLine("dP\"\"\"88\"\"\"\"\"Y8,                             , dPYb, dP\"\"\"88\"\"\"\"\"\"Y8b,                                                ");
            Console.WriteLine("Yb,  88      `8b                             IP'`Yb     Yb,  88       `8b,   ");
            Console.WriteLine(" `\"  88,      8P                             I8  8I      `\"  88        `8b gg  ");
            Console.WriteLine("     88aaaad8P\"                              I8  8'          88         Y8 \"\"        ");
            Console.WriteLine("     88\"\"\"\"\",gggg, gg, gggggg,    , gggg, gg I8 dPgg,        88         d8 gg,    ggg,,ggg,,ggg,    ,ggg,     ,g, ");
            Console.WriteLine("     88    dP\"  \"Y8I   dP\"\"\"\"8I   dP\"  \"Y8I  I8dP\" \"8I       88        ,8P 88   ,8\" \"8P\" \"8P\" \"8,  i8\" \"8i   ,8'8,  ");
            Console.WriteLine("     88   i8'    ,8I  ,8'    8I  i8'    ,8I  I8P    I8       88       ,8P' 88   I8   8I   8I   8I  I8, ,8I  ,8'  Yb   ");
            Console.WriteLine("     88  ,d8,   ,d8b,,dP     Y8,,d8,   ,d8b,,d8     I8,      88______,dP'_,88,_,dP   8I   8I   Yb, `YbadP' ,8'_   8) ");
            Console.WriteLine("     88  P\"Y8888P\"`Y88P      `Y8P\"Y8888P\"`Y888P     `Y8     888888888P\"  8P\"\"Y88P'   8I   8I   `Y8888P\"Y888P' \"YY8P8P         ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ,ggg,         ,gg                                                                  ,ggg, ,ggg,_,ggg,");
            Console.WriteLine("dP\"\"Y8a       ,8P                              8I                                  dP\"\"Y8dP\"\"Y88P\"\"Y8b                        ,dPYb, ");
            Console.WriteLine("Yb, `88       d8'                              8I                                  Yb, `88'  `88'  `88                        IP'`Yb ");
            Console.WriteLine(" `\"  88       88                               8I   gg                              `\"  88    88    88                        I8  8I      gg  ");
            Console.WriteLine("     88       88                               8I   \"\"                                  88    88    88                        I8  8'      \"\" ");
            Console.WriteLine("     I8       8I ,ggg,    ,ggg,,ggg,     ,gggg,8I   gg    ,ggg,,ggg,     ,gggg,gg       88    88    88    ,gggg,gg    ,gggg,  I8 dPgg,    gg    ,ggg,,ggg,    ,ggg,     ,g, ");
            Console.WriteLine("     `8,     ,8'i8\" \"8i  ,8\" \"8P\" \"8,   dP\"  \"Y8I   88   ,8\" \"8P\" \"8,   dP\"  \"Y8I       88    88    88   dP\"  \"Y8I   dP\"  \"Yb I8dP\" \"8I   88   ,8\" \"8P\" \"8,  i8\" \"8i   ,8'8, ");
            Console.WriteLine("      Y8,   ,8P I8, ,8I  I8   8I   8I  i8'    ,8I   88   I8   8I   8I  i8'    ,8I       88    88    88  i8'    ,8I  i8'       I8P    I8   88   I8   8I   8I  I8, ,8I  ,8'  Yb  ");
            Console.WriteLine("       Yb,_,dP  `YbadP' ,dP   8I   Yb,,d8,   ,d8b,_,88,_,dP   8I   Yb,,d8,   ,d8I       88    88    Y8,,d8,   ,d8b,,d8,_    _,d8     I8,_,88,_,dP   8I   Yb, `YbadP' ,8'_   8)");
            Console.WriteLine("        \"Y8P\"  888P\"Y8888P'   8I   `Y8P\"Y8888P\"`Y88P\"\"Y88P'   8I   `Y8P\"Y8888P\"888      88    88    `Y8P\"Y8888P\"`Y8P\"\"Y8888PP88P     `Y88P\"\"Y88P'   8I   `Y8888P\"Y888P' \"YY8P8P");
            Console.WriteLine("                                                                            ,d8I'");
            Console.WriteLine("                                                                          ,dP'8I");
            Console.WriteLine("                                                                          ,8\"  8I ");
            Console.WriteLine("                                                                          I8   8I   ");
            Console.WriteLine("                                                                          `8, ,8I  ");
            Console.WriteLine("                                                                           `Y8P\"");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("================================================================================================================================================================================");
            Console.WriteLine("================================================================================================================================================================================");
            Console.WriteLine(" ");
        }

        static double balUsage(string checkString) //will work with the user balance
        {
            double accTotal = 0.00;

            double.TryParse(checkString, out accTotal);

            accTotal = Math.Round(accTotal, 2);

            if (accTotal > 10)
            {
                Console.WriteLine("The total you are attempting to add your account is above the limit");
                Console.ReadLine();
                accTotal = 0.00;
                return accTotal;
            }
            else if (accTotal == 0.00)
            {
                Console.WriteLine("The amount you input was invalid. Please specify an amount.");
                Console.ReadLine();
                return accTotal;
            }
            else
            {
                return accTotal;
            }
        }

        static char characterCheck(char answerCheck, string checkString)//parses a string 
        {
            checkString = checkString.ToLower(); //converts the value to lower case

            char.TryParse(checkString, out answerCheck); //passes the value into a character (if able)

            if (!(char.TryParse(checkString, out answerCheck))) //if it did not, passes the first character in the string into the character
            {
                answerCheck = checkString[0];
            }

            if (answerCheck == 'y') //if the value is y
            {
                return answerCheck;
            }
            else if (answerCheck == 'n') //if the value is n
            {
                return answerCheck;
            }
            else //if the value is neither of the two
            {
                answerCheck = ' ';
                return answerCheck;
            }
        }
        static int numberCheck(int menuSelection, string checkString) //checks for a number
        {
            int.TryParse(checkString, out menuSelection);

            if (!(int.TryParse(checkString, out menuSelection)))
            {
                checkString.ToLower();

                if(checkString == "zero")
                {
                    menuSelection = 0;
                }
                else if(checkString == "one")
                {
                    menuSelection = 1;
                }
                else if(checkString == "two")
                {
                    menuSelection = 2;
                }
                else if(checkString == "three")
                {
                    menuSelection = 3;
                }
                else if(checkString == "four")
                {
                    menuSelection = 4;
                }
                else if(checkString == "five")
                {
                    menuSelection = 5;
                }
                else if (checkString == "six")
                {
                    menuSelection = 6;
                }
                else if(checkString == "seven")
                {
                    menuSelection = 7;
                }
                else
                {
                    menuSelection = 7;//placeholder value for the main menu
                }

            }

            if(menuSelection >= 0 || menuSelection <= 7)
            {
                return menuSelection;
            }
            else if (menuSelection < 0 || menuSelection > 7)
            {
                Console.WriteLine("The value you entered was not a valid option. Please try again");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("The value entered was invalid. Please try again");
                Console.ReadLine();

            }

            menuSelection = 7;
            return menuSelection;
        }

        //displays items available
        static void itemsAvailable(string[] itemName, double[] itemPrice, List<string> itemBasket, double itemCredits, double creditBal) //shows the items available to buy
        {
            Console.WriteLine("> Select items to buy <");
            Console.WriteLine(" ");

            for (int i = 1; i < 6; i++) //loops the items in the string to show on the console
            {
                Console.WriteLine("[{0}] : {1}   Price:£{2:F2} ", i, itemName[i - 1], itemPrice[i - 1]);
            }
            Console.WriteLine(" ");
            if (itemBasket.Count >= 1)
            {
                Console.WriteLine("[6] : Pay for items");
            }
            Console.WriteLine("[0] : Return to menu.");
            Console.WriteLine(" ");
            Console.Write("Items in basket:");

            Console.Write(String.Join(", ", itemBasket));


            Console.WriteLine(" ");
            Console.WriteLine("Items in basket: {0}/5", itemBasket.Count);

            Console.WriteLine("Current total: {0:F2}", itemCredits); //prints the total cost of the items
            Console.WriteLine("Current balance:{0:F2}", creditBal); //shows the amount in the balance
        }

        //displays items in the basket
        static void itemsInBasket()
        {

        }

    }
}
