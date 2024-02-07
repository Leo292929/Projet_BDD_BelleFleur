drop database if exists Fleur;
create database Fleur;
use Fleur;

Drop table if exists client;
Drop table if exists composition;
Drop table if exists bouquet;
Drop table if exists commande;
Drop table if exists item;
Drop table if exists Client;


CREATE TABLE IF NOT EXISTS client(
 idclient int NOT NULL AUTO_INCREMENT,
 nom varchar(20),
 prenom varchar(20),
 couriel varchar(255) UNIQUE,
 mdp varchar(255),
 adressedefacturation varchar(255),
 numtel varchar(255),
 cb bigint,
 PRIMARY KEY(idclient)
 );
 CREATE TABLE  IF NOT EXISTS bouquet(
 idbouquet int NOT NULL AUTO_INCREMENT,
 nombouquet varchar(255),
 prixbouquet decimal(10,2),
 typebouquet ENUM('standard', 'personalise'),
 categorie ENUM('TOUTE OCCASION', 'FETES','SAINT VALENTIN','ENTEREMENT','MARIAGE','PARTICULIERE'),
 PRIMARY KEY(idbouquet)
 );
 
 CREATE TABLE  IF NOT EXISTS commande(
 idcommande int AUTO_INCREMENT,
 idclient int NOT NULL,
 datecommande datetime,
 datelivraison datetime,
 adresselivraison varchar(255),
 idbouquet int,
 prixcommande decimal(10,2),
 etatcommande ENUM('VINV','CC','CPAV','CAL','CL',''),
 PRIMARY KEY(idcommande),
 FOREIGN KEY (idclient) REFERENCES client(idclient),
 FOREIGN KEY (idbouquet) REFERENCES bouquet(idbouquet)
 );
 CREATE TABLE  IF NOT EXISTS item(
 iditem int AUTO_INCREMENT,
 nomitem varchar(255),
 typeitem ENUM('FLEUR', 'ACCESSOIRE'),
 prixitem decimal(10,2),
 dispo ENUM('TOUTE LANNEE','HIVER','PRINTEMPS','ETE','AUTOMNE'),
 quantiteitem int,
 PRIMARY KEY(iditem)
 );
 CREATE TABLE  IF NOT EXISTS composition(
 idcompo int NOT NULL AUTO_INCREMENT,
 quantite int,
 idbouquet int,
 iditem int,
 nbitem int,
 PRIMARY KEY(idcompo),
 FOREIGN KEY (idbouquet) REFERENCES bouquet(idbouquet),
 FOREIGN KEY (iditem) REFERENCES item(iditem)
 );
INSERT INTO client (nom,prenom,couriel,mdp,adressedefacturation,numtel,cb)
VALUES
  ("Candice","Knapp","massa@icloud.ca","ante.","P.O. Box 667, 3749 Placerat, Avenue","01 11 78 67 19",610364938791),
  ("Vanna","Williams","tempor.augue@protonmail.org","eget","Ap #893-9108 Ac Street","03 28 13 45 61",228389012283),
  ("Preston","Richmond","cursus.luctus@yahoo.edu","Phasellus","Ap #941-8880 Suspendisse Road","05 32 19 78 75",342908995897),
  ("Edward","Zamora","lobortis.ultrices.vivamus@icloud.couk","ornare,","P.O. Box 364, 9915 Elementum, Ave","04 32 70 47 70",135158977308),
  ("Hyatt","Wells","in.at@protonmail.couk","placerat,","989-5989 Dictum St.","01 12 62 17 63",373835409937),
  ("Petra","David","commodo.auctor@icloud.com","blandit.","9220 Etiam Ave","03 53 34 30 84",294070529545),
  ("Otto","Haynes","et.arcu@hotmail.couk","dolor","Ap #208-7675 Ornare. Rd.","01 64 22 64 98",828450279170),
  ("Erich","Navarro","enim.non@google.com","volutpat","111-9668 Sem Avenue","02 12 16 56 11",900366997400),
  ("Abraham","Dunlap","condimentum.eget@protonmail.org","primis","P.O. Box 203, 8621 Gravida Road","03 10 31 09 84",474684673106),
  ("Blaze","Hurst","et.magnis@icloud.edu","sem","443-2081 Pellentesque Road","04 64 81 44 81",349347463929);
INSERT INTO item (nomitem,typeitem,prixitem,dispo,quantiteitem)
VALUES
 ('Lys blanc','fleur', 3.99,'TOUTE LANNEE', 100 ),
 ('Oeillet blanc','fleur', 3.99,'TOUTE LANNEE', 200),
 ('Rose rouge', 'fleur',3.50,'HIVER', 0 ),
 ('Muguet', 'fleur',1.50,'PRINTEMPS', 150 ),
 ('Tulipe rouge', 'fleur',4.10,'ETE', 0 ),
 ('Pivoine rose','fleur', 2.00,'AUTOMNE', 0 ),
 ('Boule de mousse','accessoire', 3.00,'TOUTE LANNEE', 50 ),
 ("Papier d'emballage",'accessoire', 0.50,'TOUTE LANNEE', 250 ),
 ('Ruban blanc','accessoire',  1.99,'TOUTE LANNEE', 10 );
INSERT INTO bouquet (nombouquet, prixbouquet, typebouquet, categorie)
VALUES 
	('Bouquet printanier', 11.48, 'standard', 'TOUTE OCCASION'),
	('Bouquet romantique', 7.98, 'personalise', 'SAINT VALENTIN'),
	('Bouquet de mariage', 25.99, 'personalise', 'MARIAGE'),
    ('Bouquet de deuil', 19.99, 'standard', 'ENTEREMENT');
INSERT INTO commande (idclient, datecommande, datelivraison, adresselivraison, idbouquet, prixcommande, etatcommande)
VALUES 
	(1, '2023-05-10 09:00:00', '2023-05-12 14:00:00', '123 Rue Principale', 1, 11.48, 'CC'),
	(2, '2023-05-09 12:00:00', '2023-05-11 10:00:00', '456 Avenue des Fleurs', 2, 7.98, 'VINV'),
    (3, '2023-05-08 15:00:00', '2022-05-13 16:00:00', '789 Rue du Bonheur', 3, 25.99, 'CPAV'),
    (1, '2023-05-07 11:00:00', '2023-05-09 13:00:00', '234 Avenue des Roses', 4, 19.99, 'CAL');
INSERT INTO composition (quantite, idbouquet, iditem, nbitem)
VALUES 
	(3, 1, 1, 1),
	(2, 1, 2, 2),
	(1, 2, 1, 1),
	(4, 2, 2, 2),
    (2, 3, 1, 1),
    (3, 3, 3, 3);
    INSERT INTO bouquet (idbouquet,nombouquet, prixbouquet, typebouquet, categorie)
VALUES 
	(999999,'', 00.00, 'standard', 'TOUTE OCCASION');