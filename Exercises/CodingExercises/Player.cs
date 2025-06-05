namespace Exercises.CodingExercises;

internal class Player
{
    public string firstname;
    public string lastname;
    public int currentScore;
    public int lastScore;

    public Player(string first, string last, int score, int previous)
    {
        firstname = first;
        lastname = last;
        currentScore = score;
        lastScore = previous;
    }
}
