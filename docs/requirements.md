## Background
E-Commerce is experiencing exponential growth and merchants who sell their goods or services online need a way to easily collect money from their customers.

We would like to build a payment gateway, an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.

Processing a card payment online involves multiple steps and entities:

![](card_payment_overview.png "Card payment overview")

**Shopper:** Individual who is buying the product online.

**Merchant:** The seller of the product. For example, Apple or Amazon.

**Payment Gateway:** Responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from the acquiring bank.

**Acquiring Bank:** Allows us to do the actual retrieval of money from the shopper’s card and pay out to the merchant. It also performs some validation of the card information and
then sends the payment details to the appropriate 3rd party organization for processing.

We will be building the payment gateway only and simulating the acquiring bank component in order to allow us to fully test the payment flow.

## Requirements
The product requirements for this initial phase are the following:

- A merchant should be able to process a payment through the payment gateway and receive one of the following types of response:
    - Authorized - the payment was authorized by the call to the acquiring bank
    - Declined - the payment was declined by the call to the acquiring bank
    - Rejected - invalid information was supplied to the payment gateway and therefore it has rejected the request
-  A merchant should be able to retrieve the details of a previously made payment

### Processing a payment
The payment gateway will need to provide merchants with a way to process a card payment. To do this, the merchant should be able to submit a request to the payment gateway. A payment request must include the following fields:

| Field        | Validation rules                     | Notes                                                                                                                                                                               |
|--------------|--------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Card number  | Required                             |                                                                                                                                                                                     |
|              | Between 14-19 characters long        |                                                                                                                                                                                     |
|              | Must only contain numeric characters |                                                                                                                                                                                     |
| Expiry month | Required                             |                                                                                                                                                                                     |
|              | Value must be between 1-12           |                                                                                                                                                                                     |
| Expiry year  | Required                             |                                                                                                                                                                                     |
|              | Value must be in the future          | Ensure the combination of expiry month + year is in the future                                                                                                                      |
| Currency     | Required                             | Refer to the list of [ISO currency codes](https://www.xe.com/iso4217.php). Ensure your submission validates against no more than 3 currency codes                                   |
|              | Must be 3 characters                 |                                                                                                                                                                                     |
| Amount       | Required                             | Represents the amount in the minor currency unit. For example, if the currency was USD then <ul><li>$0.01 would be supplied as 1</li><li>$10.50 would be supplied as 1050</li></ul> |
|              | Must be an integer                   |                                                                                                                                                                                     |
| CVV          | Required                             |                                                                                                                                                                                     |
|              | Must be 3-4 characters long          |                                                                                                                                                                                     |
|              | Must only contain numeric characters |                                                                                                                                                                                     |

Responses must include the following fields:

| Field                 | Notes                                                                                                                                                                               |
|-----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Id                    | This is the payment id which will be used to retrieve the payment details. Feel free to choose whatever format you think makes most sense e.g. a GUID is fine                       |
| Status                | Must be one of the following values `Authorized`, `Declined`                                                                                                                        |
| Last four card digits | Payment gateways cannot return a full card number as this is a serious compliance risk. However, it is fine to return the last four digits of a card                                |
| Expiry month          |                                                                                                                                                                                     |
| Expiry year           |                                                                                                                                                                                     |
| Currency              | Refer to the list of [ISO currency codes](https://www.xe.com/iso4217.php). Ensure your submission validates against no more than 3 currency codes                                   |
|                       |                                                                                                                                                                                     |
| Amount                | Represents the amount in the minor currency unit. For example, if the currency was USD then <ul><li>$0.01 would be supplied as 1</li><li>$10.50 would be supplied as 1050</li></ul> |

Consider that the response fields don’t need to represented in your API as part of the HTTP body. Use what you feel makes the most sense to provide a good experience to the merchants calling the gateway you are implementing.

### Retrieving a payment’s details 
The second requirement for the payment gateway is to allow a merchant to retrieve details of a previously made payment using its identifier. Doing this will help the merchant with their reconciliation and reporting needs. The response should include a masked card number and card details along with a status code which indicates the result of the payment.

| Field                 | Notes                                                                                                                                                                               |
|-----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Id                    | This is the payment id which will be used to retrieve the payment details. Feel free to choose whatever format you think makes most sense e.g. a GUID is fine                       |
| Status                | Must be one of the following values `Authorized`, `Declined`                                                                                                                        |
| Last four card digits | Payment gateways cannot return a full card number as this is a serious compliance risk. However, it is fine to return the last four digits of a card                                |
| Expiry month          |                                                                                                                                                                                     |
| Expiry year           |                                                                                                                                                                                     |
| Currency              | Refer to the list of [ISO currency codes](https://www.xe.com/iso4217.php). Ensure your submission validates against no more than 3 currency codes                                   |
|                       |                                                                                                                                                                                     |
| Amount                | Represents the amount in the minor currency unit. For example, if the currency was USD then <ul><li>$0.01 would be supplied as 1</li><li>$10.50 would be supplied as 1050</li></ul> |

**Note: Payment Storage** You do not need to integrate with a real storage engine or database. It is fine to use a test double to represent this.

## Documentation
Please document your key design considerations and assumptions made when the test is performed as a take away exercise.

## Implementation considerations
We expect the following with each submission:
- Code must compile
- Your code is covered by automated tests. It is your choice which type of tests and the number of them you want to implement.
- The code to be simple and maintainable. We do not want to encourage overengineering.
- Your API design and architecture should be focused on meeting the functional requirements outlined above. Nothing more.