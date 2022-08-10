using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Image chessSprites;
        public bool isChess960;
        public NumericUpDown numeric;
        public int[,] map = new int[8, 8]
        {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
        };

        public Button[,] butts = new Button[8, 8];

        public int currPlayer;

        public Button prevButton;

        public bool isMoving = false;

        public Form1()
        {
            InitializeComponent();

            // --------- put your path and comment others out-------
            //chessSprites = new Bitmap("C:\\Users\\sodrk\\Desktop\\chess.png");
            //chessSprites = new Bitmap("C:\\Users\\Justin\\OneDrive\\school\\year 2\\Software in existing code\\GroupChessProject\\Chess_960\\ChessGame-master\\Chess\\Sprites\\chess.png");
            chessSprites = new Bitmap("C://Users//javanderniet//Documents//GitHub//Chess_960//ChessGame-master//Chess//Sprites//chess.png");

            //button1.BackgroundImage = part;

            // get seed input
            numeric = (NumericUpDown)this.Controls.Find("numUpDown", true)[0];

            Init();
        }

        public int[,] GenerateGrid(string positions)
        {
            // generate grid object
            int[,] grid = null;

            // create skeleton grid with pawns
            grid = new int[8, 8]
            {
            {0,0,0,0,0,0,0,0},
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 }, // Note that the 10's are white,
            {0,0,0,0,0,0,0,0 }, // and the 20's are black.
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {0,0,0,0,0,0,0,0},
            };

            // for each letter in position string
            for (int i = 0; i < positions.Length; i++)
            {
                // get current positions letter
                char p = positions[i];
                // place respective black and white piece
                switch (p)
                {
                    case 'r': // rook
                        grid[0, i] = 15;
                        grid[7, i] = 25;
                        break;
                    case 'n': // knight
                        grid[0, i] = 14;
                        grid[7, i] = 24;
                        break;
                    case 'b': // bishop
                        grid[0, i] = 13;
                        grid[7, i] = 23;
                        break;
                    case 'q': // queen
                        grid[0, i] = 12;
                        grid[7, i] = 22;
                        break;
                    case 'k': // king
                        grid[0, i] = 11;
                        grid[7, i] = 21;
                        break;
                }
            }
            
            // return int[,]
            return grid;
        }

        public string Chess960Positions(int seed)
        {
            // soon to by modified string representing light and dark tiles
            StringBuilder results = new StringBuilder("________"); 
            //                                         abcdefgh
            //                                         01234567
            // validate seed
            if (seed < 0 || seed > 960)
            {
                // return default layout
                return "rnbqkbnr";
            } else if (seed == 960)
            {
                // generate a random number from 0 - 959
                seed = new Random().Next(seed);
            }

            // used for setting location of specific pieces
            int remainder;

            // first calculation for light bishop
            remainder = seed % 4;
            seed = seed / 4;
            results[(remainder * 2) + 1] = 'b'; // 0 = b, 1 = d, 2 = f, 3 = h
            Console.WriteLine("first: " + seed + "r" + remainder);
            
            // second calculation for dark bishup
            remainder = seed % 4;
            seed = seed / 4;
            results[remainder * 2] = 'b'; // 0 = a, 1 = c, 2 = e, 3 = g
            Console.WriteLine("second: " + seed + "r" + remainder);

            // third calculation for queen
            remainder = seed % 6;
            seed = seed / 6;
            Console.WriteLine("third: " + seed + "r" + remainder);

            // track leftover indexes
            List<int> index_left = new List<int>();
            // get remaining spaces
            for (int i = 0; i < results.Length; i++)
            {
                // for each char that is blank
                if (results[i] == '_')
                {
                    // add to leftover indexes
                    index_left.Add(i);
                }
            }

            // add queen and remove index from tracker
            results[index_left[remainder]] = 'q';
            index_left.RemoveAt(remainder);

            // get order for last characters
            string KRNcode = GetKernCode(seed);

            // for each character, set in next available index then remove index from list
            foreach (char c in KRNcode)
            {
                results[index_left[0]] = c;
                index_left.RemoveAt(0);
            }

            return results.ToString();
        }

        public string GetKernCode(int seed)
        {
            string kern = "";
            switch (seed)
            {
                case 0:
                    kern = "nnrkr";
                    break;
                case 1:
                    kern = "nrnkr";
                    break;
                case 2:
                    kern = "nrknr";
                    break;
                case 3:
                    kern = "nrkrn";
                    break;
                case 4:
                    kern = "rnnkr";
                    break;
                case 5:
                    kern = "rnknr";
                    break;
                case 6:
                    kern = "rnkrn";
                    break;
                case 7:
                    kern = "rknnr";
                    break;
                case 8:
                    kern = "rknrn";
                    break;
                case 9:
                    kern = "rkrnn";
                    break;
                default:
                    // this is for debuggin, if you get a layout with 5 knights, something went wrong.
                    kern = "nnnnn";
                    break;
            }
            return kern;
        }

        public void Init()
        {
            // default chess
            string positions = "rnbqkbnr";

            // if user has selected to play chess 960
            if (isChess960)
            {
                // get seed from input
                int seed = (int)numeric.Value;

                // generate and overwrite original setup
                positions = Chess960Positions(seed);
            }

            // apply new layout
            map = GenerateGrid(positions);

            currPlayer = 1;
            CreateMap();
        }

        public void CreateMap()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();

                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j*50,i*50);

                    switch (map[i, j]/10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0+150*(map[i,j]%10-1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10-1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);

                    butts[i, j] = butt;
                }
            }
        }
        
        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = Color.White;

            Button pressedButton = sender as Button;

            //pressedButton.Enabled = false;
            
            if (map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] != 0 && map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]/10 == currPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                ShowSteps(pressedButton.Location.Y / 50, pressedButton.Location.X / 50, map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = Color.White;
                    ActivateAllButtons();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }else
            {
                if (isMoving)
                {
                    int temp = map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50];
                    map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] = map[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                    map[prevButton.Location.Y / 50, prevButton.Location.X / 50] = temp;
                    pressedButton.BackgroundImage = prevButton.BackgroundImage;
                    prevButton.BackgroundImage = null;
                    isMoving = false;
                    CloseSteps();
                    ActivateAllButtons();
                    SwitchPlayer();
                }
            }
           
            prevButton = pressedButton;
        }

        public void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            switch (currFigure%10)
            {
                case 6:
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                        }
                    }
                    
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure+1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].Enabled = true;
                        }
                    }
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure - 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure - 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure - 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].Enabled = true;
                        }
                    }
                    break;
                case 5:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    break;
                case 3:
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 2:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 1:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure,true);
                    ShowDiagonal(IcurrFigure, JcurrFigure,true);
                    break;
                case 4:
                    ShowHorseSteps(IcurrFigure, JcurrFigure);
                    break;
            }
        }

        public void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
        {
            if (InsideBorder(IcurrFigure - 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure - 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure +1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure -2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure +1, JcurrFigure -2);
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure,bool isOneStep=false)
        {
            int j = JcurrFigure + 1;
            for(int i = IcurrFigure-1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j <7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        public void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure,bool isOneStep=false)
        {
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure + 1; j < 8; j++)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int IcurrFigure,int j)
        {
            if (map[IcurrFigure, j] == 0)
            {
                butts[IcurrFigure, j].BackColor = Color.Yellow;
                butts[IcurrFigure, j].Enabled = true;
            }
            else
            {
                if (map[IcurrFigure, j] / 10 != currPlayer)
                {
                    butts[IcurrFigure, j].BackColor = Color.Yellow;
                    butts[IcurrFigure, j].Enabled = true;
                }
                return false;
            }
            return true;
        }

        public bool InsideBorder(int ti,int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;
            return true;
        }

        public void CloseSteps()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.White;
                }
            }
        }

        public void SwitchPlayer()
        {
            if (currPlayer == 1)
                currPlayer = 2;
            else currPlayer = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // this.Controls.Clear(); old restart would wipe the entire form out and just create new board, no other controls
            // now, loops through and grabs only buttons and clears them one by one.
            foreach (Button button in butts)
            {
                this.Controls.Remove(button);
            }
            // then restart
            Init();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // checked = true, unchecked = false
            isChess960 = checkBox1.Checked;
        }
    }
}
