# VendingMachine
The program is designed to show usage of persistent data with text files, nesting of menus and data, as well as in basic database knowledge

Upon loading the program, the user will be prompted for a PIN. The default PIN is 1234, with an account balance of 0000.
Upon successful login, 4 menus are displayed, as well as an exit option, that utilises an authorisation loop.

1 - Purchase items
  The user can purchase items contained within the machine.
  Items are displayed in line with actual currency, including to 2 decimal places.
  
2 - add funds
  User can add funds to the account in order to purchase items. 
  
3 - edit items displayed within the vending machine
  With admin level authorisation on the account, the user can edit items contained within the machine, including price and name. It not only writes these to the local variables that contain the data, but also to the text file that contains this data.
  
4 - edit account information
  User can edit information contained within their account, including name and PIN. Each PIN is unique to the user. It also creates a new file in a new folder in order to output a copy to the new user, which can then be transferred to them.
  
A vending machine program built in c# that reads data and persistent data
