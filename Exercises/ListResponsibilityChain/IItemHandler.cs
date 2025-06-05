namespace Exercises.ListResponsibilityChain;

internal interface IItemHandler
{
    IItemHandler Next { get; set; }
    void Handle(string items);
}
