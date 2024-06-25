Feature: API Test suite

As a user I want to test the API's for employee managment application

@apitest
Scenario: 01_Create employee in application using api post method and delete the same
Given I load addemployee.json api request file and input below parameters
| key    | value             |
| email  | jwick66@gmail.com |
| status | inactive          |
When I execute post api request for addemployee functionality and validate response
When I execute delete api request for deleteemployee functionality and validate response

@apitest
Scenario Outline: 02_Create employee in application using api post method and delete
Given I load addemployee.json api request file and input below parameters
| key    | value    |
| email  | <email>  |
| status | <status> |
When I execute post api request for addemployee functionality and validate response
When I execute delete api request for deleteemployee functionality and validate response
Examples: 
 | email                    | status   |
 | sreenathsm5007@gmail.com | inactive |
 | john.doe@example.com     | active   |
 | jane.smith@example.com   | inactive |
 | test.user@example.com    | active   |

 @apitest
Scenario Outline: 03_Create more employees in application and get all employees list
Given I load addemployee.json api request file and input below parameters
| key            | value            |
| name           | <name>           |
| email          | <email>          |
| gender         | <gender>         |
| status         | <status>         |
| proofsubmitted | <proofsubmitted> |
| department     | <department>     |
| salary         | <salary>         |
When I execute post api request for addemployee functionality and validate response
When I execute getallusers api request for getallexistingusers functionality and validate response
When I execute delete api request for deleteemployee functionality and validate response
Examples: 
 | name     | email                   | gender | status | proofsubmitted | department | salary |
 | Dichu    | Deeshma6@gmail.com      | Female | Active | PAN            | Foxtrot    | 135000 |
 | Sreenath | SMitrasenan06@gmail.com | Male   | Active | PAN            | Omega      | 17500  |
 | Amjed    | Amjed4u@gmail.com       | Male   | Active | PAN            | Sigma      | 10500  |

 Scenario Outline: 04_Execute complete workflow using create, get, update, getall and delete users
Given I load addemployee.json request file
When I execute post api request for addemployee functionality and validate response
Then I execute get api request for fetchuserdetail functionality and validate response
Given I load addemployee.json api request file and input below parameters
| key            | value            |
| name           | <name>           |
| email          | <email>          |
Then I execute update api request for update functionality and validate response

When I execute delete api request for deleteemployee functionality and validate response
Examples: 
 | name  | email            | 
 | Poman | Poman6@gmail.com | 

