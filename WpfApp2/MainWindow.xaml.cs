using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String food;
        private Dictionary<String, int> food_count;
        private Stopwatch stopwatch;
        private bool eat;
        private DispatcherTimer timer;
        private DispatcherTimer animate_timer;
        private int animate;
        private int dream;

        // constants
        private const int movie = 0;
        private const int kitty = 1;
        private const int work = 2;
        private const int dnd = 3;
        private const int xmen = 4;
        private const int historical = 5;

        public MainWindow()
        {
            InitializeComponent();

            food = "fly";
            eat = false;
            animate = 0;
            dream = 4;

            food_count = new Dictionary<string, int>() {
                {"fly", 0 },
                {"brocoli", 0 },
                {"strawberry", 0 }
            };
            

            stopwatch = new Stopwatch();

            timer = new DispatcherTimer();
            animate_timer = new DispatcherTimer();

            Storyboard story_turtle_l = FindResource("story_turtle_left") as Storyboard;
            Storyboard story_turtle_r = FindResource("story_turtle_right") as Storyboard;
            Storyboard eat_left = FindResource("eat_left") as Storyboard;
            Storyboard eat_right = FindResource("eat_right") as Storyboard;
            
            story_turtle_l.Begin();
            story_turtle_l.Completed += new EventHandler(story_turtle_l_Completed);
            story_turtle_r.Completed += new EventHandler(story_turtle_r_Completed);
            eat_left.Completed += new EventHandler(eat_left_Completed);
            eat_right.Completed += new EventHandler(eat_right_Completed);
        }

        private void eat_right_Completed(object sender, EventArgs e)
        {
            turtle_think(sender, e);

        }

        private void turtle_think(object sender, EventArgs e)
        {

            Random rand = new Random();
            dream = rand.Next(0, 5);

            do_thought_bubble();

            timer.Tick += story_turtle_l_Completed;
            timer.Interval = System.TimeSpan.FromSeconds(4);
            timer.Start();
        }
        
        private void do_thought_bubble()
        {

            thought_bubble.Visibility = Visibility.Visible;
            
            switch (dream)
            {
                case movie:
                case work:
                    thought_bubble.Source = new BitmapImage(new Uri(@"images/" + dream + ".png", UriKind.Relative));
                    break;
                case xmen:
                    animate = -1;
                    animate_timer.Tick += animate_thought_bubble;
                    animate_timer.Interval = System.TimeSpan.FromMilliseconds(500);
                    animate_timer.Start();
                    break;
                default:
                    animate_timer.Tick += animate_thought_bubble;
                    animate_timer.Interval = System.TimeSpan.FromMilliseconds(500);
                    animate_timer.Start();
                    break;
            }
        }

        private void animate_thought_bubble(object sender, EventArgs e)
        {
            Debug.Write("\nANIMATED\n");
            if (animate ==0)
            {
                thought_bubble.Source = new BitmapImage(new Uri(@"images/" + dream + "_1.png", UriKind.Relative));
                animate = 1;

            }
            else if(animate == 1)
            {
                thought_bubble.Source = new BitmapImage(new Uri(@"images/" + dream + "_2.png", UriKind.Relative));
                animate = 0;

                if (dream == xmen)
                {
                    animate = -1;
                }
            }
            else if(animate == -1)
            {
                thought_bubble.Source = new BitmapImage(new Uri(@"images/" + dream + "_3.png", UriKind.Relative));
                animate = 0;
            }
        }

        private void eat_left_Completed(object sender, EventArgs e)
        {
            Storyboard eat_right = FindResource("eat_right") as Storyboard;

            Debug.Write("THINK");
            Image curr_food = grid.FindName(food + "_" + food_count[food]) as Image;
            grid.Children.Remove(curr_food);

            turtle.Source = new BitmapImage(new Uri(@"images/turtle_right.png", UriKind.Relative));
            turtle.Margin = new Thickness(0, 592, 865, -23); 
            eat_right.Begin();
        }

        private void story_turtle_l_Completed(object sender, EventArgs e)
        {
            thought_bubble.Visibility = Visibility.Hidden;
            if(timer.IsEnabled)
            {
                timer.Stop();
                animate_timer.Stop();
                thought_bubble.Source = new BitmapImage(new Uri(@"images/thought_bubble.png", UriKind.Relative));
                btn_feed.Source = new BitmapImage(new Uri(@"images/btn_green.png", UriKind.Relative));
                food = "brocoli";
                animate = 0;
                eat = false;

            }

            if (!eat)
            {
                turtle.Source = new BitmapImage(new Uri(@"images/turtle_right.png", UriKind.Relative));
                turtle.Width = 300;
                turtle.Height = 300;
                turtle.Margin = new Thickness(0, 592, 865, -23);

                Storyboard story_turtle_r = FindResource("story_turtle_right") as Storyboard;
                story_turtle_r.Begin();
            }
            else
            {
                Storyboard eat_left = FindResource("eat_left") as Storyboard;
                eat_left.Begin();
            }
        }

        private void story_turtle_r_Completed(object sender, EventArgs e)
        {
            turtle.Source = new BitmapImage(new Uri(@"images/turtle_left.png", UriKind.Relative));
            turtle.Width = 300;
            turtle.Height = 300;
            turtle.Margin = new Thickness(0, 592, 865, -23);

            Storyboard story_turtle_l = FindResource("story_turtle_left") as Storyboard;
            story_turtle_l.Begin();
        }

        private void btn_feed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!eat)
            {
                stopwatch.Start();
            }
            
        }

        private void btn_feed_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            story_turtle_l_Completed(sender, e);
        }

        private void btn_feed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            stopwatch.Stop();

            // check if the button has been held for two seconds
            if(stopwatch.ElapsedMilliseconds >= 1000)
            {
                eat = true;

                btn_feed.Source = new BitmapImage(new Uri(@"images/btn_gray.png", UriKind.Relative));

                // put food in bowl
                Image food_img = new Image();
                food_img.Source = new BitmapImage(new Uri(@"images/food_" + food + ".png", UriKind.Relative));

                food_img.HorizontalAlignment = HorizontalAlignment.Left;
                food_img.VerticalAlignment = VerticalAlignment.Bottom;

                // positioning
                int margin_left = 70;
                int margin_bottom = 280;
                
                food_img.Margin = new Thickness(margin_left,0,0,margin_bottom);

                // appearance
                food_img.Visibility = Visibility.Visible;
                food_img.Stretch = Stretch.None;

                food_count[food] += 1;
                food_img.Name = food + "_" + food_count[food];
                Debug.Write("\n\nFOOD: " + food_img.Name);

                // add to the grid
                grid.Children.Add(food_img);
                grid.RegisterName(food_img.Name, food_img);
            }

            // switch the 
            else
            {
                if (eat)
                {

                    btn_feed.Source = new BitmapImage(new Uri(@"images/btn_gray.png", UriKind.Relative));

                }
                else
                {
                    switch (food)
                    {
                        case "fly":
                            // change button source
                            btn_feed.Source = new BitmapImage(new Uri(@"images/btn_green.png", UriKind.Relative));

                            // change food flag
                            food = "brocoli";

                            break;
                        case "brocoli":
                            // change button source
                            btn_feed.Source = new BitmapImage(new Uri(@"images/btn_red.png", UriKind.Relative));

                            // change food flag
                            food = "strawberry";

                            // put food in bowl

                            break;
                        case "strawberry":
                            // change button source
                            btn_feed.Source = new BitmapImage(new Uri(@"images/btn_blue.png", UriKind.Relative));

                            // change food flag
                            food = "fly";

                            // put food in bowl

                            break;
                        default:
                            Debug.Write("\nDEFLT: yes");
                            break;
                    }
                }
            }


            stopwatch.Reset();
            Debug.Write("\nENDUP: yes");
            // click through the food

        }
    }
}
