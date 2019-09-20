### Vending Machine ###

* The vending machine offers Products; 
* In order to obtain a product from the machine, a Payment has to be made. The vending machine supports the following payment methods: 
  * cash
  * card
* Payment flow:
  1. The client specifies the selected product by providing the productId
  2. The client select the payment method: Cash or CreditCard
  3. The client pays the necessary amount
  4. The machine gives back change if needed
  5. The product is dispensed
* In order to analyse the profitability of the vending machine, following data is collected and stored. Later, the statistics will be presented using CSV reports:
  * Sales
  * Actual stock
* In order to keep the products stock updated, vending machine has the option to communicate with an admin;
- - - -
### Vending Machine Admin ###

* In order to communicate with the vending machine a socket connection is established;
* In order to see the current state of the vending machine, the admin can ask for the products list;
* The Vending Machine Admin provides the following functionalities:
  * Modify a product:
    * add a product
    * update a product
    * delete a product
  * Refill - all products quantity wll be set to 10; 
  * Ask for report


  





