namespace Exercises.Iterator;

using System.Collections;
using System.Collections.Generic;

internal class DaysInMonthCollection : IEnumerable<MonthWithDays>
{
    public IEnumerator<MonthWithDays> GetEnumerator()
    {
        return new DaysInMonthEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}