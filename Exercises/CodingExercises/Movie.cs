namespace Exercises.CodingExercises;

internal class Movie
{
    // MARK: Write your solution in this class
    public string title;
    public string rating;
    public int reviewScore;

    // 1
    private bool fresh { get { return reviewScore > 75; } }

    public Movie(string title, string rating, int score)
    {
        this.title = title;
        this.reviewScore = score;
        this.rating = rating;
    }

    // 2
    public override string ToString()
    {
        // 3
        string description = $"-> {title} \n\tRated {rating} \n\t{reviewScore}% on RT";

        // 4
        if (fresh)
        {
            description += " - Certified Fresh!";
        }

        // 5
        return description;
    }
}
