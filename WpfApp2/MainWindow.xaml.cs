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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Media;
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
        private int direction;
        private bool eat;

        public MainWindow()
        {
            InitializeComponent();

            food = "fly";
            direction = -1;
            eat = false;

            food_count = new Dictionary<string, int>() {
                {"fly", 0 },
                {"brocoli", 0 },
                {"strawberry", 0 }
            };

            stopwatch = new Stopwatch();

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
            eat = false;

            turtle_think();

            story_turtle_l_Completed(sender, e);
        }

        private void turtle_think()
        {

        }

        private void eat_left_Completed(object sender, EventArgs e)
        {
            Storyboard eat_right = FindResource("eat_right") as Storyboard;

            eat_right.Begin();
        }

        private void story_turtle_l_Completed(object sender, EventArgs e)
        {
            if(!eat)
            {
                direction = 1;
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
            direction = -1;
            turtle.Source = new BitmapImage(new Uri(@"images/turtle_left.png", UriKind.Relative));
            turtle.Width = 300;
            turtle.Height = 300;
            turtle.Margin = new Thickness(0, 592, 865, -23);

            Storyboard story_turtle_l = FindResource("story_turtle_left") as Storyboard;
            story_turtle_l.Begin();
        }

        private void btn_feed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stopwatch.Start();
            
            Debug.Write("\nSTART: " + stopwatch.ElapsedMilliseconds);
        }

        private void btn_feed_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_feed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            stopwatch.Stop();

            // check if the button has been held for two seconds
            if(stopwatch.ElapsedMilliseconds >= 2000)
            {
                eat = true;

                // put food in bowl
                Image food_img = new Image();
                food_img.Source = new BitmapImage(new Uri(@"images/food_" + food + ".png", UriKind.Relative));

                food_img.HorizontalAlignment = HorizontalAlignment.Left;
                food_img.VerticalAlignment = VerticalAlignment.Bottom;

                // positioning
                int margin_left = 60 + ((food_count[food] % 15) * 10);
                int margin_bottom = 280;
                
                if (margin_left >= 200)
                {
                    margin_bottom += 10;
                    margin_left = 60;
                }
                food_img.Margin = new Thickness(margin_left,0,0,margin_bottom);

                // appearance
                food_img.Visibility = Visibility.Visible;
                food_img.Stretch = Stretch.None;

                food_count[food] += 1;
                food_img.Name = food + "_" + food_count[food];

                // add to the grid
                grid.Children.Add(food_img);
            }

            // switch the 
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


            stopwatch.Reset();
            Debug.Write("\nENDUP: yes");
            // click through the food

        }
    }
}
