namespace Exercises.Iterator;

using System.Collections;
using System.Collections.Generic;
internal class DaysInMonthEnumerator : IEnumerator<MonthWithDays>
{
    private int year = 1;
    private int month = 0;

    public MonthWithDays Current =>
        new MonthWithDays()
        {
            Date = $"{year.ToString().PadLeft(4, '0')}-{month}",
            Days = DateTime.DaysInMonth(year, month)
        };

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        month++;
        if (month > 12)
        {
            month = 1;
            year++;
        }
        return year < 5;
    }

    public void Reset()
    {
        year = 0;
        month = 1;
    }
}