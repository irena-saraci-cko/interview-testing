There were a few assumptions and simplifications that were made during the implementation
- No information about the merchant was stored for a given payment
- Request authentication and authorizion was not taken into consideration
- The cards are stored as plain text
- The logs are written to the console only
- Payments are stub is used instead of integrating with a real data storage
- The controller integration tests were not added due to time constrains
- All invalid input data are returned as a 422 response including the Acquirer BadRequests
-