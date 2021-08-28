create DATABASE inventory_management;

CREATE TABLE users
(
	Id			INTEGER		NOT NULL AUTO_INCREMENT,
	Username		VARCHAR(255),
	Password		VARCHAR(255),
	FirstName		VARCHAR(255),
	LastName		VARCHAR(255),
		Active TINYINT(1) DEFAULT 0,
			IsSuperUser TINYINT(1) DEFAULT 0,
	PRIMARY KEY(Id)

); 

CREATE TABLE products
(
	Id			INTEGER		NOT NULL AUTO_INCREMENT,
	Code		VARCHAR(255),
	Name		VARCHAR(255),
	CurrentQty		DECIMAL(10,5) DEFAULT 0 ,
	PRIMARY KEY(Id)

); 

CREATE TABLE merchants
(
	Id			INTEGER		NOT NULL AUTO_INCREMENT,
	Code		VARCHAR(255),
	Name		VARCHAR(255),
	EMail		VARCHAR(255),
	PRIMARY KEY(Id)

); 

CREATE TABLE security_privileges
(
	Id			INTEGER		NOT NULL AUTO_INCREMENT,
	Code		VARCHAR(255),
	Value		TINYINT(1) DEFAULT 0,
	UserId INTEGER Not null,
	PRIMARY KEY(Id),
	CONSTRAINT FK_merchants_UserId FOREIGN KEY(UserId) REFERENCES users(Id)
); 

CREATE TABLE emails
(
	Id			INTEGER		NOT NULL AUTO_INCREMENT,
	Recipient		VARCHAR(500),
	Subject		VARCHAR(500),
	Body		VARCHAR(500),
	FailedReson		VARCHAR(255),
	ResendAttempts INTEGER ,
	Status VARCHAR(500),
	   CreatedDateTime		DateTime,
	SentDateTime		DateTime,
	PRIMARY KEY(Id)

); 

INSERT INTO users( Username, Password, FirstName, LastName, Active, IsSuperUser) VALUES ('Admin', '82B537B20C1CF2EA0E384DB1F6BCE91D', 'Admin', 'Admin', 1, 1);
