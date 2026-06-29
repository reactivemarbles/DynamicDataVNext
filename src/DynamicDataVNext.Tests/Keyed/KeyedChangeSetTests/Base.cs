using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public class Base
{
    protected static int SelectKey(int item)
        => item + 100;
        
    protected static KeyedItem<int, int> SelectKeyedItem(int item)
        => new()
        {
            Key     = SelectKey(item),
            Item    = item
        };
        
    protected static KeyValuePair<int, int> SelectKeyValuePair(int item)
        => new(
            key:    SelectKey(item),
            value:  item);
}
