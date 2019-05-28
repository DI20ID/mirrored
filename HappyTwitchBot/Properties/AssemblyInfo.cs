using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
[assembly: AssemblyTitle("HappyTwitchBot")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("HappyTwitchBot")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten.  Wenn Sie auf einen Typ in dieser Assembly von 
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.
[assembly: ComVisible(false)]

//Um mit dem Erstellen lokalisierbarer Anwendungen zu beginnen, legen Sie 
//<UICulture>ImCodeVerwendeteKultur</UICulture> in der .csproj-Datei
//in einer <PropertyGroup> fest.  Wenn Sie in den Quelldateien beispielsweise Deutsch
//(Deutschland) verwenden, legen Sie <UICulture> auf \"de-DE\" fest.  Heben Sie dann die Auskommentierung
//des nachstehenden NeutralResourceLanguage-Attributs auf.  Aktualisieren Sie "en-US" in der nachstehenden Zeile,
//sodass es mit der UICulture-Einstellung in der Projektdatei übereinstimmt.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //Speicherort der designspezifischen Ressourcenwörterbücher
                                     //(wird verwendet, wenn eine Ressource auf der Seite
                                     // oder in den Anwendungsressourcen-Wörterbüchern nicht gefunden werden kann.)
    ResourceDictionaryLocation.SourceAssembly //Speicherort des generischen Ressourcenwörterbuchs
                                              //(wird verwendet, wenn eine Ressource auf der Seite, in der Anwendung oder einem 
                                              // designspezifischen Ressourcenwörterbuch nicht gefunden werden kann.)
)]


// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.2.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


// ursprüngliche AssemblyVersion 1.0.0.0
// Features bis jetzt:
// Grafische Oberfläche zur Eingabe von Verbindungsinformationen, zum Verbinden und Steuerung der LED Cluster
// ircClient zum Senden der Verbindungsinformationen und Senden und Empfangen von
//      TwitchCommands/Messages (Senden von Commands und Messages nur verwendet für den Verbindungsaufbau; senden über die Oberfläche zu diesem Zeitpunkt nicht implementiert)
// ircPattern zum erkennen von Twitchchat Befehlen (!led etc.) und Twitch Debug Messages
//
//1.0.1.0
//Überholung der Grafischen Oberfläche für leichtere Bearbeitung und Bedienung
//          einige Initialisierungswerte in TextBoxen gelöscht (wieder implementieren -)
//Implementierung der Speicherung der Verbindungsinformationen in HappyTwitchBot.exe.config
//1.0.2.0
//implementierung von GUI für Communikation zwischen XCOM 2 und Twitch
//Toolbar zum ein und ausblenden verschiedener GUI Elemente
//hinzufügen einer Textbox als Ausgabefenster


