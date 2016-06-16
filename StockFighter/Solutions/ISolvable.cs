namespace StockFighter.Solutions
{
    /// <summary>
    /// A class implementing this interface must (attempt to) solve a level and
    /// report its solvedness.
    /// </summary>
    public interface ISolvable
    {
        /// <summary>
        /// A method to solve the level.
        /// </summary>
        /// <returns>Returns if the level was solved or not.</returns>
        bool Solve();
    }
}
