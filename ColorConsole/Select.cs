namespace ColorConsole
{
    public static partial class CConsole
    {
        /// <summary>
        /// Prompt básico para múltiplas escolhas.
        /// Usa o método virtual ToString().
        /// </summary>
        /// <typeparam name="T">Any</typeparam>
        /// <param name="message">Mensagem da seleção</param>
        /// <param name="choices">Enumerado com os valores</param>
        /// <returns>Number</returns>
        public static int SelectNumber<T>(string message, IEnumerable<T> choices)
        {
            return Input.InternalSelect(message, choices).Number;
        }

        /// <summary>
        /// Prompt básico para múltiplas escolhas.
        /// Usa o método virtual ToString().
        /// </summary>
        /// <typeparam name="T">Any</typeparam>
        /// <param name="message">Mensagem da seleção</param>
        /// <param name="choices">Enumerado com os valores</param>
        /// <returns>Value</returns>
        public static T Select<T>(string message, IEnumerable<T> choices)
        {
            return Input.InternalSelect(message, choices).Value;
        }

        /// <summary>
        /// Seleção que usa as Arrow keys para múltiplas escolhas.
        /// Usa o método virtual ToString().
        /// </summary>
        /// <typeparam name="T">Any</typeparam>
        /// <param name="message">Mensagem da seleção</param>
        /// <param name="choices">Enumerado com os valores</param>
        /// <returns>Number</returns>
        public static int SelectableNumber<T>(string message, IEnumerable<T> choices)
        {
            return Input.InternalSelect(message, choices, null, true).Number;
        }

        /// <summary>
        /// Seleção que usa as Arrow keys para múltiplas escolhas.
        /// Usa o método virtual ToString().
        /// </summary>
        /// <typeparam name="T">Any</typeparam>
        /// <param name="message">Mensagem da seleção</param>
        /// <param name="choices">Enumerado com os valores</param>
        /// <returns>Value</returns>
        public static T Selectable<T>(string message, IEnumerable<T> choices)
        {
            return Input.InternalSelect(message, choices, null, true).Value;
        }
    }
}