using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SCD04HELICOPTERGAME
{
    public partial class Helicopter_Game : Form
    {
        bool up, down;
        int scor;
        SoundPlayer backgroundmusic;
        SoundPlayer crashsound;
        bool bgplaying;
        private int highestScore = Properties.Settings.Default.highest_score;




        public Helicopter_Game()
        {
            InitializeComponent();
            lbl_over.Visible = false;
            MXP.URL = @"bg_music_mediaplayer.mp3";
            MXP.settings.playCount = 999;
            MXP.Ctlcontrols.stop();
            MXP.Visible = false;
            lbl_highest.Text = "Highest Score: "+highestScore;
        }
        void Game_Over()
        {
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && x.Tag == "walls")
                {
                    foreach (Control y in this.Controls)
                    {
                        if (y is PictureBox && y.Tag == "pillars")
                        {
                            foreach (Control z in this.Controls)
                            {
                                if (z is PictureBox && z.Tag == "enemies")
                                {
                                    if(player.Bounds.IntersectsWith(x.Bounds)|| player.Bounds.IntersectsWith(y.Bounds)|| player.Bounds.IntersectsWith(z.Bounds))
                                    {
                                        CrashSound();
                                        lbl_over.Visible = true;
                                        timer1.Stop();
                                        bgplaying = false;
                                        MXP.Ctlcontrols.stop();
                                        if (scor > highestScore)
                                        {
                                            lbl_highest.Text = "Highest Score: " + scor;
                                            Properties.Settings.Default.highest_score = scor;
                                            Properties.Settings.Default.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void smoke()
        {
            PictureBox smoke = new PictureBox();
            smoke.BackColor = System.Drawing.Color.Aqua;
            smoke.Width = 3;
            smoke.Height = 3;
            smoke.Top = player.Top + player.Height / 2;
            smoke.Left = player.Left + player.Width - 35;
            smoke.Tag = "smoke";
            this.Controls.Add(smoke);
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && x.Tag == "smoke")
                {
                    x.Left -= 10;
                    if (x.Left < 0)
                    {
                        this.Controls.Remove(x);
                        x.Dispose();
                    }
                }
            }
        }
        private void enemies_move()
        {
            Random rand = new Random();
            int x;
            foreach(Control j in this.Controls)
            {
                if(j is PictureBox && j.Tag == "enemies")
                {
                    j.Left -= 5;
                    if (j.Left < 0)
                    {
                        x = rand.Next(80, 300);
                        j.Location = new Point(800, x);
                        scor++;
                        lbl_score.Text = "Score: " + scor;
                    }
                }
            }
        }
        private void pillars_move()
        {
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && x.Tag== "pillars")
                {
                    x.Left -= 5;
                    if (x.Left < -350)
                    {
                        x.Left = 700;
                    }
                }
            }
        }
        private void player_move()
        {
            if (up == true)
            {
                player.Top -= 5;
            }
            if (down == true)
            {
                player.Top += 5;
            }
        }

        private void Helicopter_Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                up = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                down = true;
            }
        }

        private void Helicopter_Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                up = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                down = false;
            }
        }
        private void BackgroundMusic()
        {
            backgroundmusic = new SoundPlayer(@"bg_music.wav");
            backgroundmusic.PlayLooping();

            bgplaying = true;
        }
        private void CrashSound()
        {
            crashsound = new SoundPlayer(@"crashsound.wav");
            crashsound.Play();
        }

        private void Helicopter_Game_Load(object sender, EventArgs e)
        {
            BackgroundMusic();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            player_move();
            pillars_move();
            enemies_move();
            smoke();
            Game_Over();

        }

    }
}
