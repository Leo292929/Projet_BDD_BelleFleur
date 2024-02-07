using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public MySql()
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
                string connexionString = "SERVER=localhost,3306;" +
                                         "DATABASE=fleur;" +
                                         "UID=" + identifiant + ";PASSWORD=" + password;
                connexion = new MySqlConnection(connexionString);
                connexion.Open();
                Console.WriteLine('yes')
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
            MySqlCommand command = connexion.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            string[] valueString = new string[reader.FieldCount];

            // on stock les valeurs reçues dans une liste de tableau de string
            while (reader.Read())
            {
                string[] line = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    line[i] = valueString[i];
                }
                data.Add(line);
            }
            reader.Close();
            command.Dispose();
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
        public string NonQueryAvecVariable(string requete, Dictionary<string, MySqlDbType> params_, object[] value)
        {
            string retour = null;
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
            return retour;
        }
        #endregion
    }
}
