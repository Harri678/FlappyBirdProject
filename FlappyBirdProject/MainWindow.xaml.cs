using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlappyBirdProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private DispatcherTimer gameTimer = new DispatcherTimer();
		private double birdVelocity = 0;
		private double gravity = 0.5;
		private double jumpStrenght = -8;
		private bool gameOver = false;
		public MainWindow()
        {
            InitializeComponent();

			gameTimer.Interval = TimeSpan.FromMilliseconds(20);
			gameTimer.Tick += GameLoop;
			gameTimer.Start();

		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!gameOver && e.Key == Key.Space)
			{
				birdVelocity = jumpStrenght;
			}
		}

		private void GameLoop(object sender, EventArgs e)
		{
			birdVelocity += gravity;
			double newTop = Canvas.GetTop(Bird) + birdVelocity;
			Canvas.SetTop(Bird, newTop);

		}
	}
}