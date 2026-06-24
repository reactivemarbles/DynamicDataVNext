using System;

namespace DynamicDataVNext;

/// <summary>
/// Describes the basic structural representation of a single-item change.
/// </summary>
/// <typeparam name="TChangeType">An enum describing the types of change actions that can be represented by this structure.</typeparam>
/// <remarks>
/// This exists to allow for certain bits of logic regarding how changesets are built to be shared between all different types of collections. I.E. it's a compatibility tag interface, it's not intended to allow many different types of changes to be grouped together. It should basically never be used by public consumers, unless you're building your own custom type of collection. 
/// </remarks>
public interface IChange<out TChangeType>
    where TChangeType : Enum
{
    /// <summary>
    /// The category of change action being represented.
    /// </summary>
    ChangeCategory Category { get; }
    
    /// <summary>
    /// The type of change action being represented.
    /// </summary>
    TChangeType Type { get; }
}
