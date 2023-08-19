-- ########################
-- # Drop previous tables #
-- ########################
DROP TABLE IF EXISTS "Monies";
DROP TABLE IF EXISTS "Accounts";
DROP TABLE IF EXISTS "Snapshots";
DROP TABLE IF EXISTS "Transactions";
DROP TABLE IF EXISTS "Tags";


-- ###################
-- # Tables creation #
-- ###################
CREATE TABLE "Monies" (
    "MoniId" 		INTEGER PRIMARY KEY,
    "MoniLogin" 	TEXT NOT NULL,
    "MoniPwd"		TEXT NOT NULL
);

CREATE TABLE "Accounts" (
    "AccountId"		INTEGER PRIMARY KEY,
    "AccountName"	TEXT NOT NULL,
    "MoniId"		INTEGER NOT NULL,
    "AccountBalance"	REAL NOT NULL,
    FOREIGN KEY("MoniId") REFERENCES "Monies" ("MoniId") ON DELETE CASCADE
);

CREATE TABLE "Snapshots" (
    "SnapshotId" 	INTEGER PRIMARY KEY,
    "AccountId"		INTEGER NOT NULL,
    "SnapshotDate"	DATE NOT NULL,
    "SnapshotBalance"	REAL NOT NULL,
    FOREIGN KEY("AccountId") REFERENCES "Accounts" ("AccountId") ON DELETE CASCADE
);

CREATE TABLE "Tags" (
    "TagId" 		INTEGER PRIMARY KEY,
    "TagName" 		TEXT NOT NULL,
    "MoniId" 		INTEGER NOT NULL,
    FOREIGN KEY("MoniId") REFERENCES "Monies" ("MoniId") ON DELETE CASCADE
);

CREATE TABLE "Transactions" (
    "TransactionId"	INTEGER PRIMARY KEY,
    "AccountId"		INTEGER NOT NULL,
    "TransactionAmount" REAL NOT NULL,
    "TransactionDate"	DATE NOT NULL,
    "TagId"		INTEGER NOT NULL,
    FOREIGN KEY("TagId") REFERENCES "Tags" ("TagId") ON DELETE CASCADE
);





-- ##################
-- # Data insertion #
-- ##################

-- User
INSERT INTO "Monies"("MoniLogin","MoniPwd") VALUES ("Admin", "1234");


-- Accounts
INSERT INTO "Accounts"("MoniId","AccountName" ,"AccountBalance") VALUES (1, "Compte courant", 300.12);
INSERT INTO "Accounts"("MoniId","AccountName" ,"AccountBalance") VALUES (1, "Compte Epargne", "20");


-- Transactions

