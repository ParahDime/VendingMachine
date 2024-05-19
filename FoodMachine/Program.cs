using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;




namespace FoodMachine
{
    class Program
    {


        static void Main(string[] args)
        {

            bool programRun = true; //as long as it is true, the logo will run
            bool logRun = false; //if user is logged in, turns to true

            char answerCheck = ' '; //allows the user to check for an answer by changing the char to y or n
            string checkString = ""; //used for passing values

            int menuSelection = 0; //used for selecting the different menus
            int loginAttempts = 0; //used for gaining access to the program
            int subMenu = 0; //used to define the in sub menu

            int selectedUser = 0; //user selected from the list
            double tempBal = 0.00; //temporary balance 
            double basketTotal = 0.00; //total of items in basket

            List<Account> users = new List<Account>(); //list of users
            List<VendingItem> items = new List<VendingItem>(); //list of purchasable items

            string fileName; //name of file
            string filePath = ""; //path to file

            const int maxBasket = 5;

            //#########################################
            //          Read Files
            //#########################################
            List<string> itemBasket = new List<string>();

            fileName = "TextFile1.txt";
            getFilePath(ref filePath, fileName);
            //check data can be read (userdata)
            if (checkRead(filePath))
            {        
                ReadItems(filePath, ref users);
            }
            else
            {
                Console.Write("ERROR. Press any key to exit the program");
                Console.ReadKey();
                return;
            }

            fileName = "data.txt";
            getFilePath(ref filePath, fileName);
            //check data can be read (items)
            if(checkRead(filePath))
            {
                ReadData(filePath, ref items);
            }
            else
            {
                Console.Write("ERROR. Press any key to exit the program");
                Console.ReadKey();
                return;
            }
            Console.Clear();

            //#########################################
            //          Login
            //#########################################

            while (loginAttempts < 4) //Used to login to gain access to the system
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("[1] : Login");
                Console.WriteLine("[0] : Exit");
                checkString = Console.ReadLine();

                menuSelection = numberCheck(menuSelection, checkString);
                Console.Clear();

                while (logRun == false) //if they select the login method
                {
                    Console.WriteLine("[1] : Login");
                    Console.WriteLine(" ");

                    while (logRun == false) //used to check the password once it has been entered
                    {
                        //if they fail to input the right password, they will be given X more chances
                        Console.WriteLine("Please enter your pin:");
                        checkString = Console.ReadLine();

                        //for loop to check if it matches any users
                        for (int i = 0; i < users.Count(); i++)
                        {
                            if (users[i].PIN == checkString)
                            {
                                logRun = true;
                                selectedUser = i;
                                break;
                            }                            
                        }

                        if (logRun == false)
                        {
                            //if they fail to input a correct password, they will be given X more attempts (max 4)
                            Console.WriteLine("Incorrect PIN");
                            loginAttempts++;
                            if (loginAttempts < 3) {
                                Console.WriteLine("Please try again");
                            }
                            else //if 4 attempts fail, end the program
                            {
                                Console.Write("Could not identify user. Press any key to terminate program");
                                Console.ReadKey();
                                return;
                            }
                        }
                    }

                    //#########################################
                    //          Authorised Login
                    //#########################################

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
                            if (itemBasket.Count() >= items.Count())
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
                                itemsAvailable(ref items, itemBasket, users[selectedUser].balance, basketTotal);

                                //Read line
                                checkString = Console.ReadLine();
                                itemChoice = numberCheck(itemChoice, checkString);
                                //output response based on input

                                //if response is valid
                                if (itemChoice == items.Count() + 1)
                                {
                                    subMenu = 3;
                                    break;
                                }
                                else if (itemChoice > 0 && itemChoice <= items.Count() + 1)
                                {
                                    //add item to basket
                                    Console.WriteLine("{0} has been added to the list", items[itemChoice - 1].name); //selects the item (list starts from position 0)
                                    Console.WriteLine("Press enter to continue");
                                    Console.ReadLine();

                                    basketTotal = basketTotal + items[itemChoice - 1].price; //adds the price of the// item onto the temporary total cost
                                    checkString = items[itemChoice - 1].name; //takes the item name and puts it into the checkstring
                                    itemBasket.Add(items[itemChoice - 1].name); //adds the checkstring value to the list
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

                                    if (basketTotal > users[selectedUser].balance) //if total of basket is bigger than the balance
                                    {
                                        Console.WriteLine("You do not have enough credits to purchase these items.");
                                        Console.WriteLine("Amount of credits over: {0:F2}", basketTotal - users[selectedUser].balance); //shows how many credits over they are
                                        Console.WriteLine("Would you like to add more credits to your account? Y/N");
                                        Console.WriteLine("This will take you to [2] : Add more credits"); //moves the user to [2], but keeps the items in their basket for when they select option 1
                                        checkString = Console.ReadLine();
                                        characterCheck(ref answerCheck, checkString);

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

                                    else if (basketTotal <= users[selectedUser].balance) //if the cost of the items is less than or equal to the total credit balance
                                    {
                                        Console.WriteLine("Would you like to purchase these items? Y/N");
                                        checkString = Console.ReadLine();
                                        characterCheck(ref answerCheck, checkString);

                                        if (answerCheck == 'y') //if uses says y to purchasing the items
                                        {
                                            Console.WriteLine(String.Join(" Purchased.", itemBasket));

                                            users[selectedUser].balance = users[selectedUser].balance - basketTotal;

                                            Console.WriteLine("You have {0:F2} credits remaining.", users[selectedUser].balance);
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
                            Console.WriteLine($"Available balance:  £{users[selectedUser].balance:F2}");
                            Console.WriteLine();
                            Console.WriteLine("[1] : Add funds");
                            Console.WriteLine("[0] : Exit");

                            checkString = Console.ReadLine();
                            subMenu = numberCheck(subMenu, checkString);

                            if (subMenu == 1)
                            {                             
                                //if they have chosen to add funds
                                Console.WriteLine($"Available balance: £{users[selectedUser].balance:F2}");
                                Console.WriteLine();
                                Console.WriteLine("Input an amount you would like to add");
                                Console.WriteLine("A maximum amount of £10 can be added. Anything else will be returned");
                                checkString = Console.ReadLine();
                                //parse the value and read it
                                tempBal = balUsage(checkString);
                                //check the amount
                                if (tempBal > 0.00)
                                {
                                    Console.WriteLine("Do you wish to add £{0:F2} credits to your account? Y/N", tempBal);
                                    Console.WriteLine("Funds currently in account: £{0:F2}", users[selectedUser].balance);
                                    Console.WriteLine("Amount after adding: £{0:F2}", users[selectedUser].balance + tempBal);
                                    checkString = Console.ReadLine();
                                    characterCheck(ref answerCheck, checkString);

                                    if (answerCheck == 'y')
                                    {
                                        users[selectedUser].balance = users[selectedUser].balance + tempBal;
                                        Console.WriteLine("Your credit balanced has been updated. New Total, £{0:F2}", users[selectedUser].balance);
                                        Console.WriteLine("Press enter to return to the main menu");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    }
                                    else if (answerCheck == 'n')
                                    {
                                        Console.WriteLine("Your funds were not updated. Total credits: £{0:F2}", users[selectedUser].balance);
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
                            Console.WriteLine("[3] : Access System Values\n");

                            if (users[selectedUser].accessLevel > 1) //if admin and above
                            {
                                Console.WriteLine("Select mode");
                                // add, edit, remove
                                Console.WriteLine("[1] : Add items");
                                Console.WriteLine("[2] : Edit items");
                                Console.WriteLine("[3] : Remove items\n");
                                Console.WriteLine("[0] : Exit");

                                checkString = Console.ReadLine();
                                subMenu = numberCheck(subMenu, checkString);


                                if (subMenu == 0)
                                {
                                    menuSelection = 5;
                                    break;
                                }

                                int numUsed = -1;
                                while(subMenu == 1) //add items
                                {
                                    
                                    //input variables
                                    Console.WriteLine("Input item name");

                                    Console.WriteLine("Input price for item");

                                    //output the combined vars

                                    //ask for verif

                                    //if yes, add item to file

                                    //else ignore, back to submenu
                                    break;

                                }
                                while(subMenu == 2) //edit items
                                {
                                    ItemPicker("modify", ref items, ref checkString, ref numUsed, ref filePath);                            
                                    break;
                                }
                                while(subMenu == 3) //remove items
                                {
                                    //list items by name

                                    ItemPicker("remove", ref items, ref checkString, ref numUsed, ref filePath);
                                }
                            }
                            else //if regular user
                            {
                                Console.WriteLine("ERROR! \nYou do not have the correct permissions.");
                                Console.WriteLine("\n\n Press any key to return to main menu.");
                                Console.ReadKey();
                                menuSelection = 5;
                                break;
                            }
                        }
                        while (menuSelection == 4) //Used for changing user info
                        {
                            Console.Clear();
                            Console.WriteLine("[4] : Access User Data\n");
                            Console.WriteLine("[1] : Edit PIN");
                            Console.WriteLine("[2] : Add users\n");
                            Console.WriteLine("[0] : Exit");

                            //verif
                            checkString = Console.ReadLine();
                            subMenu = numberCheck(subMenu, checkString);

                            //exit
                            while (subMenu == 0)
                            {
                                menuSelection = 5;
                                break;
                            }

                            while (subMenu == 1) //edit PIN
                            {
                                Console.Clear();

                                if (pinVerif(ref loginAttempts, users, selectedUser, checkString, ref subMenu))
                                {
                                    Console.WriteLine("PIN has been verified\n");
                                    //ask if user wants to change
                                    Console.WriteLine("Would you like to change your PIN? y/n");

                                    checkString = Console.ReadLine();
                                    characterCheck(ref answerCheck, checkString);

                                    if (answerCheck == 'y')
                                    {
                                        bool unique = true;
                                        bool pinChange = false;
                                        Console.Clear();
                                        while (pinChange == false)
                                        {
                                            Console.WriteLine("Please enter your new PIN. Your PIN must be unique");
                                            Console.WriteLine("Press 0 to return to the menu");

                                            //if yes, user inputs a PIN
                                            checkString = Console.ReadLine();
                                            //check if PIN in use
                                            for (int i = 0; i < users.Count(); i++)
                                            {
                                                if (users[i].PIN == checkString)
                                                {
                                                    unique = false;
                                                    break;
                                                }

                                            }
                                            //if yes, loop, error
                                            if (unique && pinChange == true)
                                            {
                                                //open file
                                                //edit file
                                                //at location with name
                                                //close file


                                                Console.WriteLine("SUCCESS! Pin has been changed.");
                                                Console.WriteLine("Press any key to continue");
                                                Console.ReadKey();
                                                subMenu = 0;
                                                pinChange = true;
                                                break;
                                            }
                                            else if (!unique && pinChange == false)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Your PIN could not be changed to {0} due to security concerns. Please try again");
                                            }

                                            if (checkString == "0")
                                            {
                                                subMenu = 0;
                                                pinChange = false;
                                                break;
                                            }
                                        }
                                    }
                                    else if (answerCheck == 'n')
                                    {
                                        //if verif, accept, break
                                        Console.WriteLine("PIN has not been changed");
                                        Console.WriteLine("Press any key to continue");
                                        Console.ReadKey();
                                        subMenu = 0;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Response\nPress any key to continue");
                                        Console.ReadKey();
                                        subMenu = 0;
                                        break;
                                    }
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            while (subMenu == 2) //adding users
                            {
                                //if have validation
                                if (users[selectedUser].accessLevel > 1)
                                {
                                    Console.WriteLine("This requires login. Please use a staff username and password");

                                    while (pinVerif(ref loginAttempts, users, selectedUser, checkString, ref subMenu))
                                    {
                                        Console.WriteLine("PIN has been verified\n");

                                        //ask user if they want to use a file
                                        Console.WriteLine("Would you like to import the data from a file? y/n");

                                        if (characterCheck(ref answerCheck, checkString) == 'y')
                                        {
                                            //ask user which file

                                            //provide input for the file name

                                            //check the file is valid

                                            //if yes, read the data into the program, then update both list and og data file

                                            //if not valid, let user know
                                        }
                                        else
                                        {
                                            //prompt user for information line by line

                                            //name

                                            //default pin

                                            //security level is always 1
                                            //bal is always empty

                                            //ask if params are correct
                                            //if yes, add as above

                                            //if not, loop
                                        }
                                    }


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
                            } 
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
                                 characterCheck(ref answerCheck, checkString);

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
                if (menuSelection == 0) //if the user chooses to exit the program
                {
                    loginAttempts = 4;
                    return;
                }

                if (menuSelection != 1 && menuSelection != 2) //if neither option is correct, the user will be told
                {
                    Console.WriteLine("The option you entered was not available. Please try again");
                    Console.WriteLine(" ");
                }

            }
            while ((programRun == true))
            {

            }
        }

        //get the file path of the item searched
        static string getFilePath(ref string filePath, string fileName)
        {
            //get the directory of the folder
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string projectDirectory = Directory.GetParent(Directory.GetParent(baseDirectory).FullName).FullName;
            projectDirectory = Directory.GetParent(Directory.GetParent(projectDirectory).FullName).FullName;

            return filePath = Path.Combine(projectDirectory, "FoodMachine", fileName);
        }

        //checks a file is readable
        static bool checkRead(string name)
        {
            //if can be opened, debug text yes
            

            if(File.Exists(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
        //reads account into from a file
        static void ReadItems(string filePath, ref List<Account> users)
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Loop through each line
                foreach (string line in lines)
                {
                    // Split the line by ws
                    string[] parts = line.Split(' ');

                    //Check there are 4 parts
                    if (parts.Length == 4)
                    {
                        //Parse the values if needed
                        string name = parts[0];                 
                        string id = parts[1];
                        int someInteger = int.Parse(parts[2]);
                        double someFloat = double.Parse(parts[3]);

                        //Create a new person object
                        Account person = new Account(name, id, someInteger, someFloat);

                        // Add the person to the list of users
                        users.Add(person);
                    }
                    else
                    {
                        //display an error message, wait for feedback to ensure read
                        Console.WriteLine($"Invalid line format: {line}");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ReadKey();
            }
        }
        //reads info to populate vending machine
        static void ReadData(string filePath, ref List<VendingItem> items)
        {
            try
            {
                //Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);
                

                foreach (string line in lines)
                {
                    //split line by ws
                    string[] parts = line.Split(' ');
                    

                    if (parts.Length == 2)
                    {
                        //ensure that there are 2 parts
                        string name = parts[0];
                        double price = double.Parse(parts[1]);

                        VendingItem item = new VendingItem(name, price);
                        items.Add(item);
                    }
                    else
                    {
                        //display an error message, wait for feedback to ensure read
                        Console.WriteLine($"Invalid line format: {line}");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                //display an error message, wait for feedback to ensure read
                Console.WriteLine($"An error  occurred: {ex.Message}");
                Console.ReadKey();
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

        static char characterCheck(ref char answerCheck, string checkString)//parses a string 
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

                if (checkString == "zero")
                {
                    menuSelection = 0;
                }
                else if (checkString == "one")
                {
                    menuSelection = 1;
                }
                else if (checkString == "two")
                {
                    menuSelection = 2;
                }
                else if (checkString == "three")
                {
                    menuSelection = 3;
                }
                else if (checkString == "four")
                {
                    menuSelection = 4;
                }
                else if (checkString == "five")
                {
                    menuSelection = 5;
                }
                else if (checkString == "six")
                {
                    menuSelection = 6;
                }
                else if (checkString == "seven")
                {
                    menuSelection = 7;
                }
                else
                {
                    menuSelection = 7;//placeholder value for the main menu
                }

            }

            if (menuSelection >= 0 || menuSelection <= 7)
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
        static void itemsAvailable(ref List<VendingItem> items, List<string> itemBasket, double itemCredits, double creditBal) //shows the items available to buy
        {
            Console.WriteLine("> Select items to buy <");
            Console.WriteLine(" ");

            for (int i = 1; i < items.Count() + 1; i++) //loops the items in the string to show on the console
            {
                Console.WriteLine("[{0}] : {1}   Price:£{2:F2} ", i, items[i - 1].name, items[i - 1].price);
            }
            Console.WriteLine(" ");
            if (itemBasket.Count >= 1)
            {
                Console.WriteLine("[{0}] : Pay for items", items.Count() + 1);
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

        //checks the users pin verification
        static bool pinVerif(ref int loginAttempts, List<Account> users, int selectedUser, string checkString, ref int subMenu)
        {
            loginAttempts = 0;
            //user inputs their pin
            while (loginAttempts < 3 && users[selectedUser].PIN != checkString) //loop, verify
            {
                Console.WriteLine("For security reasons, please enter your PIN to verify:");
                checkString = Console.ReadLine();
                if (users[selectedUser].PIN == checkString) //if correct
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Input was not correct. Please try again");
                    loginAttempts++;
                }

                if (loginAttempts == 3)
                {
                    Console.WriteLine("User could not verify their PIN. Press any key to continue");
                    Console.ReadKey();
                    subMenu = 0;
                    return false;
                }
            }
            return false;
        }

        //selects which item to pick from the list (selection 3)
        static void ItemPicker(string verb, ref List <VendingItem> items, ref string checkString, ref int numUsed, ref string filePath)
        {
            char ans = ' ';
            for(int i = 0; i < items.Count(); i++) //output list of items
            {
                Console.WriteLine("{0}: {1}", i, items[i].name);
            }
            Console.WriteLine("Select item to {0}", verb);
            checkString = Console.ReadLine(); //check if item is within the items listed

            if (int.TryParse(checkString, out numUsed)) //attempt to parse the value of checkstring into a number
            {
                if(numUsed >= 0 && numUsed < items.Count()) //if a valid value
                {
                    //if yes, ask for pin confirmation
                    Console.WriteLine("Item selected: {0}: {1}", items[numUsed].name, items[numUsed].price);
                    Console.WriteLine("\nIs this correct? y/n");
                    checkString = Console.ReadLine();
                    characterCheck(ref ans, checkString);
                    if(ans == 'y')
                    {
                        if(verb == "modify") //modify the item
                        {
                            int itemEdit = 0;
                            Console.Clear();
                            Console.WriteLine("Select item to modify:"); //option to modify name or price
                            Console.WriteLine("[1]: name");
                            Console.WriteLine("[1]: price");
                            Console.WriteLine("[0]: exit");
                            checkString = Console.ReadLine();
                            itemEdit = numberCheck(itemEdit, checkString);

                            if(itemEdit == 1) //edit name
                            {
                                Console.WriteLine("Enter the new name for {0}", items[numUsed].name);
                                string newName = Console.ReadLine();
                                
                                Console.WriteLine("{0}: old name: {1}", numUsed, items[numUsed].name);
                                Console.WriteLine("{0}: new name: {1}", numUsed, newName);
                                //validate
                                Console.WriteLine("\nIs this correct? y/n");
                                checkString = Console.ReadLine();
                                characterCheck(ref ans, checkString);

                                if(ans == 'y')//yes
                                {
                                    items[numUsed].name = newName;//update the list

                                    //update the file
                                }
                                else if (ans == 'n')//no
                                {
                                    Console.WriteLine("Values were not changed. Press any key to exit");
                                    Console.ReadKey();
                                    Console.Clear();
                                    numUsed = -1;
                                    return;
                                }
                                else//invalid
                                {
                                    Console.WriteLine("Invalid input. Press any key to exit");
                                    Console.ReadKey();
                                    Console.Clear();
                                    numUsed = -1;
                                    return;
                                }
                            }
                            else if(itemEdit == 2) //edit price
                            {
                                Console.WriteLine("Enter the new price for {0}", items[numUsed].name);
                                checkString = Console.ReadLine();
                                double newPrice;

                                try //parse the value
                                {
                                    double.TryParse(checkString, out newPrice);
                                }
                                catch (Exception ex) //catch exception
                                {
                                    Console.WriteLine("invalid entry. Press any key to exit");
                                    Console.ReadKey();
                                    Console.Clear();
                                    return;
                                }

                                Console.WriteLine("{0}: {1}", numUsed, items[numUsed].name);
                                Console.WriteLine("Old price: {0}", items[numUsed].price);
                                Console.WriteLine("New price: {0}", checkString);
                                //validate
                                Console.WriteLine("\nIs this correct? y/n");
                                checkString = Console.ReadLine();

                                characterCheck(ref ans, checkString);

                                if (ans == 'y')//yes
                                {
                                    items[numUsed].price = newPrice;//update list

                                    //update the file
                                }
                                else if (ans == 'n')//no
                                {
                                    Console.WriteLine("Values were not changed. Press any key to exit");
                                    Console.ReadKey();
                                    Console.Clear();
                                    numUsed = -1;
                                    return;
                                }
                                else//invalid
                                {
                                    Console.WriteLine("Invalid input. Press any key to exit");
                                    Console.ReadKey();
                                    Console.Clear();
                                    numUsed = -1;
                                    return;
                                }
                            }
                            else if(itemEdit == 0) //exit
                            {
                                Console.Clear();
                                numUsed = -1;
                                return;
                            }
                            else //invalid input
                            {
                                Console.WriteLine("Invalid input. Press any key to return");
                                Console.ReadKey();
                                Console.Clear();
                                numUsed = -1;
                                return;
                            }                          
                        }
                        else if(verb == "remove") //remove the item
                        {
                            //select and remove item in list
                            items.RemoveAt(numUsed);
                            ModifyFile(ref items, ref filePath); //update file
                        }
                    }
                    else if (ans == 'n')
                    {
                        Console.WriteLine("Item was not modified. Press any key to return");
                        Console.ReadKey();
                        numUsed = -1;
                        return;
                    }
                    else //if input was not a valid item
                    {
                        Console.WriteLine("Invalid input. Press any key to return");
                        Console.ReadKey();
                        numUsed = -1;
                    }                
                }
                else //if number out of range
                {
                    Console.WriteLine("The number provided was not a valid input. Press any key to return");
                    Console.WriteLine();
                    numUsed = -1;
                }
            }
            else //if not a valid input
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Press any key to return");
                Console.ReadKey();
                numUsed = -1;
            }
            return;
        }

        //modify the item file by updating the list of items available
        static void ModifyFile(ref List<VendingItem> items, ref string filePath)
        {
            //open file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false)) //file is not validated before as it has already been checked to popular the items class
            {
                foreach (VendingItem item in items)
                {
                    //write to file
                    file.WriteLine($"{item.name} {item.price}");
                }
            }          
        }
    }
}