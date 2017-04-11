using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormXMLTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ReadXML();
        }

        private Dictionary<int, Panel> colorBoxList = new Dictionary<int, Panel>();
        private Dictionary<int, Label> printerList = new Dictionary<int, Label>();
        private Dictionary<int, RadioButton> mylarList = new Dictionary<int, RadioButton>();
        private Dictionary<int, RadioButton> paperList = new Dictionary<int, RadioButton>();
        private Dictionary<int, RadioButton> otherList = new Dictionary<int, RadioButton>();
        private Dictionary<int, Label> ndtList = new Dictionary<int, Label>();


        private void ReadXML()
        {

            int key = 0;
            ///////////////////////////////////////////////////////////////////////               Change to new file location              //////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            using (XmlReader reader = XmlReader.Create(@"C:\Users\wgoldsmith\Documents\Visual Studio 2015\Projects\Plot-Command-CS\Plot-Command-CS\plotprinters.xml"))
            { 

                while (reader.Read())
                {                
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name.Equals("printer"))
                            {
                                    
                                printerList.Add(key, new Label());// reader.ReadString());
                                printerList[key].Text = reader.GetAttribute("name");
                                printerList[key].Location = new Point(5, 56);

                                printerList[key].AutoSize = true;
                                printerList[key].Name = "label1";
                                    
                            }

                            else if(reader.Name.Equals("user"))
                            {
                                ndtList.Add(key, new Label());
                                ndtList[key].Text = reader.ReadElementContentAsString();
                                ndtList[key].Location = new Point(150, 100);

                                ndtList[key].AutoSize = true;
                                ndtList[key].Name = "label1";
                            }
                                
                            break;

                        case XmlNodeType.EndElement:  //  when it reaches the end of a printer, increase key by 1
                            if (reader.Name.Equals("printer"))
                                key++;

                            break;

                    }
                    
                }
            }

            paperStatGroupBox.Height = 41 * key + 15;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label temp = new Label();

            temp.Location = new Point(5, 15);
            temp.Name = "temp";
            temp.TabIndex = 1;
            temp.Text = "tempttt";// + printerList[1].Text;
            if (ndtList.ContainsKey(1))
                temp.Text = ">>>" + ndtList[1].Text + "<<<";
           
            temp.AutoSize = true;

            paperStatGroupBox.Controls.Add(temp);
            paperStatGroupBox.Controls.Add(printerList[0]);

            //                           keep same X location,     top of groupBox + height = bottom of box. + 20 for some space between box and button.
            buttonOk.Location = new Point(buttonOk.Location.X, paperStatGroupBox.Location.Y + paperStatGroupBox.Height + 20);
            buttonCancel.Location = new Point(buttonCancel.Location.X, paperStatGroupBox.Location.Y + paperStatGroupBox.Height + 20);



            label1.Text = DateTime.Now.ToShortTimeString();
            label1.AutoSize = true;
        }
    }
}
