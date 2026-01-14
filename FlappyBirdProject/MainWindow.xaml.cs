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
		private double gravity = 0.9;
		private double jumpStrenght = -10;
		private bool gameOver = false;

		List<PipePair> pipes = new List<PipePair>();
		Random random = new Random();
		int pipeCounter = 50;
		int score = 0;

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

			pipeCounter++;

			if (pipeCounter > 50)
			{
				pipes.Add(new PipePair(GameCanvas, random));
				pipeCounter = 0;
			}

			Rect birdRect = GetBirdRect();
			foreach (var pipe in pipes){
				pipe.Move();

				if(pipe.CheckCollision(birdRect))
				{
					EndGame();
					return;
				}
				if(pipe.Passed(Canvas.GetLeft(Bird)))
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
		}

		private void EndGame()
		{
			gameTimer.Stop();
			gameOver = true;
			MessageBox.Show("Game Over! Your score: " + score);
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