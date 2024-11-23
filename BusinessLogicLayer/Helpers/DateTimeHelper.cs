using BusinessLogicLayer.Exceptions;

namespace BusinessLogicLayer.Helpers {

    /// <summary>
    /// Has functions to help with date time.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class DateTimeHelper {

        /// <summary>
        /// Parse time as string. Valid input for time is HH:MM.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="InvalidInputException"></exception>
        public static (int hour, int minute) ParseTime(string text) {

            string[] parts = text.Split(':');
            if (parts.Length != 2) {
                throw new InvalidInputException("Vrijeme mora biti u formatu SATI:MINUTE!");
            }

            if (!int.TryParse(parts[0], out int hour)) {
                throw new InvalidInputException("Vrijeme mora biti u formatu SATI:MINUTE!");
            }

            if (!int.TryParse(parts[1], out int minute)) {
                throw new InvalidInputException("Vrijeme mora biti u formatu SATI:MINUTE!");
            }

            if(hour < 0 || hour >= 24) {
                throw new InvalidInputException("Broj sati mora biti između 0 i 23!");
            }

            if (minute < 0 || minute >= 60) {
                throw new InvalidInputException("Broj minuta mora biti između 0 i 59!");
            }

            return (hour, minute);
        }

    }
}
