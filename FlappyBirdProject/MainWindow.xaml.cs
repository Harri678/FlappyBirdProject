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
using WpfAnimatedGif;

namespace FlappyBirdProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private DispatcherTimer gameTimer = new DispatcherTimer();
		private double birdVelocity = 0;
		private double gravity = 0.9;
		private double jumpStrenght = -10;
		private bool gameOver = false;

		private double normalJump = -10;
		private double rainJump = -5;

		private bool fogEnabled = false;

		bool fogActive = false;
		int fogTimer = 0;

		int fogOnTime = 300;
		int fogOffTime = 100;



		List<PipePair> pipes = new List<PipePair>();
		Random random = new Random();
		int pipeCounter = 50;
		int score = 0;

		public MainWindow()
        {
            InitializeComponent();

			var gif = new BitmapImage();
			gif.BeginInit();
			gif.UriSource = new Uri("pack://application:,,,/Images/RainGif.gif");
			gif.CacheOption = BitmapCacheOption.OnLoad;
			gif.EndInit();

			ImageBehavior.SetAnimatedSource(RainLayer, gif);

			gameTimer.Interval = TimeSpan.FromMilliseconds(20);
			gameTimer.Tick += GameLoop;
			//gameTimer.Start();


		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!gameOver && e.Key == Key.Space)
			{
				birdVelocity = jumpStrenght;
			}
		}

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			MenuPanel.Visibility = Visibility.Hidden;
			Canvas.SetLeft(Bird, 100);
			Canvas.SetTop(Bird, 200);

			gameTimer.Start();
		}

		//Eső
		private void RainToggle_Checked(object sender, RoutedEventArgs e)
		{
			RainLayer.Visibility = Visibility.Visible;
			jumpStrenght = rainJump;
		}

		private void RainToggle_Unchecked(object sender, RoutedEventArgs e)
		{
			RainLayer.Visibility = Visibility.Hidden;
			jumpStrenght = normalJump;
		}

		//Köd
		private void FogToggle_Checked(object sender, RoutedEventArgs e)
		{
			fogEnabled = true;
			fogActive = true;
			FogLayer.Visibility = Visibility.Visible;
		}

		private void FogToggle_Unchecked(object sender, RoutedEventArgs e)
		{
			fogEnabled = false;
			FogLayer.Visibility = Visibility.Hidden;
		}


		//Game loop
		private void GameLoop(object sender, EventArgs e)
		{
			birdVelocity += gravity;
			double newTop = Canvas.GetTop(Bird) + birdVelocity;
			Canvas.SetTop(Bird, newTop);

			pipeCounter++;

			if (pipeCounter > 50)
			{
				pipes.Add(new PipePair(GameCanvas, random));
				pipeCounter = 0;
			}

				Rect birdRect = GetBirdRect();
				foreach (var pipe in pipes)
				{
					pipe.Move();

					if (pipe.CheckCollision(birdRect))
					{
						EndGame();
						return;
					}
					if (pipe.Passed(Canvas.GetLeft(Bird)))
					{
						score++;
					}
				}

				Rect groundRect = GetGroundRect();
				if (birdRect.IntersectsWith(groundRect))
				{
					EndGame();
					return;
				}

			if (fogEnabled)
			{
				fogTimer++;

				if (!fogActive && fogTimer > fogOffTime)
				{
					fogActive = true;
					FogLayer.Visibility = Visibility.Visible;
					fogTimer = 0;
				}
				else if (fogActive && fogTimer > fogOnTime)
				{
					fogActive = false;
					FogLayer.Visibility = Visibility.Hidden;
					fogTimer = 0;
				}
			}
		}

		private void EndGame()
		{
			gameTimer.Stop();
			gameOver = true;

			ScoreText.Text = $"Score: {score}";
			GameOverPanel.Visibility = Visibility.Visible;
		}

		private void Restart_Click(object sender, RoutedEventArgs e)
		{
			GameOverPanel.Visibility = Visibility.Hidden;
			MenuPanel.Visibility = Visibility.Visible;
			Canvas.SetLeft(Bird, 100);
			Canvas.SetTop(Bird, 200);
			birdVelocity = 0;

			foreach (var pipe in pipes)
			{
				GameCanvas.Children.Remove(pipe.TopPipe);
				GameCanvas.Children.Remove(pipe.BottomPipe);
			}
			pipes.Clear();

			score = 0;
			pipeCounter = 0;

			gameOver = false; 
		}

		private Rect GetBirdRect()
		{
			double collisionBox = 30;
			double offsetX = (Bird.Width - collisionBox) / 2;
			double offsetY = (Bird.Height - collisionBox) / 2;


			return new Rect(
				Canvas.GetLeft(Bird) + offsetX,
				Canvas.GetTop(Bird) + offsetY,
				collisionBox,
				collisionBox
			);
		}

		private Rect GetGroundRect()
		{
			double left = Canvas.GetLeft(GroundImage);
			double top = Canvas.GetTop(GroundImage);
			double width = GroundImage.Width;
			double height = GroundImage.Height;

			return new Rect(left, top, width, height);
		}
	}
}