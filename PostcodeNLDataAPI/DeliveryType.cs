namespace PostcodeNLDataAPI;

/// <summary>
/// Specifies the delivery types.
/// </summary>
public enum DeliveryType
{
    /// <summary>
    /// Complete (new) database delivery.
    /// </summary>
    Complete,
    /// <summary>
    /// Mutation (update) to a previous database delivery.
    /// </summary>
    Mutation
}
