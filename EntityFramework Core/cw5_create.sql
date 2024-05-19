-- tables
-- Table: Client
CREATE TABLE Client
(
    IdClient  int           NOT NULL IDENTITY (1,1),
    FirstName nvarchar(120) NOT NULL,
    LastName  nvarchar(120) NOT NULL,
    Email     nvarchar(120) NOT NULL,
    Telephone nvarchar(120) NOT NULL,
    Pesel     nvarchar(120) NOT NULL,
    CONSTRAINT Client_pk PRIMARY KEY (IdClient)
);

-- Table: Client_Trip
CREATE TABLE Client_Trip
(
    IdClient     int      NOT NULL,
    IdTrip       int      NOT NULL,
    RegisteredAt datetime NOT NULL,
    PaymentDate  datetime NULL,
    CONSTRAINT Client_Trip_pk PRIMARY KEY (IdClient, IdTrip)
);

-- Table: Country
CREATE TABLE Country
(
    IdCountry int           NOT NULL IDENTITY (1,1),
    Name      nvarchar(120) NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY (IdCountry)
);

-- Table: Country_Trip
CREATE TABLE Country_Trip
(
    IdCountry int NOT NULL,
    IdTrip    int NOT NULL,
    CONSTRAINT Country_Trip_pk PRIMARY KEY (IdCountry, IdTrip)
);

-- Table: Trip
CREATE TABLE Trip
(
    IdTrip      int           NOT NULL IDENTITY (1,1),
    Name        nvarchar(120) NOT NULL,
    Description nvarchar(220) NOT NULL,
    DateFrom    datetime      NOT NULL,
    DateTo      datetime      NOT NULL,
    MaxPeople   int           NOT NULL,
    CONSTRAINT Trip_pk PRIMARY KEY (IdTrip)
);

-- foreign keys
-- Reference: Country_Trip_Country (table: Country_Trip)
ALTER TABLE Country_Trip
    ADD CONSTRAINT Country_Trip_Country
        FOREIGN KEY (IdCountry)
            REFERENCES Country (IdCountry);

-- Reference: Country_Trip_Trip (table: Country_Trip)
ALTER TABLE Country_Trip
    ADD CONSTRAINT Country_Trip_Trip
        FOREIGN KEY (IdTrip)
            REFERENCES Trip (IdTrip);

-- Reference: Table_5_Client (table: Client_Trip)
ALTER TABLE Client_Trip
    ADD CONSTRAINT Table_5_Client
        FOREIGN KEY (IdClient)
            REFERENCES Client (IdClient);

-- Reference: Table_5_Trip (table: Client_Trip)
ALTER TABLE Client_Trip
    ADD CONSTRAINT Table_5_Trip
        FOREIGN KEY (IdTrip)
            REFERENCES Trip (IdTrip);


-- insert some values
INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
VALUES ('Karol', 'Smarol', 'test1@te.st', '505789456', '123456789'),
       ('Antek', 'Śmantek', 'test2@te.st', '503456123', '987654321'),
       ('Nina', 'Malina', 'test3@te.st', '501654987', '334455667'),
       ('Monika', 'Tika', 'test4@te.st', '502852741', '113355779')

INSERT INTO Trip (Name, Description, DateFrom, DateTo, MaxPeople)
VALUES ('Guatemala', 'I don''t have time for this', '2024-07-01', '2024-07-14', 12),
       ('Peru', 'I don''t have time for this', '2024-07-14', '2024-07-28', 8),
       ('Rome', 'I don''t have time for this', '2024-07-22', '2024-07-31', 6),
       ('Paris', 'I don''t have time for this', '2024-08-01', '2024-08-14', 8)

INSERT INTO Country (Name)
VALUES ('Guatemala'),
       ('Peru'),
       ('Italy'),
       ('France')

INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES (1, 1, '2024-05-01', null),
       (3, 1, '2024-05-01', null),
       (2, 2, '2024-05-13', null)

INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES (1, 1),
       (2, 2),
       (3, 3),
       (4, 4)