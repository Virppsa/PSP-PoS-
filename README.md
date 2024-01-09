# PSP-PoS-

PoS system for software engineering course.

## Changes made to ITEMS endpoints

GET /cinematic/{companyId}/inventory does not have implemented filters.

## Changes made to ORDER endpoints

1. PUT /cinematic/{companyId}/itemOrders/{itemOrderId}/status and PUT /cinematic/{companyId}/itemOrders/{itemOrderId}/assign are merged into one PUT /cinematic/{companyId}/itemOrders/{itemOrderId} endpoint
2. Additional POST /cinematic/{companyId}/itemOrders/{itemOrderId} and DELETE /cinematic/{companyId}/itemOrders/{itemOrderId} endpoints were added.
3. Void feature for payments has been removed since Refund has exact same functionalilty.
4. Several update endpoints (assign worker, edit state) have been merged into one Update endpoint for Orders.
5. Loyalty system has been changed to offer flat discount based on amount of points accrued by customer.

## Changes made to USER endpoints

1. Removed the sign in endpoint since authentication was out of scope for this.
2. Changed LoyaltyBonuses to LayaltyPoints, since there was no way to make LoyaltyBonuses comply with the assignment requirements with the given API contract.

## Changes made to SERVICE endpoints

1. Changed query parametes of querying appointments to be StartDate and EndDate instead of just querying appointments of a certain day as it would be more convenient to our users.

## To run locally

prerequisites:

- dotnet sdk 8.0
- dotnet ef cli

Edit [appsettings.json](./PspPos/appsettings.json) file: put your local mssql db connection string (AND DO NOT COMMIT IT!)

```
git pull
dotnet ef database update
dotnet run
```
