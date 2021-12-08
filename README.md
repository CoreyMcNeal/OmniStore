# OmniStore
OmniStore is a CLI C# Shopping application. The app connects to and communicates with a local SQL database (set up using MySQLWorkbench), allowing non volatile storage, and manipulation of user registrations, user account balances, and store stock.

The User must set up a local database using MySQL. The SQL Schema that is needed is provided, named "SQL Schema". This will make sure the database being written to is formatted correctly for queries and updates from the program.

# Required
- MySql.Data package
- Database admin user and password

#How To Use
 - On entry, the server name, database name, database admin username, and database admin password wil be entered
 - The user may login with a registered username and PIN (Integers) from the database, optionally create a new account, or exit from the same screen
 - Upon login, user is greeted and shown three different options, Go Shopping, Load Card, or Leave.
 - Shopping leads the user to a printed page of all the store database stock, with the option to buy an item via entering the product id
 - Load Card takes the user to an interface to load money onto their store account.
 - Leave exits the user from the program

#What I Learned
 - C# Syntax and Development
 - SQL Manipulation with the MySQL library in C#
 - SQL Setup in MySQLWorkbench
