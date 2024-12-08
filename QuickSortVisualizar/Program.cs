// See https://aka.ms/new-console-template for more information
using SFML.Graphics;
using SFML.System;
using SFML.Window;

Console.WriteLine("Hello, World!");



const int screenWidth = 800;
const int screenHeight = 600;

var window = new RenderWindow(new VideoMode((uint)screenWidth, (uint)screenHeight), "Sorting Animation in SFML.Net");
window.Closed += (sender, e) => window.Close();

const int numberCount = screenWidth / 10;

// Generate random numbers
var random = new Random();
var randomNumbers = new List<int>();
for (int i = 0; i < numberCount; i++)
{
    randomNumbers.Add(random.Next(0, screenHeight));
}

// Generate animation frames
var frames = new List<List<int>>();
QuickSortAnimation(randomNumbers, 0, randomNumbers.Count - 1, frames);

int currentFrame = 0;

var transition = frames.Count * frames.First().Count;
var steps = 2;



while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear(Color.Black);

    int counter = 0;
    var total = 50;
    foreach (var number in frames[currentFrame])
    {
        float xPos = counter * 10.0f;

        // Generate a dynamic color based on the current frame
        byte red = (byte)((currentFrame * 50 + total) % 256);
        byte green = (byte)((currentFrame * 30) % 256);
        byte blue = (byte)((currentFrame * 80 + counter * 5) % 256);
        var curColor = new Color(red, green, blue, (byte)255);

        DrawBar(window, number, 10.0f, curColor, xPos, 0.0f);
        counter++;
        total += 5;
    }

    // Increment frame unless it's the last one
    if (currentFrame < frames.Count - 1)
    {
        Thread.Sleep(100); // Pause for visualization
        currentFrame++;
    }

    window.Display();
}



static void DrawBar(RenderWindow window, float height, float width, Color color, float x, float y)
{
    var borderWidth = 3.0f;

    // Create a rectangle with the specified dimensions
    var bar = new RectangleShape(new Vector2f(width - borderWidth, height))
    {
        Position = new Vector2f(x + borderWidth, y),
        FillColor = color
    };

    // Draw the bar on the window
    window.Draw(bar);
}

static void QuickSortAnimation(List<int> numbers, int low, int high, List<List<int>> frames)
{
    if (low < high)
    {
        int pivot = numbers[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (numbers[j] <= pivot)
            {
                i++;
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
                frames.Add(new List<int>(numbers)); // Save state after swap
            }
        }

        (numbers[i + 1], numbers[high]) = (numbers[high], numbers[i + 1]);
        frames.Add(new List<int>(numbers)); // Save state after pivot placement

        int pi = i + 1;
        QuickSortAnimation(numbers, low, pi - 1, frames);
        QuickSortAnimation(numbers, pi + 1, high, frames);
    }
}
