namespace StockFighter.Solutions
{
    /// <summary>
    /// A class implementing this interface must (attempt to) solve a level and
    /// report its solvedness.
    /// </summary>
    public interface ISolvable<T>
    {
        /// <summary>
        /// A method to solve the level.
        /// </summary>
        /// <returns>Returns if the level was solved or not.</returns>
        bool Solve();

        /// <summary>
        /// A constructor that requires an apiKey.
        /// </summary>
        /// <param name="apiKey">An API key.</param>
        /// <returns>Returns an initialized instance of T.</returns>
        T T(string apiKey);
    }
}
