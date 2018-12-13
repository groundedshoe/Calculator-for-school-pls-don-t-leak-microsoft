using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string LeftTerm = "0"; // termen till vänster 
        string MidTerm = ""; // tar det som ska hända
        string RightTerm = ""; // argumentet för det som ska hända
        bool TermState = false; // villken term som kan redigeras representerar denna. false är vänster och true är höger
        bool Deg = false; // för sin/cos/tan då man använder antingen grader eller radiens. false är radiens
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) { // alla knappar leder hit
            SetTerms((sender as Button).Text);//skickar namnet av knappen till SetTerms
            UpdateLabel(); // updaterar texten
        }
        void UpdateLabel() { // uppdaterar texten
            label1.Text = LeftTerm + MidTerm + RightTerm;
        }
        /// <summary>
        /// sätter alla variabler till det de ska vara när vi startar Resolve()
        /// Hanterar också när Resolve() ska starta
        /// altså en god funktion
        /// </summary>
        void SetTerms(string Arg) {
            int v;
            if (Int32.TryParse(Arg,out v)) { // kollar ifall den är ett heltal
                if (TermState) {//ifall sant kollar den villken term används och lägger till
                    if(RightTerm == "0") { // ser till att det inte blir tal med 0 först
                        RightTerm = Arg;
                    }else {
                        RightTerm += Arg;
                    }
                }else {
                    if(LeftTerm == "0") {
                        LeftTerm = Arg;
                    } else {
                        LeftTerm += Arg;
                    }
                }
               return;//sparar minne
            }
            switch (Arg) {//ifall detta inte går då så startar den ett switch case
                case "+":
                case "-":
                case "/":
                case "*":
                case "%":
                case "^":
                case "n√"://alla som kräver användar input på högertermen
                    if (MidTerm != "") {//ifall mitten termen är inte blank måste den bli klar innan något nytt argument
                        Resolve();//den använder argumenten för att annars skulle den bara försvinna
                        MidTerm = Arg;//sätter mitten till sig
                        RightTerm = "0";
                        TermState = true;
                    }else {//bar sätter mitten till det det ska vara
                        MidTerm = Arg;
                        RightTerm = "0";
                        TermState = true;
                    }
                    break;
                case "1/x":
                case "√":
                case "tan":
                case "cos":
                case "sin"://för allt som inte kräver mer än 1 argument
                    Resolve();
                    MidTerm = Arg;
                    Resolve();
                    break;
                case "="://lika med. sätter bara ihop med argument
                    Resolve();
                    break;
                case ","://komma. Man kan inte ha mer än 1 komma sim ni vet så har vi lite har inte satser
                    if (TermState && !(RightTerm.Contains(','))) {
                        RightTerm += ',';
                    }else if((!TermState && !(LeftTerm.Contains(',')))){
                        LeftTerm += ',';
                    }
                    break;
                case "*-"://för att göra nuvarande term till negativt
                    if (TermState) {
                        RightTerm = (float.Parse(RightTerm) * -1).ToString();
                    }else {
                        LeftTerm = (float.Parse(LeftTerm) * -1).ToString();
                    }
                    break;
                case "C": // ta bort en
                    if (TermState) {
                        RightTerm = RightTerm.Remove(RightTerm.Length-1);
                        if (RightTerm == "") {
                            RightTerm = "0";
                        }

                    } else {
                        LeftTerm = LeftTerm.Remove(LeftTerm.Length - 1) ;
                        if(LeftTerm == "") {
                            LeftTerm = "0";
                        }
                    }
                    break;
                case "Deg/Rad"://byt mellan radiens och grader
                    Deg = !Deg;
                    break;
                case "CE"://allt blir som vid starten(förutom grader/radiens)
                    LeftTerm = "0";
                    MidTerm = "";
                    RightTerm = "";
                    TermState = false;
                    break;

            }
                
        }
        void Resolve() {//ser till att kalkyleringen händer.
            float a = 0;
            if ((float.TryParse(LeftTerm.Replace(",","") + RightTerm.Replace(",",""), out a))){//bara ifall du på något sätt fick ∞66 som float.parse inte tillåter
                a = default(float);
                switch (MidTerm) {//kollar mitten termen
                    case "+":
                        LeftTerm = (float.Parse(LeftTerm) + float.Parse(RightTerm)).ToString();//addera talen
                        break;
                    case "-":
                        LeftTerm = (float.Parse(LeftTerm) - float.Parse(RightTerm)).ToString();//minus
                        break;
                    case "/":
                        try {
                            LeftTerm = (float.Parse(LeftTerm) / float.Parse(RightTerm)).ToString();//dividera
                        } catch {//ifall nån försöker nåt lustigt
                            LeftTerm = "0";
                            MidTerm = "";
                            RightTerm = "";
                            TermState = false;
                        }
                        break;
                    case "*":
                        LeftTerm = (float.Parse(LeftTerm) * float.Parse(RightTerm)).ToString();
                        break;
                    case "1/x":
                        LeftTerm = (1.0f / (float.Parse(LeftTerm))).ToString();//invers
                        break;
                    case "√":
                        if (!(float.Parse(LeftTerm) < 0)) {
                            LeftTerm = (Math.Sqrt(float.Parse(LeftTerm))).ToString();
                        }
                        break;
                    case "^":
                        LeftTerm = (Math.Pow(float.Parse(LeftTerm), float.Parse(RightTerm))).ToString();
                        break;
                    case "%":
                        LeftTerm = ((float.Parse(LeftTerm) / float.Parse(RightTerm)) * 100).ToString();
                        break;
                    case "n√":
                        LeftTerm = (Math.Pow(float.Parse(LeftTerm), 1 / float.Parse(RightTerm))).ToString();
                        break;
                    case "tan"://trigonometriska funktionerna
                        if (Deg) {//ifall du använder grader
                            LeftTerm = (Math.Tan(float.Parse(LeftTerm) / 57.2957795)).ToString();
                        } else {
                            LeftTerm = (Math.Tan(float.Parse(LeftTerm))).ToString();
                        }
                        break;
                    case "cos":
                        if (Deg) {
                            LeftTerm = (Math.Cos(float.Parse(LeftTerm) / 57.2957795)).ToString();
                        } else {
                            LeftTerm = (Math.Cos(float.Parse(LeftTerm))).ToString();
                        }
                        break;
                    case "sin":

                        if (Deg) {
                            LeftTerm = (Math.Sin(float.Parse(LeftTerm) / 57.2957795)).ToString();
                        } else {
                            LeftTerm = (Math.Sin(float.Parse(LeftTerm))).ToString();
                        }
                        break;

                }
                if (LeftTerm == "") {//ser till att den inte kan bli blankt villket ger float.parse errors
                    LeftTerm = "0";
                }
                MidTerm = "";
                RightTerm = "";
                TermState = false;
            }else {
                LeftTerm = "0";
                MidTerm = "";
                RightTerm = "";
                TermState = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            UpdateLabel();
        }
    }
}