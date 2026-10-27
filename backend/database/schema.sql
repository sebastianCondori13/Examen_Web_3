CREATE TABLE "Products" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "Stock" INTEGER NOT NULL,
    "Category" TEXT NOT NULL,
    "Brand" TEXT NOT NULL,
    "Color" TEXT NOT NULL,
    CONSTRAINT "CK_Product_Price" CHECK (CAST(Price AS NUMERIC) > 0),
    CONSTRAINT "CK_Product_Stock" CHECK (Stock >= 0)
);


CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL,
    "Role" TEXT NOT NULL
);


CREATE TABLE "Sales" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Sales" PRIMARY KEY AUTOINCREMENT,
    "CreatedAt" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL,
    "CustomerName" TEXT NOT NULL,
    "Notes" TEXT NOT NULL,
    CONSTRAINT "FK_Sales_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "SaleItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_SaleItems" PRIMARY KEY AUTOINCREMENT,
    "SaleId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" TEXT NOT NULL,
    CONSTRAINT "CK_Item_Price" CHECK (CAST(UnitPrice AS NUMERIC) > 0),
    CONSTRAINT "CK_Item_Quantity" CHECK (Quantity > 0),
    CONSTRAINT "FK_SaleItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SaleItems_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_SaleItems_ProductId" ON "SaleItems" ("ProductId");


CREATE UNIQUE INDEX "IX_SaleItems_SaleId_ProductId" ON "SaleItems" ("SaleId", "ProductId");


CREATE INDEX "IX_Sales_UserId" ON "Sales" ("UserId");


CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
