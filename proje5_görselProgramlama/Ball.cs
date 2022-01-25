using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace topSektirme
{
    public class Ball
    {
        public int kord_x { get; set; }
        public int kord_y { get; set; }
        public int yon_bilgisi { get; set; }
        public SolidBrush renk { get; set; }

        public bool durum = true;
        public bool cezaPuani = false;
        public static int score = 0;
        public int move_x = 2;
        public int move_y = 2;
        public int ball_h = 25;
        public int ball_w = 25;
       
        public void Draw(Graphics grpe)
        {
            grpe.FillEllipse(renk, kord_x, kord_y, ball_h, ball_w);
        }

        public void Move(int P_location, Form form_m)
        {

            if (yon_bilgisi == 0)
            {
                kord_x += move_x;
                kord_y += move_y;
            }
            else if (yon_bilgisi != 0)
            {
                kord_x -= move_x;
                kord_y -= move_y;
            }
            if (kord_y < 0 & (kord_x > 235 & kord_x < 430))
            {
                score += 20;
                durum = false;
            }
            if (kord_y < 30 & !((kord_x > 237 & kord_x < 430)))
            {
                move_y = -move_y;
            }
            if (kord_y > form_m.ClientSize.Height - 75)
            {
                score -= 20;
                cezaPuani = true;
            }

            if (kord_y > form_m.ClientSize.Height - 140)
            {
                if (kord_x > P_location - 20 & kord_x < P_location + 172)
                {
                    score += 1;
                    move_y = -move_y;
                }
            }
            if (kord_x < 30 || kord_x >= form_m.ClientSize.Width - 50)
            {
                move_x = -move_x;
            }
        }

    }
}
