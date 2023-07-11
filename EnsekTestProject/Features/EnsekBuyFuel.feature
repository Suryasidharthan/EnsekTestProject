Feature: Ensek buy fuel

Background: 
Given I have a valid login with access token
When I reset the test data

@mytag
 Scenario Outline: Buy fuel of each type by resetting the test data and then verify orders.   
    When I buy some <quantity> of fuelType having energy_id as <fuelId>
    Then I verify that the above order is returned in the orders list with the expected <fuelType> and <quantity> and the current Datetime details
    Examples: 
     | fuelType | quantity | fuelId |
     | electric | 1        | 3      |
     | gas      | 5        | 1      |
    # | nuclear  | 2        | 2      |
     | oil      | 1        | 4      |  
   
  # Scenarios to be implemented
  # Scenario Outline: When I try to buy a fuel which has quantity = 0 , then service should not place an order.
  #  When I buy some <quantity> of fuelType having energy_id as <fuelId>
  #  Then I verify that the above order is not placed and it is not returned in the orders list.
  #  Examples: 
  #   | fuelType | quantity | fuelId |
  #   | nuclear  | 2        | 2      |


  # Scenarios to be implemented
  #  Scenario Outline: When I buy a fuel then the total available quantity of the fuel should be updated.
  #  When I buy some <buy_quantity> of fuelType having energy_id as <fuelId>
  #  Then I verify that the above order is placed and it is returned in the orders list.
  #  And verify that total quantity of the fuel is deducted and the value is TotalQuantityBeforePurchase - buy_quantity
  #  Examples: 
  #   | fuelType | buy_quantity | fuelId |
  #   | nuclear  | 2            | 2      |
  #




    