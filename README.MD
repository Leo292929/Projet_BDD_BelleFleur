
FOUGEROUD Léo

#Application de gestion de stock et de commande et de suivi de client pour le magasin BelleFleur

Le projet se présente sous la forme d’un code SQL se chargeant de la construction des classes et de leurs peuplements pour les exemples et d’un code en C# qui sert à gérer l’interface et la base de données.


Le but étant de simuler le fonctionnement d’un magasin de fleur nous avons créés 5 tables :

**Table « client »**

Cette table contient les informations relatives aux clients du magasin de fleurs.
Chaque client est identifié par un ID unique (idclient).
Les colonnes de la table incluent le nom, le prénom, l'adresse e-mail (unique), le mot de passe, l'adresse de facturation, le numéro de téléphone et le numéro de carte bancaire du client.

**Table « bouquet »**

Cette table stocke les informations sur les bouquets de fleurs disponibles dans le magasin.
Chaque bouquet est identifié par un ID unique (idbouquet).
Les colonnes de la table incluent le nom du bouquet, le prix, le type de bouquet (standard ou personnalisé) et la catégorie du bouquet (toute occasion, fêtes, Saint Valentin, enterrement, mariage, particulière).

**Table « commande »**

Cette table enregistre les détails des commandes passées par les clients.
Chaque commande est identifiée par un ID unique (idcommande).
Les colonnes de la table comprennent l'ID du client (idclient), la date de la commande, la date de livraison, l'adresse de livraison, l'ID du bouquet commandé (idbouquet), le prix de la commande et l'état de la commande (différentes étapes du processus de commande sont représentées par des codes).

**Table « item »**

Cette table contient les informations sur les articles individuels tels que les fleurs et les accessoires vendus dans le magasin.
Chaque article est identifié par un ID unique (iditem).
Les colonnes de la table incluent le nom de l'article, le type d'article (fleur ou accessoire), le prix, la disponibilité (toute l'année, en hiver, au printemps, en été, en automne) et la quantité en stock.
**Table « composition »**
Cette table définit la composition des bouquets en enregistrant les articles (fleurs ou accessoires) qui les composent.
Chaque composition est identifiée par un ID unique (idcompo).
Les colonnes de la table comprennent la quantité de chaque article dans la composition, l'ID du bouquet auquel la composition appartient (idbouquet), l'ID de l'article (iditem) et le nombre d'articles nécessaires pour la composition (nbitem).


Chaque commande est associée à un unique bouquet, et un bouquet est composé de plusieurs éléments de composition : un élément par item diffèrent. Les premiers bouquets correspondent au bouquets prédéfinis, par la suite dès qu’un client créera une commande avec un bouquet personnalisé, il rajoutera un nouveau bouquet dans la table.
Ces tables sont interconnectées à l'aide de clés primaires et de clés étrangères pour établir des relations entre les différentes entités et permettre la gestion des clients, des bouquets, des commandes, des articles et des compositions dans le magasin de fleurs.



Le code **C#** permet : 
De se connecter ou de se créer un compte ; sachant que l’administrateur (le patron) dispose d’un compte prédéfini avec l’id -2 (mail = mdp = root), qui lui permet de consulter les stocks (si le nombre d’un item est en dessous de 10 l’item est affiché en rouge), les stats et les commandes et de modifier l’état des commandes et les stocks. De même les employés ont aussi leurs comptes avec l’id -3(mail = mdp = bozo), ils disposent des mêmes fonctionnalités que l’administrateur avec la vision des statistiques en moins.
De plus l’administrateur peut exporter en XML les clients ayant commandé au cours des 6 dernier mois.
Pour un client classique : soit il se connecte via son email et son mot de passe soit il créer un nouveau compte (qui nécessite un email non utilisé) et il sera dans ce cas connecté automatiquement.
Il peut ensuite consulter les commandes qu’il a déjà passé ou en passer une nouvelle.
S’il choisit d’en faire une nouvelle, il rentre d’abord son adresse et la date de livraison souhaitée (attention au format de la date).
Ensuite, il choisit s’il veut créer un nouveau bouquet ou en choisir un parmi les bouquets préexistants.
Dans le premier cas il doit rentrer le nombre d’item souhaité puis quels seront ces items.
Dans le second deux cas il y a 2 possibilités : la date de livraison est moins de trois jours après la date de commande auquel cas un employé vient s’en occuper, ou la date de livraison est plus de trois jours après la date de commande et il lui suffit de choisir son bouquet.
Peut importe l’utilisateur, a la fin de chaque partie il lui est demandé s’il souhaite revenir au menu principal ou quitter (ce qui retourne à l’écran de connexion/inscription du début).
