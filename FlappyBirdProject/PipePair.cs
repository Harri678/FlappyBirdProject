using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlappyBirdProject
{
	internal class PipePair
	{
		public Rectangle TopPipe { get; private set; }
		public Rectangle BottomPipe { get; private set; }
		public bool Scored { get; set; }
		public double speed = 6;

		public PipePair(Canvas canvas, Random random)
		{
			int gapHeight = 150;
			int topHeight = random.Next(50, 250);

			ImageBrush topBrush = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/pipe.png")),
				Stretch = Stretch.Fill,
				RelativeTransform = new RotateTransform(180, 0.5, 0.5)
			};

			TopPipe = new Rectangle
			{
				Width = 80,
				Height = topHeight,
				Fill = topBrush,
			};


			ImageBrush bottomBrush = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/pipe.png")),
				Stretch = Stretch.Fill
			};

			BottomPipe = new Rectangle
			{
				Width = 80,
				Height = canvas.Height - topHeight - gapHeight,
				Fill = bottomBrush,
			};

			Canvas.SetLeft(TopPipe, canvas.ActualWidth);
			Canvas.SetTop(TopPipe, 0);

			Canvas.SetLeft(BottomPipe, canvas.ActualWidth);
			Canvas.SetTop(BottomPipe, topHeight + gapHeight);

			canvas.Children.Add(TopPipe);
			canvas.Children.Add(BottomPipe);
		}

		public bool Passed(double birdX)
		{
			if(!Scored && Canvas.GetLeft(TopPipe) + TopPipe.Width < birdX) { 
				Scored = true;
				return true;
			}
			return false;
		}

		public void Move()
		{
			Canvas.SetLeft(TopPipe, Canvas.GetLeft(TopPipe) - speed);
			Canvas.SetLeft(BottomPipe, Canvas.GetLeft(BottomPipe) - speed);
		}


		public bool CheckCollision(Rect birdRect)
		{
			Rect topRect = GetRect(TopPipe);
			Rect bottomRect = GetRect(BottomPipe);

			return birdRect.IntersectsWith(topRect) ||
				   birdRect.IntersectsWith(bottomRect);
		}

		private Rect GetRect(Rectangle r)
		{
			return new Rect(
				Canvas.GetLeft(r),
				Canvas.GetTop(r),
				r.Width,
				r.Height
			);
		}

	}		
}
