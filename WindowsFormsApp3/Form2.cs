using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using WindowsFormsApp3.Properties;
using System.Text.RegularExpressions;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        int dificulty;
        public Form2(int dificulty)
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.dificulty = dificulty;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        PictureBox[] images = new PictureBox[8] , images_2 = new PictureBox[8];
        Image spate = (Image)Properties.Resources.spate;
        bool clicked1=false,clicked2=false;
        string ima1 = null, ima2 = null;
        int score = 0, viata = 3 ,curr_1 = 0, curr_2= 0,sc=0;
        Label score_l = new Label(), viata_l = new Label();

        public class LabelCreator
        {
            public Label CreateLabel(string labelText, int positionX, int positionY)
            {
                Label newLabel = new Label();
                newLabel.Text = labelText;
                newLabel.Location = new System.Drawing.Point(positionX, positionY);
                newLabel.BackColor = Color.Transparent;
                return newLabel;
            }
        }
        public Label Afis(string scop)
        {
                LabelCreator labelCreator = new LabelCreator();
                Label myLabel = labelCreator.CreateLabel("", 100, this.Size.Height-100);
            if(scop == "life") {
                myLabel.Name = "life";
                myLabel.Text = "life: " + viata;
                myLabel.Size = new Size(120, 50);
                myLabel.Font = new Font("Arial", 20);
                this.Controls.Add(myLabel);
            }
            else
            {
                myLabel.Location = new Point(this.Size.Width - 200, this.Size.Height - 100);
                myLabel.Name = "score";
                myLabel.Text = "score: " + score;
                myLabel.Size = new Size(120, 50);
                myLabel.Font = new Font("Arial", 20);
                this.Controls.Add(myLabel);
            }
            return myLabel;
        }
        private void init_score()
        {
        score_l = Afis("score");
        viata_l = Afis("life");
        }
        private void Hide(int i , int cate)
        {
            try
            {
                if (cate == 1)
                {
                    images[i].Image.Dispose();
                    images[i].Image = spate;    //// inlocuire imagine
                }
                else
                {
                    images_2[i].Image.Dispose();
                    images_2[i].Image = spate;
                }
            }
            catch
            {
                MessageBox.Show("error");
                Application.Restart();
            }
        }
        private int Hide_Check(string ima)
        {
            if (ima[ima.Length-1] == 'd')
                return 2;
            return 1;
        }
        private async void Timer1(int time)
        {
            int f_time = time;
            /*create timer*/

            LabelCreator labelCreator = new LabelCreator();
            Label myLabel = labelCreator.CreateLabel("", this.Size.Width/3, this.Size.Height/2);
            myLabel.Size = new Size(120, 60);
            myLabel.Font = new Font("Segoe Script", 40);
            this.Controls.Add(myLabel);

            //..
            //use timer 

            for (int i = 0; i < time; i++)
            {
                myLabel.Text = Convert.ToString(f_time - i);
                await Task.Delay(1000);

            }
            //..
            //delete timer

            myLabel.Dispose();
        }
        private async void pictureBox_Click(object sender, EventArgs e) //picture click event handler
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            Image myImage = null;
            try
            {
            if (clickedPictureBox.Name[clickedPictureBox.Name.Length-1]!='d')
                myImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(clickedPictureBox.Name);
            else
                myImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(clickedPictureBox.Name.Remove(clickedPictureBox.Name.Length-1));
            clickedPictureBox.Image = myImage;

            }
            catch { MessageBox.Show("Error occured during image processing","",MessageBoxButtons.OK, MessageBoxIcon.Error); 
            
            }
            if (!clicked1)
            {
                clicked1 = true;
                // show_image(); // to be done 
                ima1 = clickedPictureBox.Name;
                Match match = Regex.Match(ima1, @"\d+");
                if (match.Success)
                {
                    // If a match was found, parse it to an integer
                    curr_1 = int.Parse(match.Value)-1;
                }
            }
            else
            {
                if (!clicked2 && clickedPictureBox.Name!=ima1)
                {
                                                                            // show_image() // to be done
                ima2 = clickedPictureBox.Name;
                Match match = Regex.Match(ima2, @"\d+");
                if (match.Success)
                {
                    // If a match was found, parse it to an integer
                    curr_2 = int.Parse(match.Value)-1;
                }
                    await Task.Delay(800);

                    if ((ima1 + "d" == ima2) || (ima1 == ima2 + "d"))
                    {
                        score++;
                        images[curr_1].Image.Dispose();
                        images_2[curr_2].Image.Dispose();
                        Controls[ima1].Dispose();                       //    dump_imagini
                        Controls[ima2].Dispose();
                        this.Invoke((MethodInvoker)delegate {
                            score_l.Text = "score: " + score;
                            viata_l.Text = "life: " + viata;
                        });
                    }
                    else
                    {
                        viata--;
                        Hide(curr_2, Hide_Check(ima2));                 //   hide_imagini()
                        Hide(curr_1, Hide_Check(ima1));
                        curr_2 = 0;
                        curr_1 = 0;
                        await Task.Delay(50);
                        this.Invoke((MethodInvoker)delegate {
                            score_l.Text = "score: " + score;
                            viata_l.Text = "life: " + viata;
                        });
                    }
                clicked1 = false;
                }
            }
            if(viata == 0)
            {
                MessageBox.Show("You Lost");
                Application.Restart();
            }
            if(score==sc)
            {
                MessageBox.Show("You Win");
                Application.Restart();
            }
        }
        private void Create_Pictures(int dif , ref int x , ref int y)
        {
            int pictureWidth = 110, pictureHeight = 110;
            int spacing = 10; // Space between PictureBoxes

            try
            {

            for (int i = 0; i < dif; i++)
            {
                    images[i] = new PictureBox();
                    var myImage = (Bitmap)Properties.Resources.ResourceManager.GetObject($"cat_{i + 1}");
                    if (myImage != null)
                    {
                        images[i].Image = myImage;
                    }
                    images[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    images[i].Name = "cat_" + (i + 1);
                    images[i].Size = new System.Drawing.Size(pictureWidth, pictureHeight);
                    images[i].BackColor = Color.Transparent;
                    // Calculate the location of the PictureBox
                    int row = 0;
                    int column = 0;
                if (dificulty == 3)
                    {
                        row = 1; 
                        column = i % 3;
                    }
                    else
                    {
                        if (dificulty == 4)
                        {
                        row = i / 2;
                        column = i % 2;
                        }
                        else
                        {
                            row = i / 3;
                            column = i % 3;
                        }

                    }
                int xPos = x + column * (pictureWidth + spacing);
                int yPos = y + row * (pictureHeight + spacing);

                images[i].Location = new System.Drawing.Point(xPos, yPos);
            }
            foreach (PictureBox pictureBox in images)
            {
                Controls.Add(pictureBox);
            }
            for (int i = 1; i <= dif; i++)
            {
                // Find the PictureBox by name
                PictureBox pb = this.Controls.Find("cat_" + i, true).FirstOrDefault() as PictureBox;

                if (pb != null)
                {
                    // Add the Click event handler
                    pb.Click += new System.EventHandler(this.pictureBox_Click);
                }
            }
            }
            catch {
                MessageBox.Show("Error occured during image processing", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void Create_Pictures_2(int dif, ref int x, ref int y)
        {
            int pictureWidth = 110, pictureHeight = 110;
            int spacing = 10; // Space between PictureBoxes

            try
            {

                for (int i = 0; i < dif; i++)
                {
                    images_2[i] = new PictureBox();
                    // Use the same image as in Create_Pictures
                    Image myImage = images[i].Image;
                    images_2[i].Image = myImage;
                    images_2[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    images_2[i].Name = "cat_" + (i + 1) + "d";
                    images_2[i].Size = new System.Drawing.Size(pictureWidth, pictureHeight);
                    images_2[i].BackColor = Color.Transparent;
                    // Calculate the location of the PictureBox
                    int row = 0;
                    int column = 0;
                    if (dificulty == 3)
                    {
                        row = 0;
                        column = i % 3;
                    }
                    else
                    {
                        if (dificulty == 4)
                        {
                            row = i / 2;
                            column = i % 2;
                        }
                        else
                        {
                            row = i / 3;
                            column = i % 3;
                        }

                    }
                    int xPos = x + column * (pictureWidth + spacing);
                    int yPos = y + row * (pictureHeight + spacing);

                    images_2[i].Location = new System.Drawing.Point(xPos, yPos);
                }
                foreach (PictureBox pictureBox in images_2)
                {
                    Controls.Add(pictureBox);
                }
                for (int i = 1; i <= dif; i++)
                {
                    // Find the PictureBox by name
                    PictureBox pb = this.Controls.Find("cat_" + i + "d", true).FirstOrDefault() as PictureBox;

                    if (pb != null)
                    {
                        // Add the Click event handler
                        pb.Click += new System.EventHandler(this.pictureBox_Click);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred during the image processing: " + ex.Message);
                Application.Exit();
            }
        }
        public async void Start(int dif)
        {
            init_score();
            sc = dif;
            int x = 10, y = 10;
            Create_Pictures(dif, ref x, ref y);
            if(dificulty>3)
             x = dificulty/2*110 + (dificulty / 2 + 1) * 10;
            Create_Pictures_2(dif, ref x, ref y);
            if(dificulty<=4)
                await Task.Delay(2500);
            else
                await Task.Delay(dificulty*500);

            for (int i = 0; i < dif; i++)
            {
                Hide(i, 1);
                Hide(i, 2);
            }

        }
        private async void Form2_Load(object sender, EventArgs e)
        {
            Timer1(3);
            await Task.Delay(3050);
            Start(dificulty);  // adjust dificulty
        }
    }
}
