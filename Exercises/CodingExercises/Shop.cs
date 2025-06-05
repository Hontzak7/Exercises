namespace Exercises.CodingExercises;

internal class Shop
{
    private Dictionary<int, string> _inventory = new Dictionary<int, string>();

    // 1
    public string this[int index]
    {
        // 2
        get
        {
            // 3
            string itemName;
            if (_inventory.TryGetValue(index, out itemName))
            {
                return itemName;
            }

            throw new ArgumentException("Inventory item missing or unassigned...");
        }

        // 4
        set
        {
            //
            if (_inventory.ContainsKey(index))
            {
                throw new ArgumentException("Duplicate inventory item found...");
            }


            _inventory[index] = value;
        }
    }
}
