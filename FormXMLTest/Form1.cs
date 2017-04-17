//-----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Goldsmith Engineering">
//     Copyright (c) Goldsmith Engineering. All rights reserved.
// </copyright>
// <author>Winston Goldsmith</author>
//-----------------------------------------------------------------------

namespace FormXMLTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// Initializes form, reads in data from XML file and displays it on the form.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// A list to hold all the Panels on the form that show different colors based on what type of paper is selected
        /// </summary>
        private Dictionary<int, Panel> colorBoxList = new Dictionary<int, Panel>();

        /// <summary>
        /// A list to hold all the Panels on the form that hold all the controllers that must be placed dynamically based on the XML file
        /// </summary>
        private Dictionary<int, Panel> containerList = new Dictionary<int, Panel>();

        /// <summary>
        /// A list to hold all the Labels on the form that show name of each printer
        /// </summary>
        private Dictionary<int, Label> printerList = new Dictionary<int, Label>();

        /// <summary>
        /// A list to hold all the RadioButtons on the form for selecting the mylar option for that printer
        /// </summary>
        private Dictionary<int, RadioButton> mylarList = new Dictionary<int, RadioButton>();

        /// <summary>
        /// A list to hold all the RadioButtons on the form for selecting the paper option for that printer
        /// </summary>
        private Dictionary<int, RadioButton> paperList = new Dictionary<int, RadioButton>();

        /// <summary>
        /// A list to hold all the RadioButtons on the form for selecting the other option for that printer
        /// </summary>
        private Dictionary<int, RadioButton> otherList = new Dictionary<int, RadioButton>();

        /// <summary>
        /// A list to hold all the Labels on the form that show the name of the last person to change the option on that printer, 
        /// as well as the date and time it was changed
        /// </summary>
        private Dictionary<int, Label> ndtList = new Dictionary<int, Label>();

        /// <summary>
        /// Initializes a new instance of the Form1 class. Auto generated constructor for Form1
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Reads the plot printers xml file and creates controllers based on the information in the file
        /// </summary>
        private void ReadXML()
        {
            int key = 0;
            ///////////////////////////////////////////////////////////////////////               Change to new file location              //////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            using (XmlReader reader = XmlReader.Create(@"K:\common\Winston_Goldsmith\plotprinters.xml"))
            { 
                while (reader.Read())
                {                
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name.Equals("printer"))
                            {
                                this.containerList.Add(key, new Panel());

                                this.printerList.Add(key, new Label());
                                this.printerList[key].Text = reader.GetAttribute("name");
                                this.printerList[key].Name = "printerLabel"+key;
                                this.printerList[key].AutoSize = true;
                            }
                            else if (reader.Name.Equals("user"))
                            {
                                this.ndtList.Add(key, new Label());
                                this.ndtList[key].Text = "Last changed by: " + reader.ReadElementContentAsString();
                                this.ndtList[key].Name = "ntdLabel" + key;
                                this.ndtList[key].AutoSize = true;
                            }

                            // month and hour are both preceded by " - "
                            else if (reader.Name.Equals("month") || reader.Name.Equals("hour"))
                            {
                                this.ndtList[key].Text = this.ndtList[key].Text + " - " + reader.ReadElementContentAsString();
                            }
                            else if (reader.Name.Equals("day") || reader.Name.Equals("year"))
                            {
                                this.ndtList[key].Text = this.ndtList[key].Text + "/" + reader.ReadElementContentAsString();
                            }
                            else if (reader.Name.Equals("minute"))
                            {
                                this.ndtList[key].Text = this.ndtList[key].Text + ":" + reader.ReadElementContentAsString();
                            }
                            else if (reader.Name.Equals("roll"))
                            {
                                this.colorBoxList.Add(key, new Panel());
                                this.colorBoxList[key].BorderStyle = BorderStyle.Fixed3D;
                                this.colorBoxList[key].Size = new Size(35, 25);
                                this.colorBoxList[key].Name = "colorBox" + key;

                                this.mylarList.Add(key, new RadioButton());
                                this.mylarList[key].Text = "Mylar";
                                this.mylarList[key].Name = "mylarButton" + key;
                                this.mylarList[key].AutoSize = true;

                                this.paperList.Add(key, new RadioButton());
                                this.paperList[key].Text = "Paper";
                                this.paperList[key].Name = "paperButton" + key;
                                this.paperList[key].AutoSize = true;

                                this.otherList.Add(key, new RadioButton());
                                this.otherList[key].Text = "Other";
                                this.otherList[key].Name = "otherButton" + key;
                                this.otherList[key].AutoSize = true;

                                SetRoll(reader.ReadElementContentAsString(), key);
                            }

                            break;

                        case XmlNodeType.EndElement:  // when it reaches the end of a printer, increase key by 1
                            if (reader.Name.Equals("printer"))
                            {
                                key++;
                            }

                            break;
                    }                   
                }
            }
        }

        /// <summary>
        /// Reads the plot printers xml file and creates controllers based on the information in the file
        /// </summary>
        /// <param name="roll">The name of the roll that was read from the XML file.</param>
        /// <param name="key">The number of the current printer.</param>
        private void SetRoll(string roll, int key)
        {
            if (roll.Equals("Mylar"))
            {
                this.colorBoxList[key].BackColor = Color.Red;
                this.mylarList[key].Checked = true;
            }
            else if (roll.Equals("Paper"))
            {
                this.colorBoxList[key].BackColor = Color.White;
                this.paperList[key].Checked = true;
            }
            else if (roll.Equals("Other"))
            {
                this.colorBoxList[key].BackColor = Color.Green;
                this.otherList[key].Checked = true;
            }
        }

        /// <summary>
        /// Places container panels in the group box and spaces each one 41 spaces from the last one
        /// </summary>
        private void PlaceContainerPanel(int key, int spacer)
        {
            // On the first entry the key will be 0, so container will be placed at 5, 15
            // On all others the container will be placed 41 spaces lower than the previous container
            this.containerList[key].Location = new Point(5, 20 + (key * spacer));
            this.containerList[key].Size = new Size(this.paperStatGroupBox.Width - 10, spacer);
            this.paperStatGroupBox.Controls.Add(this.containerList[key]);  
        }

        private void PlaceInContainer(int key)
        {
            this.colorBoxList[key].Parent = this.containerList[key];
            this.colorBoxList[key].Location = new Point(0, 0);
            ////this.containerList[key].Controls.Add(this.colorBoxList[key]);
            ////this.containerList[key].Controls.Find("colorBox" + key, true).;

            this.printerList[key].Location = new Point(37, 6);
            this.containerList[key].Controls.Add(this.printerList[key]);

            this.mylarList[key].Location = new Point(142, 4);
            this.containerList[key].Controls.Add(this.mylarList[key]);

            this.paperList[key].Location = new Point(198, 4);
            this.containerList[key].Controls.Add(this.paperList[key]);

            this.otherList[key].Location = new Point(257, 4);
            this.containerList[key].Controls.Add(this.otherList[key]);

            this.ndtList[key].Location = new Point(2, 25);
            this.containerList[key].Controls.Add(this.ndtList[key]);
        }

        /// <summary>
        /// Runs when form is loaded. Loads all controllers and adds them to form based on what is in plot printer xml file.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ReadXML();

            int spacer = 41; // the amount of space between each container panel

            // interate through container panels, place them in group box, and place controllers inside.
            // if there are no container panels, nothing will happen.
            for (int key = 0; key < this.containerList.Count; key++)
            {
                PlaceInContainer(key);

                PlaceContainerPanel(key, spacer);

                // -------------------------top spacer + (spacer * # of container boxes) + bottom spacer
                this.paperStatGroupBox.Height = 15 + (spacer * this.containerList.Count) + 10;
            }

            // -------------------------------------keep same X location,     top of groupBox         +    height = bottom of box. + 20 for some space between box and button.
            this.buttonOk.Location = new Point(this.buttonOk.Location.X, this.paperStatGroupBox.Location.Y + this.paperStatGroupBox.Height + 20);
            this.buttonCancel.Location = new Point(this.buttonCancel.Location.X, this.paperStatGroupBox.Location.Y + this.paperStatGroupBox.Height + 20);


            ////label1.Text = DateTime.Now.Hour.ToString();
            ////label1.AutoSize = true;
        }

        /// <summary>
        /// When user clicks cancel button, close form.
        /// </summary>
        /// <param name="sender">Auto generated sender object by Visual Studio.</param>
        /// <param name="e">Auto generated EventArgs by Visual Studio.</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
