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
    "MoniLogin" 	TEXT UNIQUE NOT NULL,
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
    "TransactionName"   TEXT NOT NULL,
    "TransactionDate"	DATE NOT NULL,
    "TagId"		INTEGER NOT NULL,
    FOREIGN KEY("TagId") REFERENCES "Tags" ("TagId") ON DELETE CASCADE
);





-- ##################
-- # Data insertion #
-- ##################

-- User
INSERT INTO "Monies"("MoniLogin","MoniPwd") VALUES ('Admin', '1234');


-- Accounts
INSERT INTO "Accounts"("MoniId","AccountName" ,"AccountBalance") VALUES (1, 'Compte courant', 300.12);
INSERT INTO "Accounts"("MoniId","AccountName" ,"AccountBalance") VALUES (1, 'Compte Epargne', 20);


-- Tags 2007-01-01
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Enfants', 1);
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Maison', 1);
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Travail', 1);
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Nourriture', 1);
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Loisir', 1);
INSERT INTO "Tags"("TagName", "MoniId") VALUES ('Autre', 1);

-- Transactions
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 1, -109.90, 'Achat cododo', '2023-08-21');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 1, -25.99, 'Achat doudou', '2023-08-21');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 1, -150, 'Nounou Septembre', '2023-09-01');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 2, -681, 'Loyer Septembre', '2023-09-05');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 3, 1650, 'Paye Septembre', '2023-08-30');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(2, 6, 100, 'Economies Septembre', '2023-09-03');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 6, -100, 'Economies Septembre', '2023-09-03');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 2, -90, 'Assurance habitation', '2023-09-07');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 5, -69.99, 'Achat FFXVI', '2023-09-10');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 5, -50.70, 'Achat partitions guitare', '2023-09-15');
INSERT INTO "Transactions"("AccountId", "TagId", "TransactionAmount", "TransactionName", "TransactionDate") VALUES(1, 4, -154.87, 'Courses Leclerc drive', '2023-09-18');
