Feature: Ensek buy fuel

@mytag
 Scenario Outline: Reset test data, buy fuel of Type = Electric, and verify orders
    Given I have a valid access token
   #When I reset the test data
    When I buy a <quantity> of fuelType having energy_id as <fuelId>
    Then I verify that the above order is returned in the orders list with the expected <quantity> and <fuelType> and Datetime details
    Examples: 
     | fuelType | quantity | fuelId |
  #   | electric | 1        | 3      |
     | gas      | 1        | 1      |
     | nuclear  | 2        | 2      |
     | oil      | 1        | 4      |  
   





    