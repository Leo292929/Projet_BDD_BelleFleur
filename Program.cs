using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml;

namespace Projet_BDD_Fleur
{
    public class SQL
    {
        /// classe traitant tout le sql: connexion requette modification de table etc...

        #region Attributs
        MySqlConnection connexion;
        #endregion

        #region Constructeur
        /// le constructeur de la classe SQL
        /// Initialise l'objet MySqlConnection qui permet la connexion à la base
        public void MySql()
        {
            connexion = null;
        }
        #endregion

        #region Connexion
        /// Connecte l'application à la base Fleur stockée en local 
        /// param: identifiant = id de l'utilisateur
        /// param: password = mdp de l'utilisateur
        /// retourne Null si l'opération s'est bien déroulée, une erreur sinon
        public string Connexion(string identifiant, string password)
        {
            try
            {
                string connexionString = "SERVER=localhost;port=3306;" +
                                         "DATABASE=fleur;" +
                                         "UID=" + "root" + ";PASSWORD=" + "root";
                connexion = new MySqlConnection(connexionString);
                connexion.Open();
                return null;
            }
            catch (MySqlException er)
            {
                return "Erreur de connexion : " + er.ToString();
            }
        }

        /// Ferme la connexion à la base
        /// retourn Null si l'opération s'est bien déroulée, une erreur sinon
        public string Close()
        {
            string retour = null;
            try
            {
                connexion.Close();
            }
            catch (Exception e)
            {
                retour = e.Message;
            }
            return retour;
        }
        #endregion

        #region Requete Select
        /// permet d'executer une requete SQL qui demande des valeur sans donner de variable
        /// parametre requete =  requete à executer
        /// retourne une liste de tableau de string où chaque élement de la liste représente une ligne et chaque élement du tableau, une colonne
        public List<string[]> Selection(string requete)
        {
            List<string[]> data = new List<string[]>();

            using (MySqlConnection connexion = new MySqlConnection("SERVER=localhost;port=3306;" +
                                         "DATABASE=fleur;" +
                                         "UID=" + "root" + ";PASSWORD=" + "root"))
            {
                connexion.Open();

                MySqlCommand command = connexion.CreateCommand();
                command.CommandText = requete;
                ////
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] line = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            line[i] = reader.GetValue(i).ToString();

                        }
                        data.Add(line);
                    }
                }
            }

            return data;
        }
        #endregion

        #region Requete NoValNoVar
        /// parametre requete =  requete à executer
        /// retourne Null si l'opération s'est bien déroulée, une erreur sinon
        public string NonQuerySansVariable(string requete)
        {
            string retour = null;
            try
            {
                MySqlCommand command = connexion.CreateCommand();
                command.CommandText = requete;
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (MySqlException er)
            {
                retour = "Erreur : " + er.ToString();
            }
            return retour;
        }
        #endregion

        #region Requete Modif
        /// permet d'executer une requete SQL qui modifie les tables en fonction des variables donnnées
        /// parametre requete =  requete à executer
        /// parametre params = dictionnaire contenant les noms des variables
        /// parametre value = liste des valeurs des variables
        /// retourne Null si l'opération s'est bien déroulée, une erreur sinon
        public string NonQueryAvecVar(string requete, Dictionary<string, MySqlDbType> params_, object[] value)
        {
            using (MySqlConnection connexion = new MySqlConnection("SERVER=localhost;port=3306;" +
                                         "DATABASE=fleur;" +
                                         "UID=" + "root" + ";PASSWORD=" + "root"))
            {
                connexion.Open();
                MySqlCommand command = connexion.CreateCommand();
                command.CommandText = requete;

                // On ajoute chaque variable à la requête
                int indice = 0;
                foreach (KeyValuePair<string, MySqlDbType> ele in params_)
                {
                    MySqlParameter param = new MySqlParameter(ele.Key, ele.Value);
                    param.Value = value[indice];
                    command.Parameters.Add(param);
                    indice += 1;
                }

                command.ExecuteNonQuery();
                command.Dispose();
                return null;
            }
        }
        #endregion

    }

    public class StatistiquesMagasinFleurs
    {
        private string connectionString = "SERVER=localhost;port=3306;" +
                                         "DATABASE=fleur;" +
                                         "UID=" + "root" + ";PASSWORD=" + "root;";

        public decimal CalculerPrixMoyenBouquet()
        {
            decimal prixMoyen = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT AVG(prixbouquet) FROM bouquet WHERE typebouquet = 'standard'";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    prixMoyen = Convert.ToDecimal(result);
                }
            }

            return prixMoyen;
        }

        public string ObtenirMeilleurClient()
        {
            string meilleurClient = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT nom, prenom FROM client WHERE idclient = (SELECT idclient FROM commande GROUP BY idclient ORDER BY COUNT(*) DESC LIMIT 1)";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    meilleurClient = result.ToString();
                }
            }

            return meilleurClient;
        }

        public string ObtenirBouquetStandardPlusPopulaire()
        {
            string bouquetPopulaire = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT nombouquet FROM bouquet WHERE typebouquet = 'standard' AND idbouquet = (SELECT idbouquet FROM commande GROUP BY idbouquet ORDER BY COUNT(*) DESC LIMIT 1)";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    bouquetPopulaire = result.ToString();
                }
            }

            return bouquetPopulaire;
        }

        public string ObtenirFleurMoinsVendue()
        {
            string fleurMoinsVendue = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT nomitem FROM item WHERE typeitem = 'FLEUR' AND iditem = (SELECT iditem FROM composition GROUP BY iditem ORDER BY SUM(quantite) ASC LIMIT 1)";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    fleurMoinsVendue = result.ToString();
                }
            }

            return fleurMoinsVendue;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("bienvenue chez bellefleur");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Voulez-vous vous connecter ou vous inscrire ? (1/2)");
            int n = Convert.ToInt32(Console.ReadLine());
            int id = -1;
            switch (n)
            {
                case 1:
                    do
                    {
                        Console.WriteLine("Veuillez donner votre adresse mail :");
                        string mail = Console.ReadLine();
                        Console.WriteLine("Veuillez donner votre mot de passe :");
                        string mdp = Console.ReadLine();
                        if (mail == "root" && mdp == "root")
                        {
                            id = -2;
                        }
                        else if (mail == "bozo" && mdp == "bozo")
                        {
                            id = -3;
                        }
                        else
                        {
                            id = Login(mail, mdp);
                        }
                    }
                    while (id == -1);
                    break;
                case 2:
                    do
                    {
                        Console.WriteLine("Donner des identifiants avec un email unique");
                        Console.WriteLine("nom :");
                        string vernom = Console.ReadLine();
                        Console.WriteLine("prénom :");
                        string prenom = Console.ReadLine();
                        Console.WriteLine("mail :");
                        string mail = Console.ReadLine();
                        Console.WriteLine("mot de passe :");
                        string mdp = Console.ReadLine();
                        Console.WriteLine("adresse de facturation:");
                        string adresse = Console.ReadLine();
                        Console.WriteLine("numéro de téléphone :");
                        string telephone = Console.ReadLine();
                        Console.WriteLine("coordonnées bancaires :");
                        string cb = Console.ReadLine();
                        id = Register(vernom, prenom, mail, adresse, telephone, mdp, cb);
                        ///Console.Clear();
                    }
                    while (id == -1);
                    break;
            }
            Console.Clear();
            if (id == -2) Admin();
            else if (id == -3) Employe();
            else
            {
                string nom = DonnerNom(id);
                while (n != 0)
                {
                    Console.Clear();
                    Console.WriteLine("Bonjour M./Mme " + nom + " !\n(1)Commander ?\n(2)consulter votre commande ?\n(0)Quitter");
                    n = Convert.ToInt32(Console.ReadLine());
                    switch (n)
                    {
                        case 1:
                            Commande(id);
                            break;
                        case 2:
                            CheckCommande(id);
                            break;
                    }
                }

            }
        }
        static int Login(string couriel, string mdp)
        {
            var sql = new SQL();
            int id = -1;
            try
            {
                string identifiant = "root"; // identifiant de connexion
                string password = "root"; // mot de passe de connexion
                // Connexion à la base de données
                string erreur = sql.Connexion(identifiant, password);
                if (erreur != null)
                {
                    Console.WriteLine(erreur);
                    return -1;
                }

                // Exécution de la requête SQL pour vérifier l'existence de l'utilisateur
                string requete = "SELECT COUNT(*) FROM Client WHERE couriel = '" + couriel + "' AND mdp = '" + mdp + "';";
                List<string[]> resultats = sql.Selection(requete);
                // Vérification de l'existence de l'utilisateur
                if (resultats.Count > 0 && resultats[0][0] != "0")
                {
                    string requete1 = "SELECT idClient FROM Client WHERE couriel = '" + couriel + "';";
                    List<string[]> resultatsID = sql.Selection(requete1);
                    if (resultatsID.Count > 0)
                    {
                        id = Int32.Parse(resultatsID[0][0]);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur de connexion" + e.ToString());
            }
            return id;
        }
        static int Register(string nom, string prenom, string couriel, string adressedefacturation, string numtel, string mdp, string cb)
        {
            var sql = new SQL();
            int id = -1;
            try
            {
                // On vérifie si l'email n'est pas déjà utilisé
                string countQuery = "SELECT COUNT(*) FROM Client WHERE couriel = '" + couriel + "';";
                int count = int.Parse(sql.Selection(countQuery)[0][0]);
                if (count == 0)
                {
                    // On insère le nouveau client
                    string insertQuery = "INSERT INTO Client (nom, prenom, couriel, adressedefacturation, numtel, mdp, cb) " +
                                         "VALUES ('" + nom + "','" + prenom + "','" + couriel + "','" + adressedefacturation + "','" + numtel + "','" + mdp + "','" + cb + "');";
                    Dictionary<string, MySqlDbType> insertParams = new Dictionary<string, MySqlDbType>();
                    insertParams.Add("@nom", MySqlDbType.VarChar);
                    insertParams.Add("@prenom", MySqlDbType.VarChar);
                    insertParams.Add("@couriel", MySqlDbType.VarChar);
                    insertParams.Add("@adressedefacutration", MySqlDbType.VarChar);
                    insertParams.Add("@numtel", MySqlDbType.VarChar);
                    insertParams.Add("@mdp", MySqlDbType.VarChar);
                    insertParams.Add("@cb", MySqlDbType.VarChar);
                    object[] insertValues = new object[] { nom, prenom, couriel, adressedefacturation, numtel, mdp, cb };
                    sql.NonQueryAvecVar(insertQuery, insertParams, insertValues);
                    // On récupère l'id du nouveau client
                    string idQuery = "SELECT LAST_INSERT_ID();";
                    id = int.Parse(sql.Selection(idQuery)[0][0]);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur de connexion" + e.ToString());
            }
            return id;
        }
        static string DonnerNom(int id)
        {
            var sql = new SQL();
            string nom = "";
            try
            {
                string requete = "SELECT nom FROM Client WHERE idClient = " + id + ";";
                List<string[]> resultats = sql.Selection(requete);
                nom = resultats[0][0];
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur de connexion" + e.ToString());
            }
            return nom;
        }
        static void Commande(int id)
        {
            int idclient = id;
            var sql = new SQL();
            ///int item;
            //int idcom = 0;
            double prixcommande = 0;
            decimal nouveauprixcommande = 0;
            //string requetecompo = "SELECT MAX(idcommande) FROM commande;";
            //List<string[]> resultatscompo = sql.Selection(requetecompo);
            //int idcompo = int.Parse(resultatscompo[0][0]);
            Console.Clear();
            Console.WriteLine("A quelle adresse voulez vous faire livrer cette commande?");
            string adresselivraison = Console.ReadLine();
            StringBuilder etatcommande = new StringBuilder("");
            DateTime datecommande = DateTime.Now;
            Console.WriteLine("Quand voulez vous faire livrer cette commande? (10/05/2023 12:07:37)");
            String datelivraisonstring = Console.ReadLine();
            DateTime datelivraison = DateTime.Parse(datelivraisonstring);
            int idbouquet = 999999;
            string insertCommande = "INSERT INTO Commande (idclient,datecommande,datelivraison,adresselivraison,idbouquet,prixcommande,etatcommande)" +
                                    "VALUES('" + idclient + "', '" + datecommande.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + datelivraison.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + adresselivraison + "', '" +idbouquet+"','"+ prixcommande + "', '" + etatcommande.ToString() + "'); ";
            Dictionary<string, MySqlDbType> insertcommande = new Dictionary<string, MySqlDbType>();
            insertcommande.Add("@idclient", MySqlDbType.Int32);
            insertcommande.Add("@datecommande", MySqlDbType.DateTime);
            insertcommande.Add("@datelivraison", MySqlDbType.DateTime);
            insertcommande.Add("@adresselivraison", MySqlDbType.VarChar);
            insertcommande.Add("@idbouquet", MySqlDbType.Int32);
            insertcommande.Add("@prixcommande", MySqlDbType.Decimal);
            insertcommande.Add("@etat", MySqlDbType.VarChar);
            object[] insertValues = new object[] { id, datecommande, datelivraison, adresselivraison,idbouquet, prixcommande, etatcommande };
            sql.NonQueryAvecVar(insertCommande, insertcommande, insertValues);
            Console.Clear();
            Console.WriteLine("Voulez vous:\n(1) choisir parmi une selection de bouquet\n(2) ou creer votre bouquet personalisé  ?");
            int n = Convert.ToInt32(Console.ReadLine());
            StringBuilder typebouquet = new StringBuilder("standard");
            switch (n)
            {
                case 1:
                    Console.Clear();
                    TimeSpan diff = datelivraison - datecommande;
                    if (Math.Abs(Convert.ToInt32(diff.TotalDays)) < 3)
                    {
                        etatcommande.Append("VINV");
                        Console.WriteLine("Un employé va vous prendre en charge");
                        Console.ReadKey();
                        break;
                    }
                    idbouquet = SelBouquet();

                    string requeteidcommande = "SELECT MAX(idcommande) FROM commande;";
                    List<string[]> paramidcommande = sql.Selection(requeteidcommande);
                    int idcommande = Convert.ToInt32(paramidcommande[0][0]);

                    string requeteupdatecommande = "UPDATE commande SET idbouquet = "+idbouquet+" WHERE idcommande = " + idcommande +  ";";
                    Dictionary<string, MySqlDbType> paramupdatecommandeidbouquet = new Dictionary<string, MySqlDbType>();
                    paramupdatecommandeidbouquet.Add("@idbouquet", MySqlDbType.VarChar);
                    object[] updatecommande = new object[] {idbouquet };
                    sql.NonQueryAvecVar(requeteupdatecommande, paramupdatecommandeidbouquet, updatecommande);


                    string requeteprixbouquet = "SELECT prixbouquet FROM bouquet WHERE idbouquet = " + idbouquet + ";";
                    List<string[]> resultatprixbouquet = sql.Selection(requeteprixbouquet);
                    nouveauprixcommande = decimal.Parse(resultatprixbouquet[0][0]);
                    etatcommande.Append("CC");
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Vous pouvez creer votre propre bouquet , combien d'item voulez vous?");
                    int nbitem = -1;
                    do
                    {
                        nbitem = Convert.ToInt32(Console.ReadLine());
                    } while (nbitem <= 0 && nbitem < 100);
                    Console.Clear();
                    string requeteidbouquet = "SELECT MAX(idbouquet) FROM bouquet;";
                    List<string[]> paramidbouquet = sql.Selection(requeteidbouquet);
                    idbouquet = Convert.ToInt32(paramidbouquet[0][0]);
                    for (int i = 0; i < nbitem; i++)
                    {
                        nouveauprixcommande += AffItem(idbouquet);
                    }
                    etatcommande.Append("CPAV");
                    int intnombouquet = idclient + idbouquet;
                    string nombouquet = intnombouquet.ToString();
                    string prixbouquet = nouveauprixcommande.ToString();
                    prixbouquet = prixbouquet.Replace(",", ".");
                    typebouquet.Clear();
                    typebouquet.Append("personalise");
                    string categorie = "PARTICULIERE";
                    string requetebouquet = "INSERT INTO bouquet (nombouquet,prixbouquet,typebouquet,categorie)" +
                         "VALUES('" + nombouquet + "', '" + prixbouquet + "', '" + typebouquet + "', '" + categorie + "'); ";
                    Dictionary<string, MySqlDbType> parambouquet = new Dictionary<string, MySqlDbType>();
                    parambouquet.Add("@nombouquet", MySqlDbType.VarChar);
                    parambouquet.Add("@prixbouquet", MySqlDbType.VarChar);
                    parambouquet.Add("@typebouquet", MySqlDbType.VarChar);
                    parambouquet.Add("@categorie", MySqlDbType.VarChar);
                    object[] insertbouquet = new object[] { nombouquet, prixbouquet, typebouquet, categorie };
                    sql.NonQueryAvecVar(requetebouquet, parambouquet, insertbouquet);                  
                    break;
            }
            Console.WriteLine("Votre commande a ete enregistrée,\n1) Revenir au menu\n2) Se deconnecter");
            int w = Convert.ToInt32(Console.ReadLine());
            switch (w)
            {
                case 1:
                    DonnerNom(id);
                    break;
                case 2:
                    string[] zzz = new string[] { "valeur1", "valeur2", "valeur3" };
                    Main(zzz);
                    break;
            }
        }
        static decimal AffItem(int idbouquet)
        {
            var sql = new SQL();
            decimal prixitem = 0;

            string requete = "SELECT iditem, prixitem, nomitem FROM item WHERE quantiteitem > 0;";
            List<string[]> resultats = sql.Selection(requete);
            foreach (string[] ligne in resultats)
            {
                string id = ligne[0];
                string prix = ligne[1];
                string nom = ligne[2];

                Console.WriteLine(id+") " + nom + ", Prix: " + prix);
            }
            Console.WriteLine("Veuillez choisir un item en donnant son identifiant");
            int numitem = Convert.ToInt32(Console.ReadLine());
            string requeteprix = "SELECT prixitem FROM item WHERE iditem = "+numitem+";";
            List<string[]> resultatsprix = sql.Selection(requeteprix);
            prixitem += decimal.Parse(resultatsprix[0][0]);
            string requetecheckitemcompo = "SELECT COUNT(*) FROM composition WHERE iditem = "+numitem+" AND idbouquet = "+idbouquet+";";
            List<string[]> result = sql.Selection(requetecheckitemcompo);
            if(int.Parse(result[0][0])>0)
            {
                string requeteupdatecompo = "UPDATE composition SET quantite = quantite+1, nbitem = nbitem+1 WHERE iditem = " + numitem + " AND idbouquet = " + idbouquet + ";";
                Dictionary<string, MySqlDbType> paramcompo = new Dictionary<string, MySqlDbType>();
                paramcompo.Add("@iditem", MySqlDbType.VarChar);
                paramcompo.Add("@idbouquet", MySqlDbType.VarChar);
                object[] updatecompo = new object[] { numitem, idbouquet };
                sql.NonQueryAvecVar(requeteupdatecompo, paramcompo, updatecompo);
            }
            else
            {
                string requeteinsertcompo = "INSERT INTO composition (quantite,idbouquet,iditem,nbitem)" +
                         "VALUES(1,"+idbouquet+","+numitem+",1); ";
                Dictionary<string, MySqlDbType> paraminsertcompo = new Dictionary<string, MySqlDbType>();
                paraminsertcompo.Add("@idbouquet", MySqlDbType.VarChar);
                paraminsertcompo.Add("@iditem", MySqlDbType.VarChar);
                object[] insertbouquet = new object[] { idbouquet,numitem };
                sql.NonQueryAvecVar(requeteinsertcompo, paraminsertcompo, insertbouquet);
            }
            return prixitem;
        }
        static int SelBouquet()
        {
            var sql = new SQL();
            int idbouquet;
            string standard = "standard";
            Console.WriteLine("Rentrez le numero du bouquet que vous voulez commander");
            string nbbouquetQuery = "SELECT count(*) FROM bouquet WHERE typebouquet = '" + standard + "';";
            List<string[]> resultrequetnbb = sql.Selection(nbbouquetQuery);
            int nbbouquet = int.Parse(resultrequetnbb[0][0]);
            for (int i = 0; i < nbbouquet; i++)
            {
                StringBuilder nombouquet = new StringBuilder("");
                StringBuilder categorie = new StringBuilder("");
                decimal prixbouquet;
                string infobouquetrequete = "SELECT idbouquet,nombouquet,categorie,prixbouquet FROM bouquet;";
                List<string[]> resultatsinfobouquet = sql.Selection(infobouquetrequete);
                idbouquet = int.Parse(resultatsinfobouquet[i][0]);
                nombouquet.Append(resultatsinfobouquet[i][1]);
                categorie.Append(resultatsinfobouquet[i][2]);
                prixbouquet = decimal.Parse(resultatsinfobouquet[i][3]);
                Console.WriteLine("n°" + idbouquet + ": le bouquet " + nombouquet + " est un bouquet " + categorie + ", il coute " + prixbouquet + "euros,");
            }
            idbouquet = int.Parse(Console.ReadLine());
            return idbouquet;
        }
        ////
        static void CheckCommande(int idclient)
        {
            try
            {
                var sql = new SQL();
                string requetecommande = "SELECT idcommande,datecommande,datelivraison,adresselivraison,idbouquet,prixcommande,etatcommande FROM commande WHERE idclient = '" + idclient + "';";
                List<string[]> resultatcommande = sql.Selection(requetecommande);
                Console.Clear();
                Console.WriteLine("Quelle commande voulez vous voir?");

                foreach (string[] ligne in resultatcommande)
                {
                    string date = ligne[1];
                    string id = ligne[0];
                    string requetenombouquet = "SELECT nombouquet FROM bouquet WHERE idbouquet = " + ligne[4] + ";";
                    List<string[]> resultatnombouquet = sql.Selection(requetenombouquet);
                    string nombouquettemp = resultatnombouquet[0][0];
                    Console.WriteLine("Commande n°"+id+"pour le bouquet " +nombouquettemp+" passé le "+date);
                }
                int idcommande = Convert.ToInt32(Console.ReadLine());
                string requetebonnecommande = "SELECT datecommande,datelivraison,adresselivraison,idbouquet,prixcommande FROM commande WHERE idcommande = '" + idcommande + "';";
                List<string[]> resultatbonnecommande = sql.Selection(requetebonnecommande);
                int idbouquet = Convert.ToInt32(resultatbonnecommande[0][3]);
                string requetenombouquetf = "SELECT nombouquet,prixbouquet FROM bouquet WHERE idbouquet = '" + idbouquet + "';";
                List<string[]> resultatnombouquetf = sql.Selection(requetenombouquetf);
                string nombouquet = resultatnombouquetf[0][0];
                string datecommande = resultatbonnecommande[0][0];
                string datelivraison = resultatbonnecommande[0][1];
                string adresselivraison = resultatbonnecommande[0][2];
                string prixcommande = resultatnombouquetf[0][1];
                Console.Clear();
                Console.WriteLine("Vous avez commandé le bouquet" + nombouquet + " le " + datecommande + ",il arrivera le " + datelivraison + " a l'adresse " + adresselivraison + " et il vous a couté " + prixcommande + ".");
                Console.WriteLine("Voulez vous:\n1) Consulter une autre commande,\n2) Revenir a l'accueil\n3) Quitter?");
                int n = 0;
                n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case (1):
                        CheckCommande(idclient);
                        break;
                    case (2):
                        DonnerNom(idclient);
                        break;
                    case (3):
                        string[] zzz = new string[] { "valeur1", "valeur2", "valeur3" };
                        Main(zzz);
                        break;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur:"+ e.ToString());
                string trz = Console.ReadLine();
            }
        }
        static void Admin()
        {
            int n = 0;
            Console.Clear();
            Console.WriteLine("Que voulez vous faire ?\n(1)Gerer les stocks,\n(2)Gerer les commandes,\n(3)Voir les stats?\n4)exporte en XML les personnes ayant commander plusieur fois au cours du dernier mois");
            n = Convert.ToInt32(Console.ReadLine());
            int m;
            switch (n)
            {
                case 1:
                        Console.WriteLine("Voulez-vous:\n1)Afficher les stocks,\n2)modifier les stocks?");
                        m = Convert.ToInt32(Console.ReadLine());
                        switch (m)
                        {
                            case 1:
                                affstocksA();
                                break;
                            case 2:
                                modstocksA();
                                break;
                        }
                        break;
                case 2:
                        Console.WriteLine("Voulez-vous:\n1)Afficher les commandes,\n2)modifier les commandes?");
                        m = Convert.ToInt32(Console.ReadLine());
                        switch (m)
                        {
                            case 1:
                                affcommandeA();
                                break;
                            case 2:
                                modcommandeA();
                                break;
                        }
                        break;
                case 3:
                        affstat();
                        break;
                case 4:
                        ExportClientsXML();
                        break;
            }
            Console.WriteLine("Que voulez vous faire ?\n(1)Continuer en tant qu'admin?\n(2)Revenir au menu?");
            int o = Convert.ToInt32(Console.ReadLine());
            switch (o)
            {
                case 1:
                    Admin();
                    break;
                case 2:
                    string[] zzz = new string[] { "valeur1", "valeur2", "valeur3" };
                    Main(zzz);
                    break;

            }
        }
        static void Employe()
        {
            int n = 0;
            Console.WriteLine("Que allez vous faire ?\n(1)Gerer les stocks,\n(2)Gerer les commandes?");
            n = Convert.ToInt32(Console.ReadLine());
            int m;
            switch (n)
            {
                case 1:
                    Console.WriteLine("Voulez-vous:\n1)Afficher les stocks,\n2)modifier les stocks?");
                    m = Convert.ToInt32(Console.ReadLine());
                    switch (m)
                    {
                        case 1:
                            affstocksA();
                            break;
                        case 2:
                            modstocksA();
                            break;
                    }
                    break;
                case 2:
                    Console.WriteLine("Voulez-vous:\n1)Afficher les commandes,\n2)modifier les commandes?");
                    m = Convert.ToInt32(Console.ReadLine());
                    switch (m)
                    {
                        case 1:
                            affcommandeA();
                            break;
                        case 2:
                            modcommandeA();
                            break;
                    }
                    break;
            }
            Console.WriteLine("Que voulez vous faire ?\n(1)Continuer en tant qu'employer?\n(2)Revenir au menu?");
            int o = Convert.ToInt32(Console.ReadLine());
            switch (o)
            {
                case 1:
                    Employe();
                    break;
                case 2:
                    string[] zzz = new string[] { "valeur1", "valeur2", "valeur3" };
                    Main(zzz);
                    break;
            }
        }
        static void affstocksA()
        {
            var sql = new SQL();
            Console.Clear();
            string requete = "SELECT iditem, nomitem, quantiteitem FROM item;";
            List<string[]> resultats = sql.Selection(requete);
            foreach (string[] ligne in resultats)
            {
                string id = ligne[0];
                string quantité = ligne[2];
                string nom = ligne[1];
                if (Convert.ToInt32(quantité)<11)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine(id + ") " + nom + ", quantité: " + quantité);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        static void modstocksA()
        {
            var sql = new SQL();
            Console.Clear();
            string requete = "SELECT iditem, nomitem, quantiteitem FROM item;";
            List<string[]> resultats = sql.Selection(requete);
            foreach (string[] ligne in resultats)
            {
                string id = ligne[0];
                string quantité = ligne[2];
                string nom = ligne[1];
                Console.WriteLine(id + ") " + nom + ", quantité: " + quantité);
            }
            Console.WriteLine("Quel item voulez vous modifier?");
            int n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Ecrivez la quantité a ajouter( en negatif pour retirer)");
            int q = Convert.ToInt32(Console.ReadLine());
            string requeteupdatenbitem = "UPDATE item SET quantiteitem = quantiteitem+'"+q+"'WHERE iditem = " + n + ";";
            Dictionary<string, MySqlDbType> paramnbitem = new Dictionary<string, MySqlDbType>();
            paramnbitem.Add("@quantiteitem", MySqlDbType.VarChar);
            paramnbitem.Add("@iditem", MySqlDbType.VarChar);
            object[] updatenbitem = new object[] { q, n };
            sql.NonQueryAvecVar(requeteupdatenbitem, paramnbitem, updatenbitem);
        }
        static void affcommandeA()
        {
            var sql = new SQL();
            string requetecommande = "SELECT idcommande,datecommande,datelivraison,adresselivraison,idbouquet,prixcommande,etatcommande,idclient FROM commande;";
            List<string[]> resultatcommande = sql.Selection(requetecommande);
            Console.Clear();
            foreach (string[] ligne in resultatcommande)
            {
                string datec = ligne[1];
                string id = ligne[0];
                string datel = ligne[2];
                string adresse = ligne[3];
                string etat = ligne[6];
                string prix = ligne[5];
                string requetenombouquet = "SELECT nombouquet FROM bouquet WHERE idbouquet = " + ligne[4] + ";";
                List<string[]> resultatnombouquet = sql.Selection(requetenombouquet);
                string nombouquet = resultatnombouquet[0][0];
                string requetenomclient = "SELECT nom FROM client WHERE idclient = " + ligne[7] + ";";
                List<string[]> resultatnomclient = sql.Selection(requetenomclient);
                string nomclient = resultatnomclient[0][0];
                string requeteprixbouquet = "SELECT prixbouquet FROM bouquet WHERE idbouquet = " + ligne[4] + ";";
                List<string[]> resultatprixbouquet = sql.Selection(requeteprixbouquet);
                string prixcommande = resultatprixbouquet[0][0];
                Console.WriteLine("Commande n°" + id + "pour le bouquet " + nombouquet + " passé le " + datec+" pour le client "+nomclient+".\nLa commande a couté:"+prix+" est a livrer le "+datel+" a l'adresse suivante "+adresse+"\nLa commande est dans l'etat:"+etat);
            }
        }
        static void modcommandeA()
        {
            var sql = new SQL();
            Console.Clear();
            string requete = "SELECT idcommande, etatcommande FROM commande;";
            List<string[]> resultats = sql.Selection(requete);
            foreach (string[] ligne in resultats)
            {
                string idCommandetemp = ligne[0];
                string etatCommande = ligne[1];
                Console.WriteLine("ID Commande: " + idCommandetemp + ", État: " + etatCommande);
            }
            Console.WriteLine("Quelle commande voulez-vous modifier ?");
            int idCommande = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Entrez le nouvel état de la commande :");
            string nouvelEtat = Console.ReadLine();
            string requeteUpdateEtat = "UPDATE commande SET etatcommande = @nouvelEtat WHERE idcommande = @idCommande;";
            Dictionary<string, MySqlDbType> parametres = new Dictionary<string, MySqlDbType>();
            parametres.Add("@nouvelEtat", MySqlDbType.VarChar);
            parametres.Add("@idCommande", MySqlDbType.Int32);
            object[] valeursParametres = new object[] { nouvelEtat, idCommande };
            sql.NonQueryAvecVar(requeteUpdateEtat, parametres, valeursParametres);
        }
        static void affstat()
        {
            StatistiquesMagasinFleurs statistiques = new StatistiquesMagasinFleurs();
            decimal prixMoyenBouquet = statistiques.CalculerPrixMoyenBouquet();
            string meilleurClient = statistiques.ObtenirMeilleurClient();
            string bouquetPopulaire = statistiques.ObtenirBouquetStandardPlusPopulaire();
            string fleurMoinsVendue = statistiques.ObtenirFleurMoinsVendue();
            Console.WriteLine("Prix moyen du bouquet : " + prixMoyenBouquet);
            Console.WriteLine("le meilleur client : " + meilleurClient);
            Console.WriteLine("le bouquet standart le plus populaire  : " + bouquetPopulaire);
            Console.WriteLine("la fleur la moins vendu : " + fleurMoinsVendue);
        }
        static void ExportClientsXML()
        {
            var sql = new SQL();
            DateTime lastMonth = DateTime.Now.AddMonths(-1);

            string requete = "SELECT c.idclient,c.nom,c.prenom,c.couriel FROM client c INNER JOIN commande cmd ON c.idclient = cmd.idclient WHERE cmd.datecommande >='"+lastMonth.ToString("yyyy-MM-dd HH:mm:ss")+"' GROUP BY c.idclient HAVING COUNT(cmd.idcommande) > 1";
            List<string[]> results = sql.Selection(requete);

            if (results.Count > 0)
            {
                // Créer le document XML
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement rootElement = xmlDoc.CreateElement("Clients");
                xmlDoc.AppendChild(rootElement);

                foreach (string[] row in results)
                {
                    string idclient = row[0];
                    string nom = row[1];
                    string prenom = row[2];
                    string couriel = row[3];

                    // Créer un élément Client pour chaque client
                    XmlElement clientElement = xmlDoc.CreateElement("Client");
                    clientElement.SetAttribute("ID", idclient);
                    clientElement.SetAttribute("Nom", nom);
                    clientElement.SetAttribute("Prenom", prenom);
                    clientElement.SetAttribute("Couriel", couriel);
                    rootElement.AppendChild(clientElement);
                }

                // Enregistrer le document XML dans un fichier
                string fileName = "clients_multiple_orders_last_month.xml";
                xmlDoc.Save(fileName);

                Console.WriteLine("Exportation réussie. Les clients ayant commandé plusieurs fois au cours du dernier mois ont été exportés en XML dans le fichier : " + fileName);
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                Console.WriteLine("Le fichier XML a été enregistré dans : " + currentDirectory);
            }
            else
            {
                Console.WriteLine("Aucun client n'a effectué plusieurs commandes au cours du dernier mois.");
            }
        }
    }

}