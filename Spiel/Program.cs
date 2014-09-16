using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio; 
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

// EINBINDEN VON SFML.NET
//  - Projektmappen-Explorer öffnen
//  - Rechtklick -> Verweis hinzufügen
//  - In der Linkenspalte "Durchsuchen"
//  - Ganz unten erneut auf "Durchsuchen"
//  - SFML.NET Ordner öffnen -> Libs
//  - Alle Auswählen und hinzufügen
//  - Schauen ob alle markiert sind und OK
//
//  - Rechtsklick -> Hinzufügen -> Vorhandenes Element
//  - SFML.NET Ordner öffnen -> extlibs
//  - ggf. "Alle Dateitypen (.*)" auswählen
//  - Alle Auswählen und hinzufügen
//  - Im Projektmappen-Explorer die 5 .dll auswählen
//  - Rechtsklick -> Eigenschaften
//  - Ins Ausgabeverzeichniskopieren -> "Immer kopieren" oder "Kopieren wenn neuer"

// KONSOLE AUSSCHALTEN
//  - Projektmappen-Explorer öffnen
//  - Rechtsklick auf das Projekt (Intro2D-02-Beispiel) -> Eigenschaften
//  - In den Reiter "Anwendung" (automatisch offen) wechseln
//  - Ausgabetyp -> "Windows-Anwendung"

// WICHTIG !!!!!!
//  - WENN IHR DIESES PROJEKT WEITERVERWENDEN WOLLT, MÜSST IHR DIE VERWEISE (erster Teil) NEU HINZUFÜGEN

// Wird für Programm ablauf benötigt

namespace Spiel
{
    class Program
    {
        static int index = 0;
        static Stopwatch uhr = new Stopwatch();
        static int[,] array = new int[6,7];
        static int gamerindex = 0;
        static bool oisdjfi = false;

        static void Main(string[] args)
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                    array[i, j] = 3;

            Texture bild = new Texture("Unbenannt.png");
            Sprite sprite = new Sprite(bild);
            CircleShape spieler1 = new CircleShape((float)25.0);
            CircleShape spieler2 = new CircleShape((float)25.0);
            spieler1.FillColor = new Color(Color.Red);
            spieler2.FillColor = new Color(Color.Blue);
            spieler1.Position = new Vector2f(98,50);
            spieler2.Position = new Vector2f(98, 50);
            CircleShape[] spieler = { spieler1, spieler2 };
            uhr.Start();
            
            //Fenster erzeugen
            RenderWindow win = new RenderWindow( new VideoMode(800 ,600), "4 GEWINNT");
            
            //Fenster geschloßen
            win.Closed += win_Closed;

            CircleShape gamer = spieler1;

            while (win.IsOpen())
            {
                win.Clear();
                win.Draw(sprite);
                zeichnespielfeld(spieler, win);
                if (!oisdjfi)
                {
                    win.Draw(spieler[gamerindex]);
                    if (uhr.ElapsedMilliseconds > 400f)
                        movement(spieler);
                }
                win.Display();
                win.DispatchEvents();
            }
        }

        static void movement(CircleShape[] spieler)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                if (index > 0)
                {
                    index--;
                    spieler[gamerindex].Position = new Vector2f(98 + index * 68, 50);
                    uhr.Restart(); 
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                if (index < 6)
                {
                    index++;
                    spieler[gamerindex].Position = new Vector2f(98 + index * 68, 50);
                    uhr.Restart();
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                int n=0;
                while (array[n, index] != 3)
                {
                    if (n == 6) break;
                    n++;
                }
                spieler[gamerindex].Position = new Vector2f(98, 50);
                array[n, index] = gamerindex;
                uhr.Restart();
                check();
                gamerindex = (gamerindex + 1) % 2;
                index = 0;
            }

        }

        static void zeichnespielfeld(CircleShape[] spieler, RenderWindow win)
        {
            Vector2f spieler1 = new Vector2f(spieler[0].Position.X,spieler[0].Position.Y);
            Vector2f spieler2 = new Vector2f(spieler[1].Position.X, spieler[1].Position.Y);
            float x = spieler[gamerindex].Position.X;
            float y = spieler[gamerindex].Position.Y;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (array[i, j] != 3)
                    {
                        spieler[array[i,j]].Position = new Vector2f(98 + j * 68, 385 - i*60);
                        win.Draw(spieler[array[i, j]]);
                    }
                }
            spieler[0].Position = spieler1;
            spieler[1].Position = spieler2;
        }

        static void win_Closed(object sender, EventArgs e)
        {
            //Fenster schließen
            (sender as Window).Close();
        }

        static void check()
        {
            int count=0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (array[i, j] == gamerindex)
                        count += 1;
                    else
                        count = 0;
                    if (count== 4)
                        win();
                }
                count = 0;
            }
            
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (array[j, i] == gamerindex)
                        count += 1;
                    else
                        count = 0;
                    if (count == 4)
                        win();
                }
                count = 0;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(array[i,j]==gamerindex) proofdiagonal1(i,j);
                }
            }

            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (array[i, j] == gamerindex) proofdiagonal2(i, j);
                }
            }
        }

        static void proofdiagonal1(int x, int y)
        {
            int count = 1;
            while (true)
            {
                x += 1; y += 1;
                if (array[x, y] == gamerindex)
                    count += 1;
                else break;
                if (count == 4) win();
            }

        }

        static void proofdiagonal2(int x, int y)
        {
            int count = 1;
            while (true)
            {
                x -= 1; y += 1;
                if (array[x, y] == gamerindex)
                    count += 1;
                else break;
                if (count == 4) win();
            }
        }
        static void win()
        {
            oisdjfi = true;
            
            //Fenster erzeugen
            RenderWindow win = new RenderWindow(new VideoMode(800, 600), "4 GEWINNT");

            //Fenster geschloßen
            win.Closed += win_Closed;

            while (win.IsOpen())
            {
           //     MessageBox.Show("Visual C# macht Spass");
                win.Capture();
                win.Display();
                win.DispatchEvents();
            }
        }
    }
}
