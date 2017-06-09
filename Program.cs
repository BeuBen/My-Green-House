using Constellation;
using Constellation.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Donnee
{
    public string Name;
    public int Value;
    public string Unit;
    public Donnee(string name, int value, string unit)
    {
        this.Name = name;
        this.Value = value;
        this.Unit = unit;
    }
}
public class Capteur
{
    public Donnee Température;
    public Donnee Luminosité;
    public Donnee Humidité;
    public Donnee Niveau_eau;

    public Capteur() //Creation d'une classe avec toute les données des capteurs
    {
        this.Humidité = new Donnee("Humidité", 999, "%");
        this.Température = new Donnee("Température", 999, "°C");
        this.Niveau_eau = new Donnee("Niveau d'eau", 999, "L");
        this.Luminosité = new Donnee("Luminosité", 999, "Lux");
    }
}

namespace WebTest
{
    public class Program : PackageBase
    {
        private Random rdn = new Random();
        private Capteur Rcapteur = new Capteur();


        static void Main(string[] args)
        {
            PackageHost.Start<Program>(args);
        }

        public override void OnStart()
        {
            PackageHost.WriteInfo("Package starting - IsRunning: {0} - IsConnected: {1}", PackageHost.IsRunning, PackageHost.IsConnected);
            PackageHost.WriteInfo("Je suis le package nommé {0} version {1}", PackageHost.PackageName, PackageHost.PackageVersion);

            //Mettre les valeurs des capteurs dans la classe   
            Rcapteur.Humidité.Value = rdn.Next(10, 30);
            Rcapteur.Luminosité.Value = rdn.Next(10, 30);
            Rcapteur.Niveau_eau.Value = rdn.Next(10, 30);
            Rcapteur.Température.Value = rdn.Next(10, 30);

            Task.Factory.StartNew(() =>
            {
                while (PackageHost.IsRunning)
                {
                   //STATE OBJECT CAPTEURS
                   PackageHost.PushStateObject("Résultats Capteurs", Rcapteur, lifetime: PackageHost.GetSettingValue<int>("Interval") + 4);


                   //STATE OBJECT
                   //Un state object expire si après 4 seconde de son intervale il n'est pas mis à jour. C'est un choix arbitraire
                   Thread.Sleep(PackageHost.GetSettingValue<int>("Interval"));
                }
            });

        }
    }
}
