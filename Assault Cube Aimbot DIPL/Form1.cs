using System.Runtime.InteropServices;
using System.Threading;
using ezOverLay;

namespace Assault_Cube_Aimbot_DIPL
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        Functions? funcs;
        Entity localPlayer = new Entity();
        List<Entity> entities = new List<Entity>();

        ez ezOverlay = new ez();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            funcs = new Functions();
            if(funcs != null)
            {
                /*ez overlay enables ESP hack to work
                by making our forms window invisible
                and it moves the form window wherever
                the selected process window is and 
                then draws to the invisible forms window*/

                //Make the form invisible
                ezOverlay.SetInvi(this);
                ezOverlay.DoStuff("AssaultCube", this);

                //If main app is closed, close this app too
                Thread t = new Thread(MainLoop) { IsBackground = true};
                t.Start();
            }
        }

        void MainLoop()
        {
            while (true)
            {
                //Find my local player info
                localPlayer = funcs.ReadLocalPlayer();
                //Local player parameter needed to calculate magnitude to other entities
                entities = funcs.ReadMultipleEntities(localPlayer);

                //Closest entity is first
                entities = entities.OrderBy(x => x.magnitude).ToList();

                //Left CTRL key, can be set to personal preference
                if (GetAsyncKeyState(Keys.ControlKey) < 0)
                {
                    if (entities.Count > 0)
                    {
                        foreach (var ent in entities)
                        {
                            if (ent.team != localPlayer.team)
                            {
                                //Calculate angle between local player and closest entity and aim at them
                                var angles = funcs.CalculateAngles(localPlayer, ent);
                                funcs.DoAiming(localPlayer, angles.X, angles.Y);
                                break;
                            }
                        }
                    }
                }

                Form1 form = this;
                form.Refresh();

                Thread.Sleep(10);
            }
            
        }

        //Method where we draw our ESP hack
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen red = new Pen(Color.Red, 3);
            Pen green = new Pen(Color.Green, 3);
            
            foreach(var ent in entities.ToList())
            {
                //2D coordinates of entity heads and feet
                //wts = world to screen
                var wtsFeet = funcs.WorldToScreen(funcs.ReadMatrix(), ent.feet, this.Width, this.Height);
                var wtsHead = funcs.WorldToScreen(funcs.ReadMatrix(), ent.head, this.Width, this.Height);

                if(wtsFeet.X > 0)
                {
                    //Green for allies, red for enemies
                    if(localPlayer.team == ent.team)
                    {
                        //Draw line from bottom middle of screen to entity feet
                        g.DrawLine(green, new Point(Width / 2, Height), wtsFeet);
                        g.DrawRectangle(green, funcs.CalculateRect(wtsFeet, wtsHead));
                    }
                    else
                    {
                        g.DrawLine(red, new Point(Width / 2, Height), wtsFeet);
                        g.DrawRectangle(red, funcs.CalculateRect(wtsFeet, wtsHead));
                    }
                    

                }

            }
        }
    }
}