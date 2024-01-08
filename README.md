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

## Added - 2023-12-11

Sample project with crud.

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
