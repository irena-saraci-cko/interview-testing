There were a few assumptions and simplifications that were made during the implementation:
- No merchant information is stored for a given payment.
- Security considerations such as request authentication and authorization was not implemented.
- The card numbers are not encrypted but stored as plain text.
- The logs are only written to the console.
- The payments are not stored to a real data storage, but instead a stub is used
- The integration tests to cover the payments controller were not added due to time constrains
- All invalid input data are returned as a 422 response including the acquirer BadRequests
- Idempotency checks were not taken into consideration

**How to run the application**

Spin up the container by running ```docker-compose up```

**Explore API**

Navigate to https://localhost:7092/swagger/index.html

**Send requests**

Post
```
curl --location --request POST 'https://localhost:7092/payments' \
--header 'Content-Type: application/json' \
--data-raw '
 { "cardNumber": "2222405343248112",
  "expiryMonth": 11,
  "expiryYear": 2026,
  "currency": "USD",
  "amount": 60000,
  "cvv": "456"
}
'
```

Get

```curl --location --request GET 'https://localhost:7092/payments/3fa85f64-5717-4562-b3fc-2c963f66afa6'```


